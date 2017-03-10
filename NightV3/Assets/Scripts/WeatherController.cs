using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    private static List<Weather> weather_types = new List<Weather>();
    private static Weather current_weather;
    public List<GameObject> weather_systems;
    private enum WeatherWetness { dry, wet, misc };

    public static void CreateWeatherType(string type, string name, float food_mod, float water_mod, float temperature_mod, float weather_danger, float weather_severity)
    {
        Weather new_type = new Weather(type, name, food_mod, water_mod, temperature_mod, weather_danger, weather_severity);
        weather_types.Add(new_type);
    }

	public static void SetNewWeather(float environment_severity, float environment_condition) {
		float misc_chance = Random.Range(0f, 1f);
		float environment_quality = Random.Range(0f, 1f);
		if(misc_chance < 0.1f){
			PickWeather(environment_severity, WeatherWetness.misc);
		} else if(environment_quality > environment_condition){
			PickWeather(environment_severity, WeatherWetness.wet);
		} else {
			PickWeather(environment_severity, WeatherWetness.dry);
		}
	}

	private static void PickWeather(float severity, WeatherWetness wetness){
		List<Weather> potential_weathers = new List<Weather>();
		foreach(Weather w in weather_types){
			if(w.GetWetness() == wetness){
				float lower_bound = severity - 0.4f;
				float upper_bound = severity + 0.4f;
				if(w.GetSeverity() >= lower_bound && w.GetSeverity() <= upper_bound) {
					potential_weathers.Add(w);
				}
			}
		}
		current_weather = potential_weathers[Random.Range(0, potential_weathers.Count)];
	}

    private class Weather
    {
        private string name;
        private float food_mod, water_mod, temperature_mod, weather_danger, weather_severity;
        private WeatherWetness wetness;

        public Weather(string type, string name, float food_mod, float water_mod, float temperature_mod, float weather_danger, float weather_severity)
        {
            switch (type)
            {
                case "DRY":
                    wetness = WeatherWetness.dry;
                    break;
                case "WET":
                    wetness = WeatherWetness.wet;
                    break;
                case "MISC":
                    wetness = WeatherWetness.misc;
                    break;
                default:
                    Debug.Log("Unknown weather type: " + type);
                    break;
            }
            this.name = name;
            this.food_mod = food_mod;
            this.water_mod = water_mod;
            this.temperature_mod = temperature_mod;
            this.weather_danger = weather_danger;
            this.weather_severity = weather_severity;
        }

		public WeatherWetness GetWetness(){
			return wetness;
		}

		public float GetSeverity(){
			return weather_severity;
		}
    }
}
