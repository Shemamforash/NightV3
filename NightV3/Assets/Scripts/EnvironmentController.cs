using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    private static List<Environment> environment_types = new List<Environment>();

    public static void CreateEnvironmentType(string environment_class, string name, float fuel, float water, float food, float condition, float wet_severity, float dry_severity, float min_temp, float max_temp)
    {
        Environment new_type = new Environment(environment_class, name, fuel, water, food, condition, dry_severity, wet_severity, min_temp, max_temp);
        environment_types.Add(new_type);
    }

    private class Environment
    {
        private string environment_class, name;
        private float fuel, water, food, scrap, condition, dry_severity, wet_severity, min_temp, max_temp;

        public Environment(string environment_class, string name, float fuel, float water, float food, float condition, float wet_severity, float dry_severity, float min_temp, float max_temp)
        {
            this.environment_class = environment_class;
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
    }
}
