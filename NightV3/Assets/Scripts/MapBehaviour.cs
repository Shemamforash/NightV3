using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MonoBehaviour
{
    public GameObject map_tile;
    private int map_size, tile_side_length;
    private float normalised_tile_side;
    private GameObject[,] tiles;

    // Use this for initialization
    void Start()
    {
        PersistenceLayer.LoadGameData();
        map_size = (int)PersistenceLayer.GetGameDataByName("map_size");
        float map_width_in_pixels = GetComponent<RectTransform>().rect.width;
		Debug.Log(800f / 14f);
        tile_side_length = (int)(map_width_in_pixels / map_size);
        normalised_tile_side = 1f / map_width_in_pixels * tile_side_length;
        tiles = new GameObject[map_size, map_size];
        float y_anchor = 0;
        for (int i = 0; i < map_size; ++i)
        {
            float y_anchor_max = y_anchor + normalised_tile_side;
            float x_anchor = 0;
            for (int j = 0; j < map_size; ++j)
            {
                float x_anchor_max = x_anchor + normalised_tile_side;
                GameObject new_tile = Instantiate(map_tile, transform.position, transform.rotation);
                new_tile.transform.SetParent(transform);
                RectTransform new_tile_rect = new_tile.GetComponent<RectTransform>();
				new_tile_rect.localScale = new Vector3(1f, 1f, 1f);
                new_tile_rect.anchorMin = new Vector2(x_anchor, y_anchor);
                new_tile_rect.anchorMax = new Vector2(x_anchor_max, y_anchor_max);
				new_tile_rect.offsetMin = new Vector2(0f, 0f);
				new_tile_rect.offsetMax = new Vector2(0f, 0f);		
                x_anchor += normalised_tile_side;
            }
            y_anchor += normalised_tile_side;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
