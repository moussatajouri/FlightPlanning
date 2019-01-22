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
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService ?? throw new ArgumentNullException(nameof(flightService));

        }

        [HttpGet]
        [Route("All")]
        public IActionResult GetAllFlights()
        {
            var flights = _flightService.GetAllFlights();

            return Ok(flights);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var flight = _flightService.GetFlightById(id);

            return Ok(flight);
        }

        [HttpGet("detail/{id}")]
        public IActionResult GetDetail(int id)
        {
            var flight = _flightService.GetDetailedFlightDtoById(id);

            return Ok(flight);
        }

        [HttpPost]
        public IActionResult Post([FromBody] FlightDto flight)
        {
            _flightService.InsertFlight(flight);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] FlightDto flight)
        {
            _flightService.UpdateFlight(flight);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Put(int id)
        {
            _flightService.DeleteFlight(id);

            return Ok();
        }
    }
}
