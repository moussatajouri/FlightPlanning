using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.Services.Flights.BusinessLogic;
using FlightPlanning.Services.Flights.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanning.Services.Flights.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly IAirportService _airportService;

        public AirportController(IAirportService airportService)
        {
            _airportService = airportService ?? throw new ArgumentNullException(nameof(airportService));

        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAllAirports()
        {
            var airports = _airportService.GetAllAirports();

            return Ok(airports);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var airport = _airportService.GetAirportById(id);

            return Ok(airport);
        }

        [HttpPost]
        public IActionResult Post([FromBody] AirportDto airport)
        {
            _airportService.InsertAirport(airport);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] AirportDto airport)
        {
            _airportService.UpdateAirport(airport);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _airportService.DeleteAirport(id);

            return Ok();
        }
    }
}
