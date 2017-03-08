using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : WorldObjectParent
{
    private static float begin_time = 6, end_time = 18, current_time_hours = 0, current_time_minutes = 0;
    private static int current_day = 0, journey = 0, total_days = 0;
    private static float day_length_in_seconds = 20f, quarter_hour_length_in_seconds, time_counter = 0f;
    private bool paused = false;
    private static bool loaded_game_data = false;

    // Use this for initialization
    void Start()
    {
        base.Init();
        LoadGameData();
        quarter_hour_length_in_seconds = day_length_in_seconds / ((end_time - begin_time) * 4);
    }

    private void LoadGameData()
    {
        if (!loaded_game_data)
        {
            begin_time = PersistenceLayer.GetGameDataByName("begin_time");
            end_time = PersistenceLayer.GetGameDataByName("end_time");
            day_length_in_seconds = PersistenceLayer.GetGameDataByName("level_duration");
            loaded_game_data = true;
        }
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
        Debug.Log("hour: " + current_time_hours + " --- minutes: " + current_time_minutes);
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
                    current_time_hours += 1;
                    current_time_minutes = 0;
                    if (current_time_hours == end_time)
                    {
                        EndDay();
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        UpdateTime();
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
