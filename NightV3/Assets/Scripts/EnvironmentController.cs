using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    private static List<Environment> environment_types = new List<Environment>();
    private static Environment current_environment;
    private static float bound_range = 0.15f;
    public enum EnvironmentDifficulty { easy, medium, hard };
    private static float current_temperature;
    private static float[] temperature_range;

    public static void CalculateTemperatures()
    {
		int hours = World.GetDayLength() + 1;
        float min_max_diff = current_environment.GetMaxTemp() - current_environment.GetMinTemp();
        float weather_temperature_modifier = WeatherController.GetCurrentWeather().GetTemperatureModifier();
        temperature_range = new float[hours];
        for (int i = 0; i < hours; ++i)
        {
            float temperature_val = Mathf.Sin(i * 0.21f);
            temperature_val *= min_max_diff;
            temperature_val += current_environment.GetMinTemp();
            temperature_val += weather_temperature_modifier;
            //TODO weather temperature modifier
            temperature_range[i] = Mathf.Floor(temperature_val);
        }
    }

    public static Environment GetCurrentEnvironment(){
        return current_environment;
    }

    public static float GetCurrentTemperature(int current_hour)
    {
        return temperature_range[current_hour];
    }

    public static void SetNewEnvironment(float difficulty)
    {
        float lower_bound = difficulty - bound_range;
        float upper_bound = difficulty + bound_range;
        float class_a_chance = 0, class_b_chance = 0, class_c_chance = 0;
        if (upper_bound < 0.3f)
        {
            class_a_chance = 0.3f;
        }
        else if (lower_bound < 0.3f && upper_bound > 0.3f)
        {
            class_a_chance = 0.3f - lower_bound;
            class_b_chance = 0.3f;
        }
        else if (upper_bound < 0.75f && lower_bound > 0.3f)
        {
            class_b_chance = 0.3f;
        }
        else if (upper_bound > 0.75f && lower_bound < 0.75f)
        {
            class_b_chance = 0.75f - lower_bound;
            class_c_chance = 0.3f;
        }
        else
        {
            class_c_chance = 0.3f;
        }
        float random = Random.Range(0f, 0.3f);
        if (random < class_a_chance)
        {
            SetEnvironmentOfDifficulty(EnvironmentDifficulty.easy);
        }
        else if (random < class_b_chance)
        {
            SetEnvironmentOfDifficulty(EnvironmentDifficulty.medium);
        }
        else if (random < class_c_chance)
        {
            SetEnvironmentOfDifficulty(EnvironmentDifficulty.hard);
        }
    }

    private static void SetEnvironmentOfDifficulty(EnvironmentDifficulty desired_difficulty)
    {
        List<Environment> potential_environments = new List<Environment>();
        foreach (Environment e in environment_types)
        {
            if (e.GetDifficulty() == desired_difficulty)
            {
                potential_environments.Add(e);
            }
        }
        current_environment = potential_environments[Random.Range(0, potential_environments.Count)];
    }

    private void GenerateResources()
    {
        //DOSTUFF
    }

    public static void CreateEnvironmentType(string environment_class, string name, float fuel, float water, float food, float condition, float wet_severity, float dry_severity, float min_temp, float max_temp)
    {
        Environment new_type = new Environment(environment_class, name, fuel, water, food, condition, dry_severity, wet_severity, min_temp, max_temp);
        environment_types.Add(new_type);
    }

    public class Environment
    {
        private string name;
        private float fuel, water, food, scrap, condition, dry_severity, wet_severity, min_temp, max_temp;
        private EnvironmentDifficulty difficulty;

        public Environment(string environment_class, string name, float fuel, float water, float food, float condition, float wet_severity, float dry_severity, float min_temp, float max_temp)
        {
            switch (environment_class)
            {
                case "A":
                    this.difficulty = EnvironmentDifficulty.easy;
                    break;
                case "B":
                    this.difficulty = EnvironmentDifficulty.medium;
                    break;
                case "C":
                    this.difficulty = EnvironmentDifficulty.hard;
                    break;
                default:
                    Debug.Log("Invalid Environment Class: " + environment_class);
                    break;
            }
            this.name = name;
            this.fuel = fuel;
            this.food = food;
            this.water = water;
            this.condition = condition;
            this.wet_severity = wet_severity;
            this.dry_severity = dry_severity;
            this.min_temp = min_temp;
            this.max_temp = max_temp;
        }

        public string GetName() {
            return name;
        }

        public float GetCondition(){
            return condition;
        }

        public float GetDrySeverity(){
            return dry_severity;
        }

        public float GetWetSeverity(){
            return wet_severity;
        }

        public EnvironmentDifficulty GetDifficulty()
        {
            return difficulty;
        }

        public float GetMaxTemp()
        {
            return max_temp;
        }

        public float GetMinTemp()
        {
            return min_temp;
        }
    }
}
