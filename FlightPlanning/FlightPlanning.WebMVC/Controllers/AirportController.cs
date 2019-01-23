using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.WebMVC.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using FlightPlanning.WebMVC.Models;

namespace FlightPlanning.WebMVC.Controllers
{
    public class AirportController : Controller
    {
        private readonly IAirportService _airportService;

        public AirportController(IAirportService airportService)
        {
            _airportService = airportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAirport()
        {
            return Json(await _airportService.GetAllAirports());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAirport([FromBody]Airport model)
        {
            return Json(await _airportService.InsertAirport(model));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAirport([FromBody]Airport model)
        {
            return Json(await _airportService.UpdateAirport(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAirport([FromBody]int airportId)
        {
            return Json(await _airportService.DeleteAirport(airportId));
        }
    }
}