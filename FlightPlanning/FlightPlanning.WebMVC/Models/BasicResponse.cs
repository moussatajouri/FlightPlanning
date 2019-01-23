using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightPlanning.WebMVC.Models
{
    public class BasicResponse<T>
    {       
        public Anomaly Anomaly { get; set; }

        public T Data { get; set; }

        public string Status
        {
            get
            {
                return Anomaly == null ? "OK" : "KO";
            }
        }
    }
}
