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

        public virtual DbSet<Aircraft> Aircraft { get; set; }
        public virtual DbSet<Airport> Airport { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aircraft>(entity =>
            {
                entity.Property(e => e.FuelCapacity).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.FuelConsumption).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Speed).HasColumnType("decimal(8, 4)");

                entity.Property(e => e.TakeOffEffort).HasColumnType("decimal(10, 4)");
            });

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
