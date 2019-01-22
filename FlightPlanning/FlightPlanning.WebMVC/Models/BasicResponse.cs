using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.Models
{
    public class BasicResponse
    {
        public Status Status { get; set; }
        public string Message { get; set; }
    }
}
