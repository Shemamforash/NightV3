using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

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
        TextAsset game_data_file = (TextAsset)Resources.Load("game_data.txt");
        string[] data_lines = Regex.Split(game_data_file.text, "\n|\r|\r\n");
        foreach (string next_line in data_lines)
        {
            if (next_line.Trim() != "")
            {
                string[] line = next_line.Split(':');
                string key = line[0];
                float val = float.Parse(line[1].Trim());
                game_data.Add(key, val);
            }
        }
    }

	public static float GetGameDataByName(string key){
		return game_data[key];
	}
}
