using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WeatherApp3
{
    internal class CityViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public City city;
        public string err;

        List<City> data;
        public ICommand LoadDataCommand { protected set; get; }
        public ICommand LoadNewCityCommand { protected set; get; }
        public CityViewModel()
        {
            city = new City();
            err = "test";
            this.LoadDataCommand = new Command(LoadData);
            this.LoadNewCityCommand = new Command(LoadNewCity);
        }

        private async void LoadData()
        {
            string lat = this.Latitude.ToString().Replace(",", ".");
            string lon = this.Longitude.ToString().Replace(",", ".");
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m," +
                $"relative_humidity_2m,is_day,precipitation,surface_pressure,wind_speed_10m," +
                $"wind_direction_10m&timezone=Europe%2FMoscow&forecast_days=1";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(client.BaseAddress);
                response.EnsureSuccessStatusCode();// выброс исключения, если произошла ошибка

                // десериализация ответа в формате json
                var content = await response.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(content);
                var str = o.SelectToken(@"$.current");
                var current_weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(str.ToString());

                this.Time = current_weatherInfo.Time;
                this.Temperature = current_weatherInfo.Temperature_2m;
                this.Windspeed = current_weatherInfo.Wind_speed_10m;
                this.Relative_humidity_2m = current_weatherInfo.Relative_humidity_2m;
                if (current_weatherInfo.Is_day == 1)
                {
                    this.Is_day = "День";
                }else
                {
                    this.Is_day = "Ночь";
                }
            }
            catch (Exception ex)
            {
                this.Windspeed = 404;
            }
        }

        public async void LoadNewCity()
        {
            string name = this.Name;
            if (!string.IsNullOrEmpty(name))
            {
                string url = $"https://geocoding-api.open-meteo.com/v1/search?name={name}&language=ru";
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(url);
                    var response = await client.GetAsync(client.BaseAddress);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    JObject jobj = JObject.Parse(content);
                    var str = jobj.SelectToken(@"$.results[0]");
                    if (str != null)
                    {
                        var current_cityInfo = JsonConvert.DeserializeObject<City>(str.ToString());

                        City city1 = new City();
                        city1.Name = current_cityInfo.Name;
                        city1.Latitude = current_cityInfo.Latitude;
                        city1.Longitude = current_cityInfo.Longitude;
                        LoadCities();
                        data.Add(city1);
                        string s = JsonConvert.SerializeObject(data);
                        _ = SecureStorage.SetAsync("City", s);
                    }                    
                }
                catch (Exception ex) 
                { }
            }
        }

        public async void LoadCities()
        {
            // Читаем список городов из локального хранилища
            string json = await SecureStorage.GetAsync("City");
            if (!string.IsNullOrEmpty(json))
                data = JsonConvert.DeserializeObject<List<City>>(json);
        }

        public string Name
        {
            get { return city.Name; }
            set {city.Name = value; OnPropertyChanged("Name"); }
        }

        public double Latitude
        {
            get { return city.Latitude; }
            set { city.Latitude = value; OnPropertyChanged("Latitude"); }
        }

        public double Longitude
        {
            get { return city.Longitude; }
            set { city.Longitude = value;OnPropertyChanged("Longitude"); }
        }

        public string Time
        {
            get { return city.Time; }
            set { city.Time = value;OnPropertyChanged("Time"); }
        }

        public double Temperature
        {
            get { return city.Temperature; }
            set { city.Temperature = value;OnPropertyChanged("Temperature"); }
        }

        public double Windspeed
        {
            get { return city.Windspeed; }
            set { city.Windspeed = value;OnPropertyChanged("Windspeed"); }
        }

        public double Relative_humidity_2m
        {
            get { return city.Relative_humidity_2m; }
            set { city.Relative_humidity_2m = value; OnPropertyChanged("Relative_humidity_2m"); }
        }

        public string Is_day
        {
            get { return city.Is_day; }
            set { city.Is_day = value; OnPropertyChanged("Is_day"); }
        }

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
