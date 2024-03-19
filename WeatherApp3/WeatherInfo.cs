using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp3
{
    internal class WeatherInfo
    {
        public string Time { get; set; }
        public double Temperature_2m { get; set; }
        public double Wind_speed_10m { get; set; }
        public double Relative_humidity_2m { get; set; }
        public int Is_day { get; set; }
    }
}
