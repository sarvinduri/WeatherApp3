using Newtonsoft.Json;

namespace WeatherApp3;

public partial class OptionPage : ContentPage
{
	City MyCity;
	CityViewModel viewModel;
	public OptionPage()
	{
		InitializeComponent();
        this.BindingContext = new CityViewModel();
    }

	private void OnCuttonClear(object sender, EventArgs e)
	{
        SecureStorage.RemoveAll();
        DisplayAlert("�����������", "������ ������� ������!", "�K");
        List<City> cities;
        City city1 = new City();
        city1.Name = "������";
        city1.Latitude = 55.75;
        city1.Longitude = 37.62;
        City city2 = new City();
        city2.Name = "�����-���������";
        city2.Latitude = 59.94;
        city2.Longitude = 30.31;
        City city3 = new City();
        city3.Name = "����";
        city3.Latitude = 43.60;
        city3.Longitude = 39.73;
        cities = new List<City> { city1, city2, city3 };
        string data = JsonConvert.SerializeObject(cities);
        SecureStorage.SetAsync("City", data); // key: City - ��� ��� ��������� ������ ��������
        DisplayAlert("�����������", "��������� 3 ������� ������!", "�K");
    }

	private void EntryCityChanged(object sender, TextChangedEventArgs e)
	{
		this.BindingContext = new CityViewModel
		{
			Name = EntryCity.Text.ToString(),
			Latitude = 0,
			Longitude = 0,
			Time = "",
			Temperature = 0,
			Windspeed = 0,
		};
	}

    private void addNewCity(object sender, EventArgs e)
    {
        DisplayAlert("�����������", "����� ��������!", "�K");
    }
}