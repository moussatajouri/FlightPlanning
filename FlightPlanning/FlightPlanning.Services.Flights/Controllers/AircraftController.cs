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
    public class AircraftController : ControllerBase
    {
        private readonly IAircraftService _aircraftService;

        public AircraftController(IAircraftService aircraftService)
        {
            _aircraftService = aircraftService ?? throw new ArgumentNullException(nameof(aircraftService));

        }

        [HttpGet]
        [Route("All")]
        public IActionResult GetAllAircrafts()
        {
            var aircrafts = _aircraftService.GetAllAircrafts();

            return Ok(aircrafts);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var aircraft = _aircraftService.GetAircraftById(id);

            return Ok(aircraft);
        }

        [HttpPost]
        public IActionResult Post([FromBody] AircraftDto aircraft)
        {
            _aircraftService.InsertAircraft(aircraft);

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] AircraftDto aircraft)
        {
            _aircraftService.UpdateAircraft(aircraft);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Put(int id)
        {
            _aircraftService.DeleteAircraft(id);

            return Ok();
        }
    }
}
