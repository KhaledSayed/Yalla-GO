using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using CarPooling.DTO;
using CarPooling.Helpers;
using CarPooling.Models;
using CarPooling.Models.enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetAd.Repository;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarPooling.Controllers
{
    [Route("api/trips")]
    public class TripController : Controller
    {
        private readonly ITripRepository tripRepository;
        private readonly IUserRepository userRepository;
        private readonly IDriverRepository driverRepository;
        private readonly IClientTripRepository clientTripRepository;

        public TripController(ITripRepository tripRepository,IUserRepository userRepository,IDriverRepository driverRepository,IClientTripRepository clientTripRepository)
        {
            this.tripRepository = tripRepository;
            this.userRepository = userRepository;
            this.driverRepository = driverRepository;
            this.clientTripRepository = clientTripRepository;
        }
        // GET: api/<controller>
        [HttpGet("check")]
        public async Task<IActionResult> Get([FromQuery] TripCheckQuery query)
        {
            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    var uriBuilder = new UriBuilder("https://maps.googleapis.com/maps/api/directions/json");
                    var uriQuery = HttpUtility.ParseQueryString(uriBuilder.Query);
                    uriQuery["language"] = "ar";
                    uriQuery["key"] = "AIzaSyCezD7oizzzd8QqHw5tVSveIxjyemdpwVU";
                    uriQuery["origin"] = "" + query.OriginLat + "," + query.OriginLng; 
                    uriQuery["destination"] = "" + query.DestinationLat + "," + query.DestinationLng;
                    uriBuilder.Query = uriQuery.ToString();

                    HttpResponseMessage response = await client.GetAsync(uriBuilder.ToString());
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Above three lines can be replaced with new helper method below
                    // string responseBody = await client.GetStringAsync(uri);
                    var x = JsonConvert.DeserializeObject<Directions>(responseBody);

                    return Ok(new TripInfoDto { EncodedRoute = x.Routes[0].OverviewPolyline, Info = x.Routes[0].Legs[0], ExcpectedPayment = await TripHelper.calculateTripFeesRange(x) });
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }

            return Ok();
        }

        // GET api/<controller>/5
        [HttpPost("reserve")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] TripReserveDto tripReserveDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var currentUser = await this.userRepository.FindOneById(int.Parse(userId));
            if (currentUser == null || currentUser.Role.ToString() != userRole.ToString())
            {
                return Unauthorized();
            }

            var originPoint = new Point(tripReserveDto.OriginLocation.Lat,tripReserveDto.OriginLocation.Lng) { SRID = 4326 };
            var destinationPoint = new Point(tripReserveDto.DistanceLocation.Lat, tripReserveDto.DistanceLocation.Lng) { SRID = 4326 };
            var trip = await this.tripRepository.FindTripNearestByLocation(originPoint,destinationPoint);
            var originPlace = new Place { Location = originPoint, Name = tripReserveDto.OriginAddress };
            var destantPlace = new Place { Location = destinationPoint, Name = tripReserveDto.DistantAddress };


            if (trip == null)
            {
                var driver = await driverRepository.NearestOnlineAndFreeDriver(originPoint,500);

                if(driver == null)
                {
                    return UnprocessableEntity();
                }
                else
                {
                    var newTrip = new Trip
                    {
                        StartLocation = originPlace,
                        FinalLocation = destantPlace,
                        DriverId = driver.Id,
                        Status = Models.enums.TripStatus.PICKING_USER,
                    };

                    var clientInTrip = new ClientTrip()
                    {
                        ClientId = currentUser.Id,
                        Status = Models.enums.ClientTripStatus.WAITING_FOR_DRIVER,
                        FromLocation = originPlace,
                        ToLocation = destantPlace,
                        StartedAt = DateTime.Now

                    };

                    var point = new ClientTripPointAtTime { ClientId = currentUser.Id, TripId = trip.Id, Location = originPoint, Time = DateTime.Now };


                    clientInTrip.Points.Add(point);

                    newTrip.Clients.Add(clientInTrip);

                    var expectedPoints = TripHelper.convertFromDirectionToListOfExpectedRoutes(
                       await getDirectionInfo(new TripCheckQuery { OriginLat = tripReserveDto.OriginLocation.Lat, OriginLng = tripReserveDto.OriginLocation.Lng, DestinationLat = tripReserveDto.DistanceLocation.Lat, DestinationLng = tripReserveDto.DistanceLocation.Lng })
                        );

                    //trip.ExpectedRoad = expectedPoints;

                    tripRepository.Add(newTrip);


                    if (await tripRepository.SaveAll())
                    {
                        return Ok(trip);
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
            }
            else
            {

                var clientInTrip = new ClientTrip()
                {
                    ClientId = currentUser.Id,
                    Status = Models.enums.ClientTripStatus.WAITING_FOR_DRIVER,
                    FromLocation = originPlace,
                    ToLocation = destantPlace,
                    StartedAt = DateTime.Now

                };

                var point = new ClientTripPointAtTime { ClientId = currentUser.Id, TripId = trip.Id , Location = originPoint , Time = DateTime.Now  };


                clientInTrip.Points.Add(point);

                trip.Clients.Add(clientInTrip);
                trip.Status = Models.enums.TripStatus.ANOTHER_CLIENT;
                 tripRepository.Update(trip);

                if(await tripRepository.SaveAll())
                {
                    return Ok(trip);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            
        }

        [HttpPut("{id}/clients/{clientId}/pick")]
        [Authorize]
        public async Task<IActionResult> PickClient(int id,int clientId)
        {
            string driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var trip = await this.tripRepository.FindOneById(id);

            if (trip == null) return NotFound();
            if (trip.Status == Models.enums.TripStatus.ENDED) return UnprocessableEntity();

            var driver = await this.userRepository.FindOneById(int.Parse(driverId));

            if (driver == null || driver.Role == UserType.Driver.ToString()) return Unauthorized();

            if (!await this.driverRepository.IsDriverOfTrip(id, driver.Id)) return Unauthorized();

            var clientTrip = trip.Clients.FirstOrDefault(c => c.ClientId == clientId);

            if (clientTrip == null) return BadRequest();

            clientTrip.Status = ClientTripStatus.JOINED;
            clientTrip.StartedAt = DateTime.Now;

            trip.Status = TripStatus.RUNNING;

            this.tripRepository.Update(trip);
            this.clientTripRepository.Update(clientTrip);

            if(await tripRepository.SaveAll() && await  clientTripRepository.SaveAll())
            {
                return Ok(trip);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}/clients/{clientId}/check-payment")]
        [Authorize]
        public async Task<IActionResult> CheckPayment(int id, int clientId)
        {
            string driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var trip = await this.tripRepository.FindOneById(id);

            if (trip == null) return NotFound();
            if (trip.Status == Models.enums.TripStatus.ENDED) return UnprocessableEntity();

            var driver = await this.userRepository.FindOneById(int.Parse(driverId));

            if (driver == null || driver.Role == UserType.Driver.ToString()) return Unauthorized();

            if (!await this.driverRepository.IsDriverOfTrip(id, driver.Id)) return Unauthorized();

            var clientTrip = trip.Clients.FirstOrDefault(c => c.ClientId == clientId);

            if (clientTrip == null) return BadRequest();

            clientTrip.Status = ClientTripStatus.JOINED;
            clientTrip.StartedAt = DateTime.Now;

            trip.Status = TripStatus.RUNNING;

            this.tripRepository.Update(trip);
            this.clientTripRepository.Update(clientTrip);

            if (await tripRepository.SaveAll() && await clientTripRepository.SaveAll())
            {
                return Ok(trip);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}/clients/{clientId}/pay")]
        [Authorize]
        public async Task<IActionResult> Pay(int id, int clientId)
        {
            string driverId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var trip = await this.tripRepository.FindOneById(id);

            if (trip == null) return NotFound();
            if (trip.Status == Models.enums.TripStatus.ENDED) return UnprocessableEntity();

            var driver = await this.userRepository.FindOneById(int.Parse(driverId));

            if (driver == null || driver.Role == UserType.Driver.ToString()) return Unauthorized();

            if (!await this.driverRepository.IsDriverOfTrip(id, driver.Id)) return Unauthorized();

            var clientTrip = trip.Clients.FirstOrDefault(c => c.ClientId == clientId);

            if (clientTrip == null) return BadRequest();

            clientTrip.Status = ClientTripStatus.LEAVED;
            clientTrip.LeavedAt = DateTime.Now;

            trip.Status = TripStatus.RUNNING;

            this.tripRepository.Update(trip);
            this.clientTripRepository.Update(clientTrip);

            if (await tripRepository.SaveAll() && await clientTripRepository.SaveAll())
            {
                return Ok(trip);
            }
            else
            {
                return StatusCode(500);
            }
        }


        public async Task<Directions> getDirectionInfo(TripCheckQuery query)
        {

            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {

                    var uriBuilder = new UriBuilder("https://maps.googleapis.com/maps/api/directions/json");
                    var uriQuery = HttpUtility.ParseQueryString(uriBuilder.Query);
                    uriQuery["language"] = "ar";
                    uriQuery["key"] = "AIzaSyCezD7oizzzd8QqHw5tVSveIxjyemdpwVU";
                    uriQuery["origin"] = "" + query.OriginLat + "," + query.OriginLng;
                    uriQuery["destination"] = "" + query.DestinationLat + "," + query.DestinationLng;
                    uriBuilder.Query = uriQuery.ToString();

                    HttpResponseMessage response = await client.GetAsync(uriBuilder.ToString());
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Above three lines can be replaced with new helper method below
                    // string responseBody = await client.GetStringAsync(uri);
                    var x = JsonConvert.DeserializeObject<Directions>(responseBody);


                    return x;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }

                return null;
            }
        }

    }
}
