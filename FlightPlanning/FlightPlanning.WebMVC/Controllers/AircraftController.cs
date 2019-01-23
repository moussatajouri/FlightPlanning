using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlanning.WebMVC.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using FlightPlanning.WebMVC.Models;

namespace FlightPlanning.WebMVC.Controllers
{
    public class AircraftController : Controller
    {
        private readonly IAircraftService _aircraftService;

        public AircraftController(IAircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAircraft()
        {
            return Json(await _aircraftService.GetAllAircrafts());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAircraft([FromBody]Aircraft model)
        {
            return Json(await _aircraftService.InsertAircraft(model));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAircraft([FromBody]Aircraft model)
        {
            return Json(await _aircraftService.UpdateAircraft(model));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAircraft([FromBody]int aircraftId)
        {
            return Json(await _aircraftService.DeleteAircraft(aircraftId));
        }
    }
}