using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.Services.Flights.BusinessLogic;
using FlightPlanning.Services.Flights.DataAccess;
using FlightPlanning.Services.Flights.Models;
using FlightPlanning.Services.Flights.Transverse;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlightPlanning.Services.Flights
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<FlightsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FlightsDbContext")));

            services.AddScoped<IAirportRepository, AirportRepository>();
            services.AddScoped<IAircraftRepository, AircraftRepository>();
            services.AddScoped<IFlightRepository, FlightRepository>();

            services.AddTransient<IAircraftService, AircraftService>();
            services.AddTransient<IAirportService, AirportService>();
            services.AddTransient<IFlightService, FlightService>();
            services.AddTransient<ICalculator, Calculator>();
            services.AddTransient<IDistanceCalculator, DistanceCalculator>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
