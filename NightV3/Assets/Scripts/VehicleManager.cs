using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public class Survivor
    {
        private string name, background;
        private int age, weight;
        private float thirst, hunger, dehydration_tolerance, starvation_tolerance, required_water, required_food, current_strength, max_strength;
        private float water_find_skill, hunting_skill, scavenging_skill, skill_modifier;
        private float fuel_requirement, preferred_temperature;
        private WeatherController.WeatherWetness preferred_weather_type;
        private EnvironmentController.EnvironmentDifficulty preferred_environment_type;
        private SurvivorTrait primary_trait, secondary_trait;
        private Gender gender;

        private enum Gender { male, female };

        public Survivor()
        {

        }

    }

    public class SurvivorTrait
    {

    }
}
