using System;
using FlightPlanning.Services.Flights.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlightPlanning.Services.Flights.DataAccess
{
    public partial class FlightsDbContext : DbContext
    {
        public FlightsDbContext()
        {
        }

        public FlightsDbContext(DbContextOptions<FlightsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Airport> Airport { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airport>(entity =>
            {
                entity.HasIndex(e => e.CountryName)
                    .HasName("AirportCountryNameIndex");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Iata)
                    .HasColumnName("IATA")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Icao)
                    .HasColumnName("ICAO")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude).HasColumnType("decimal(18, 15)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(18, 15)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });
        }
    }
}
