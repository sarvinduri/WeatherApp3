﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp3
{
    internal class City
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Time { get; set; }
        public double Temperature { get; set; }
        public double Windspeed { get; set; }
        public double Relative_humidity_2m { get; set; }
        public string Is_day { get; set; }

    }
}
