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
            try
            {
                var airports = await _airportService.GetAllAirports();
                return Json(airports);
            }
            catch
            {
                return Json("KO");
            }
        }

        [HttpPost]
        public IActionResult CreateAirport([FromBody]Airport model)
        {
            try
            {
                _airportService.InsertAirport(model);
                return Json(new BasicResponse { Status = Status.KO});
            }
            catch
            {
                return Json(new BasicResponse { Status= Status.KO, Message ="Unhandler Error"});
            }
        }

        [HttpPost]
        public IActionResult UpdateAirport([FromBody]Airport model)
        {
            try
            {
                _airportService.UpdateAirport(model);
                return Json(new BasicResponse { Status = Status.KO });
            }
            catch
            {
                return Json(new BasicResponse { Status = Status.KO, Message = "Unhandler Error" });
            }

        }

        [HttpPost]
        public IActionResult DeleteAirport(int airportId)
        {
            try
            {
                _airportService.DeleteAirport(airportId);
                return Json(new BasicResponse { Status = Status.KO });
            }
            catch
            {
                return Json(new BasicResponse { Status = Status.KO, Message = "Unhandler Error" });
            }

        }
    }
}