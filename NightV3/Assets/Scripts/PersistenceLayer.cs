using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Globalization;

public class PersistenceLayer : MonoBehaviour
{
    private static Dictionary<string, float> game_data = new Dictionary<string, float>();

    public static void Save()
    {
        Dictionary<string, float> environment_data = World.GetEnvironmentData();
        foreach (KeyValuePair<string, float> data in environment_data)
        {
            PlayerPrefs.SetFloat(data.Key, data.Value);
        }
    }

    public static void LoadPlayerData()
    {
        World.LoadEnvironmentData();
    }

    //Use me only when the player loses the game
    public static void Clear()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void LoadGameData()
    {
        TextAsset game_data_file = Resources.Load<TextAsset>("game_data");
        List<string> data_lines = new List<string>(Regex.Split(game_data_file.text, "\n|\r|\r\n"));
        for (int i = data_lines.Count - 1; i >= 0; --i)
        {
            if (data_lines[i].Trim() == "")
            {
                data_lines.RemoveAt(i);
            }
        }
        for (int i = 0; i < data_lines.Count; ++i)
        {
            string next_line = data_lines[i];
            if (next_line.Trim() != "")
            {
                if (next_line.Contains("WEATHER"))
                {
                    string weather_type = next_line.Split(' ')[1];
                    ++i;
                    next_line = data_lines[i];
                    while (next_line.Trim() != "END")
                    {
                        string[] weather_data = next_line.Split(':');
                        if (weather_data.Length == 6)
                        {
                            string weather_name = weather_data[0];
                            float food_mod = float.Parse(weather_data[1]);
                            float water_mod = float.Parse(weather_data[2]);
                            float temperature_mod = float.Parse(weather_data[3]);
                            float weather_danger = float.Parse(weather_data[4]);
                            float weather_severity = float.Parse(weather_data[5]);
                            WeatherController.CreateWeatherType(weather_type, weather_name, food_mod, water_mod, temperature_mod, weather_danger, weather_severity);
                        }
                        else
                        {
                            Debug.Log("incomplete weather data " + next_line);
                        }
                        ++i;
                        next_line = data_lines[i];
                    }
                }
                else if (next_line.Contains("ENVIRONMENT"))
                {
                    string environment_class = next_line.Split(' ')[1];
                    ++i;
                    next_line = data_lines[i];
                    while (next_line.Trim() != "END")
                    {
                        string[] environment_data = next_line.Split(':');
                        if (environment_data.Length == 9)
                        {
                            string environment_name = environment_data[0];
                            float fuel = float.Parse(environment_data[1]);
                            float water = float.Parse(environment_data[2]);
                            float food = float.Parse(environment_data[3]);
                            //TODO implement scrap
                            // float scrap = float.Parse(environment_data[4]);
                            float condition = float.Parse(environment_data[4]);
                            float wet_severity = float.Parse(environment_data[5]);
                            float dry_severity = float.Parse(environment_data[6]);
                            float min_temp = float.Parse(environment_data[7]);
                            float max_temp = float.Parse(environment_data[8]);
                            EnvironmentController.CreateEnvironmentType(environment_class, environment_name, fuel, water, food, condition, wet_severity, dry_severity, min_temp, max_temp);
                        }
                        else
                        {
                            Debug.Log("incomplete environment data " + next_line);
                        }
                        ++i;
                        next_line = data_lines[i];

                    }
                }
                else
                {
                    string[] line = next_line.Split(':');
                    string key = line[0];
                    float val = float.Parse(line[1].Trim());
                    game_data[key] = val;
                }
            }
        }
    }

    public static float GetGameDataByName(string key)
    {
        return game_data[key];
    }
}
