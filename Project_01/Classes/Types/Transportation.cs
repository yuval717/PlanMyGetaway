using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_01
{
    public class Transportation
    {
        public Attraction FromAttraction { get; set; }
        public Attraction ToAttraction { get; set; }
        public double TravelTime { get; set; } 
        public string TravelWay { get; set; }

        public Transportation (Attraction FromAttraction, Attraction ToAttraction, double TravelTime, string TravelWay)
        {
            this.FromAttraction = FromAttraction;
            this.ToAttraction = ToAttraction;
            this.TravelTime = TravelTime;
            this.TravelWay = TravelWay;
        }

    }
}