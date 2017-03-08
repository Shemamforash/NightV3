using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    private static List<Weather> weather_types = new List<Weather>();

    public static void CreateWeatherType(string type, string name, float food_mod, float water_mod, float temperature_mod, float weather_danger, float weather_severity)
    {
		Weather new_type = new Weather(type, name, food_mod, water_mod, temperature_mod, weather_danger, weather_severity);
		weather_types.Add(new_type);
    }

    private class Weather
    {
        private string type, name;
        private float food_mod, water_mod, temperature_mod, weather_danger, weather_severity;

        public Weather(string type, string name, float food_mod, float water_mod, float temperature_mod, float weather_danger, float weather_severity)
        {
            this.type = type;
            this.name = name;
            this.food_mod = food_mod;
            this.water_mod = water_mod;
            this.temperature_mod = temperature_mod;
            this.weather_danger = weather_danger;
            this.weather_severity = weather_severity;
        }
    }
}
