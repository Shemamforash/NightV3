using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class World : WorldObjectParent
{
    private static int begin_time = 6, end_time = 18, current_time_hours = 0;
    private static float current_time_minutes = 0;
    private static int current_day = 0, journey = 0, total_days = 0;
    private static float day_length_in_seconds = 20f, quarter_hour_length_in_seconds, time_counter = 0f;
    private bool paused = false;
    private static bool loaded_game_data = false;
    private float difficulty = 0.1f;
    private Text time_and_day_text, temperature_text, weather_text, environment_text;

    public static int GetDayLength()
    {
        return (int)(end_time - begin_time);
    }

    // Use this for initialization
    void Start()
    {
        base.Init();
        PersistenceLayer.LoadGameData();
        LoadGameData();
        quarter_hour_length_in_seconds = day_length_in_seconds / ((end_time - begin_time) * 4);
        BindObjects();
        EnvironmentController.SetNewEnvironment(difficulty);
        StartNewDay();
    }

    private void BindObjects()
    {
        time_and_day_text = GameObject.Find("Time and Day").GetComponent<Text>();
        temperature_text = GameObject.Find("Temperature").GetComponent<Text>();
        weather_text = GameObject.Find("Weather").GetComponent<Text>();
        environment_text = GameObject.Find("Environment").GetComponent<Text>();
    }

    private void LoadGameData()
    {
        if (!loaded_game_data)
        {
            begin_time = (int)PersistenceLayer.GetGameDataByName("begin_time");
            current_time_hours = begin_time;
            end_time = (int)PersistenceLayer.GetGameDataByName("end_time");
            day_length_in_seconds = PersistenceLayer.GetGameDataByName("level_duration");
            loaded_game_data = true;
        }
    }

    private void StartNewDay()
    {
        WeatherController.SetNewWeather();
        EnvironmentController.CalculateTemperatures();
        weather_text.text = WeatherController.GetCurrentWeather().GetName();
        environment_text.text = EnvironmentController.GetCurrentEnvironment().GetName();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown("space"))
        {
            paused = !paused;
        }
    }

    public static Dictionary<string, float> GetEnvironmentData()
    {
        Dictionary<string, float> environment_data = new Dictionary<string, float>();
        environment_data["current_day"] = current_day;
        environment_data["journey"] = journey;
        environment_data["total_days"] = total_days;
        return environment_data;
    }

    public static void LoadEnvironmentData()
    {
        current_day = (int)PlayerPrefs.GetFloat("current_day");
        journey = (int)PlayerPrefs.GetFloat("journey");
        total_days = (int)PlayerPrefs.GetFloat("total_days");
    }

    private void UpdateTime()
    {
        if (!paused)
        {
            time_counter += Time.deltaTime;
            if (time_counter >= quarter_hour_length_in_seconds) //Multiply by 1000 for millis
            {
                float delta = time_counter - quarter_hour_length_in_seconds;
                time_counter = delta;
                current_time_minutes += 0.15f;
                if (current_time_minutes == 0.6f)
                {
                    UpdateHours();
                    current_time_minutes = 0;
                    if (current_time_hours == end_time)
                    {
                        EndDay();
                    }
                }
            }
        }
    }

    private static void UpdateHours()
    {
        ++current_time_hours;
    }

    public float GetDifficulty()
    {
        return difficulty;
    }

    public float GetTime()
    {
        return current_time_hours;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        UpdateTime();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (time_and_day_text != null)
        {
            time_and_day_text.text = "Day: " + current_day + " " + current_time_hours.ToString("00") + ":" + ((int)(current_time_minutes * 100f)).ToString("00");
            temperature_text.text = EnvironmentController.GetCurrentTemperature(current_time_hours - begin_time) + "\u00B0" + "C";
        }
    }

    private void EndDay()
    {
        ++current_day;
        ++total_days;
        current_time_hours = begin_time;
        current_time_minutes = 0;
        time_counter = 0;
        menu_navigator.LoadEndDay();
    }
}
