using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CarPooling.DTO;
using CarPooling.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarPooling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            using (HttpClient client = new HttpClient())
            {
                // Call asynchronous network methods in a try/catch block to handle exceptions
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://maps.googleapis.com/maps/api/directions/json?origin=Golf City Mall&destination=Modern Academyin Maadi&key=AIzaSyCezD7oizzzd8QqHw5tVSveIxjyemdpwVU&language=ar");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Above three lines can be replaced with new helper method below
                    // string responseBody = await client.GetStringAsync(uri);
                    var x =  JsonConvert.DeserializeObject<Directions>(responseBody);
                    return Ok(new TripInfoDto { EncodedRoute = x.Routes[0].OverviewPolyline , Info = x.Routes[0].Legs[0] , ExcpectedPayment = await TripHelper.calculateTripFeesRange(x)});
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }

            return null;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
