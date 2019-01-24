using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.WebMVC.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using FlightPlanning.WebMVC.Models;

namespace FlightPlanning.WebMVC.Controllers
{
    public class FlightController : Controller
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFlightsDetails()
        {
            return Json(await _flightService.GetAllFlightsDetails());
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateFlight([FromBody]Flight model)
        {
            return Json(await _flightService.InsertFlight(model));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFlight([FromBody]Flight model)
        {
            return Json(await _flightService.UpdateFlight(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFlight([FromBody]int flightId)
        {
            return Json(await _flightService.DeleteFlight(flightId));
        }
    }
}