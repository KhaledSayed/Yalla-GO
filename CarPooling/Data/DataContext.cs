using CarPooling.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPooling.Data
{ 
    public class DataContext : DbContext
    {

        public IConfiguration Configuration { get; }


        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>()
                .HasMany<TripPointAtTime>(t => t.Points)
                .WithOne(tpt => tpt.Trip)
                .HasForeignKey(tpt => tpt.TripId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string con = Configuration["ConnectionStrings:Default"];
                optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:Default"],
                    x => x.UseNetTopologySuite());
              
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<Place> Places { get; set; }

        public DbSet<ClientTrip> ClientTrips { get; set; }

        public DbSet<ChatTrip> ChatTrips { get; set; }

        public DbSet<TripPointAtTime> TripPoints { get; set; }

        public DbSet<ClientTripPointAtTime> ClientInTripPoints { get; set; }
    }
}
