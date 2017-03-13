using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBehaviour : MonoBehaviour
{
    public GameObject map_tile;
    private static int map_size, tile_side_length;
    private float normalised_tile_side, desired_map_width, actual_map_width;
    private Tile[,] tiles;
    private float map_left, map_right, map_top, map_bottom;
    private int vision = 4;
    private float last_screen_width, last_screen_height;

    private class Tile
    {
        public enum Resource { water, fuel, food, scrap };
        public Resource resource_here;
        public float resource_quantity;
        public enum State { explored, exploring, unexplored };
        private float danger;
        private Color default_color;
        private GameObject tile_object;
        public Tile(GameObject tile_object, int x, int y)
        {
            this.tile_object = tile_object;
            if (x < map_size / 2)
            {
                x -= map_size;
                x = Mathf.Abs(x) - 1;
            }
            if (y < map_size / 2)
            {
                y -= map_size;
                y = Mathf.Abs(y) - 1;
            }
            x -= map_size / 2;
            y -= map_size / 2;
            Debug.Log(x + "  " + y);
            if (x >= y)
            {
                danger = 2f / map_size * x;
            }
            else
            {
                danger = 2f / map_size * y;
            }
			danger = 1 - danger;
            default_color = new Color(1, 1, 1, danger / 2f + 0.5f);
			ResetColor();
        }
        public void ResetColor()
        {
            tile_object.GetComponent<Image>().color = default_color;
        }

        public GameObject GetTileObject()
        {
            return tile_object;
        }
    }

    void Start()
    {
        PersistenceLayer.LoadGameData();
        map_size = (int)PersistenceLayer.GetGameDataByName("map_size");
        desired_map_width = GetComponent<RectTransform>().rect.width;
        actual_map_width = Vector2.Scale(transform.GetComponent<RectTransform>().rect.size, transform.lossyScale).x;
        last_screen_height = Screen.height;
        last_screen_width = Screen.width;
        GenerateSquare();
        RecalculateMapBounds();
    }
    private Tile centre_tile = null;

    private void GenerateSquare()
    {
        tile_side_length = (int)(desired_map_width / map_size);
        normalised_tile_side = 1f / desired_map_width * tile_side_length;
        tiles = new Tile[map_size, map_size];
        float y_anchor = 0;
        int centre_tiles = map_size / 2 - 1;
        bool add_centre_tile = false;
        for (int i = 0; i < map_size; ++i)
        {
            float y_anchor_max = y_anchor + normalised_tile_side;
            float x_anchor = 0;
            for (int j = 0; j < map_size; ++j)
            {
                float x_anchor_max = x_anchor + normalised_tile_side;
                bool add_tile = true;
                if (i == centre_tiles && j == centre_tiles)
                {
                    y_anchor_max += normalised_tile_side;
                    x_anchor_max += normalised_tile_side;
                    add_centre_tile = true;
                }
                else if ((i == centre_tiles && j == centre_tiles + 1) ||
                          (i == centre_tiles + 1 && j == centre_tiles) ||
                          (i == centre_tiles + 1 && j == centre_tiles + 1))
                {
                    add_tile = false;
                    tiles[i, j] = centre_tile;
                }
                if (add_tile)
                {
                    GameObject new_tile_object = Instantiate(map_tile, transform.position, transform.rotation);
                    Tile new_tile = new Tile(new_tile_object, i, j);
                    new_tile_object.transform.SetParent(transform);
                    RectTransform new_tile_rect = new_tile_object.GetComponent<RectTransform>();
                    new_tile_rect.localScale = new Vector3(1f, 1f, 1f);
                    new_tile_rect.anchorMin = new Vector2(x_anchor, y_anchor);
                    new_tile_rect.anchorMax = new Vector2(x_anchor_max, y_anchor_max);
                    new_tile_rect.offsetMin = new Vector2(0f, 0f);
                    new_tile_rect.offsetMax = new Vector2(0f, 0f);
                    if (add_centre_tile)
                    {
                        centre_tile = new_tile;
                        add_centre_tile = false;
                    }
                    tiles[i, j] = new_tile;
                }
                x_anchor += normalised_tile_side;
                y_anchor_max = y_anchor + normalised_tile_side;
            }
            y_anchor += normalised_tile_side;
        }
    }

    private List<Tile> selected_tiles = new List<Tile>();

    private void GetButtonOverlap()
    {
        Vector2 mouse_screen_position = Input.mousePosition;
        List<Vector2> potential_tiles_to_highlight = new List<Vector2>();

        foreach (Tile i in selected_tiles)
        {
            i.ResetColor();
        }

        selected_tiles.Clear();

        //If the mouse is within these bounds
        if (mouse_screen_position.x >= map_left && mouse_screen_position.x <= map_right
            && mouse_screen_position.y >= map_top && mouse_screen_position.y <= map_bottom)
        {
            //Scale the left and top bounds so the coordinates run from (0,0) to (map_width, map_width), instead of actual space
            float scaled_map_left = mouse_screen_position.x - map_left;
            float scaled_map_top = actual_map_width - (mouse_screen_position.y - map_top);

            //Calculate the width of the tiles in actual screen units
            float scaled_tile_width = actual_map_width / map_size;

            //Get the x and y value of this tile
            float scaled_x = scaled_map_left / scaled_tile_width;
            int x_index = (int)Mathf.Floor(scaled_x);
            float scaled_y = map_size - (scaled_map_top / scaled_tile_width);
            int y_index = (int)Mathf.Floor(scaled_y);
            int extra_x, extra_y;

            if (vision % 2 == 0)
            {
                if (scaled_x - x_index > 0.5f)
                {
                    extra_x = x_index + 1;
                }
                else
                {
                    extra_x = x_index;
                    x_index--;
                }

                if (scaled_y - y_index > 0.5f)
                {
                    extra_y = y_index + 1;
                }
                else
                {
                    extra_y = y_index;
                    y_index--;
                }

                if (vision == 4)
                {
                    x_index--;
                    extra_x++;
                    y_index--;
                    extra_y++;
                }
                // if(extra_x < x_index){
                // 	int temp = extra_x;
                // 	extra_x = x_index;
                // 	x_index = temp;
                // }
                // if(extra_y < x_index){
                // 	int temp = extra_x;
                // 	extra_y = x_index;
                // 	x_index = temp;
                // }
                for (int i = x_index; i <= extra_x; ++i)
                {
                    for (int j = y_index; j <= extra_y; ++j)
                    {
                        potential_tiles_to_highlight.Add(new Vector2(i, j));
                    }
                }

                potential_tiles_to_highlight.Add(new Vector2(x_index, y_index));
                potential_tiles_to_highlight.Add(new Vector2(x_index, extra_y));
                potential_tiles_to_highlight.Add(new Vector2(extra_x, y_index));
                potential_tiles_to_highlight.Add(new Vector2(extra_x, extra_y));

            }

            HighlightTiles(potential_tiles_to_highlight);

            float nearest_tile_distance = vision / 2;
        }
    }

    private void HighlightTiles(List<Vector2> tile_coordinates)
    {
        foreach (Vector2 coordinate in tile_coordinates)
        {
            if (coordinate.x < map_size && coordinate.y < map_size && coordinate.x >= 0 && coordinate.y >= 0)
            {
                Tile t = tiles[(int)coordinate.y, (int)coordinate.x];
                Image i = t.GetTileObject().GetComponent<Image>();
                if (i.gameObject != centre_tile.GetTileObject())
                {
                    i.color = Color.red;
                    selected_tiles.Add(t);
                }
            }
        }
    }

    private void RecalculateMapBounds()
    {
        //Get the map corners as actual screen coordinates, not scaled values
        //Map width must be actual map width not scaled value for this to work
        map_left = (Screen.width - actual_map_width) / 2;
        map_right = (Screen.width + actual_map_width) / 2;
        map_top = (Screen.height - actual_map_width) / 2;
        map_bottom = (Screen.height + actual_map_width) / 2;
    }

    public void UpdateScreenSides()
    {
        float screen_width = Screen.width;
        float screen_height = Screen.height;
        if (screen_height != last_screen_height || screen_width != last_screen_width)
        {
            RecalculateMapBounds();
        }
        last_screen_width = screen_width;
        last_screen_height = screen_height;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScreenSides();
        GetButtonOverlap();
    }

}
