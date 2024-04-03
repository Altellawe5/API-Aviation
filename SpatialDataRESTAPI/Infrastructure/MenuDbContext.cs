using Domain.Models;
using Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MenuDbContext : DbContext
    {
        public MenuDbContext(DbContextOptions<MenuDbContext> options) : base(options)
        {

        }

        
        public DbSet<AirportEF> Airport { get; set; }
        public DbSet<EnrouteWayPointEF> EnrouteWaypoint { get; set; }
        public DbSet<VhfNavaidEF> VhfNavaid { get; set; }
        public DbSet<AirwaysEF> EnrouteAirwaysLines { get; set; }
        public DbSet<FirUirEF> FirUir { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source=.\\SQLEXPRESS;Initial Catalog=NavSpatialData;Integrated Security=True;TrustServerCertificate=True",
                x => x.UseNetTopologySuite());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Nav");

            modelBuilder.Entity<EnrouteWayPointEF>().HasNoKey();

            modelBuilder.Entity<VhfNavaidEF>().HasNoKey();

            modelBuilder.Entity<AirwaysEF>().HasNoKey();

            modelBuilder.Entity<FirUirEF>().HasNoKey();



        }



    }
}
