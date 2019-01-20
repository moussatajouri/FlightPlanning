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
        public virtual DbSet<Flight> Flight { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aircraft>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UK_Aircraft_Name")
                    .IsUnique();

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

                entity.HasIndex(e => e.Iata)
                    .HasName("UK_Airport_Iata")
                    .IsUnique();

                entity.HasIndex(e => e.Icao)
                    .HasName("UK_Airport_Icao")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("UK_Airport_Name")
                    .IsUnique();

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

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.HasOne(d => d.Aircraft)
                    .WithMany(p => p.Flight)
                    .HasForeignKey(d => d.AircraftId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.AirportDeparture)
                    .WithMany(p => p.FlightAirportDeparture)
                    .HasForeignKey(d => d.AirportDepartureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Flight_AirportDeparture_AirportId");

                entity.HasOne(d => d.AirportDestination)
                    .WithMany(p => p.FlightAirportDestination)
                    .HasForeignKey(d => d.AirportDestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Flight_AirportDestination_AirportId");
            });
        }
    }
}
