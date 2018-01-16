// Author André Schoul

using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public Texture2D map;
    public ColorToPrefab[] colorMappings;

    private float tileLength = 2.52f;

    // Use this for initialization
    void Awake () {
        GenerateLevel();       
    }
        
    private void GenerateLevel() {
        for(int x = 0; x < map.width; x++) {
            for(int y = 0; y < map.height; y++) {
                GenerateTile(x, y);
            }
        }
    }
    /*
    private void GenerateTile(int x, int y) {
        if (map.GetPixel(x, y).a == 0) return;
        if (map.GetPixel(x - 1, y).a == 0 && map.GetPixel(x, y + 1).a != 0) Instantiate(colorMappings[5].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
        else if (map.GetPixel(x + 1, y).a == 0 && map.GetPixel(x, y + 1).a != 0) Instantiate(colorMappings[4].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
        else if (map.GetPixel(x, y + 1).a == 0 && map.GetPixel(x - 1, y).a != 0 && map.GetPixel(x + 1, y).a != 0) Instantiate(colorMappings[0].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
        else if (map.GetPixel(x, y + 1).a == 0 && map.GetPixel(x - 1, y).a == 0 && map.GetPixel(x + 1, y).a != 0) Instantiate(colorMappings[2].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
        else if (map.GetPixel(x, y + 1).a == 0 && map.GetPixel(x - 1, y).a != 0 && map.GetPixel(x + 1, y).a == 0) Instantiate(colorMappings[3].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
        else Instantiate(colorMappings[1].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);


        Debug.Log(map.GetPixel(x, y) == new Color(0, 0, 0, 1));


    }

    /*
    private void GenerateTile(int x, int y) {
        Color pixelColor = map.GetPixel(x, y);
        if (pixelColor.a == 0) return;
        foreach (ColorToPrefab colorMapping in colorMappings) {
            if (colorMapping.color.Equals(pixelColor)) {
                Vector2 position = new Vector2(x * tileLength, y * tileLength);
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
            }
        }
    }
    */




    private void GenerateTile(int x, int y) {
        /*if (map.GetPixel(x, y).a == 0) return;
        if (map.GetPixel(x, y) == ground) {
            if (map.GetPixel(x - 1, y).a == 0 && map.GetPixel(x, y + 1).a != 0) Instantiate(colorMappings[5].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
            else if (map.GetPixel(x + 1, y).a == 0 && map.GetPixel(x, y + 1).a != 0) Instantiate(colorMappings[4].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
            else if (map.GetPixel(x, y + 1).a == 0 && map.GetPixel(x - 1, y).a != 0 && map.GetPixel(x + 1, y).a != 0) Instantiate(colorMappings[0].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
            else if (map.GetPixel(x, y + 1).a == 0 && map.GetPixel(x - 1, y).a == 0 && map.GetPixel(x + 1, y).a != 0) Instantiate(colorMappings[2].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
            else if (map.GetPixel(x, y + 1).a == 0 && map.GetPixel(x - 1, y).a != 0 && map.GetPixel(x + 1, y).a == 0) Instantiate(colorMappings[3].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
            else Instantiate(colorMappings[1].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
        } else {    
            foreach (ColorToPrefab colorMapping in colorMappings) {
                if (colorMapping.color.Equals(map.GetPixel(x, y)) && map.GetPixel(x, y) != new Color(0, 0, 0, 1)) {
                    Vector2 position = new Vector2(x * tileLength, y * tileLength);
                    Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
                }
            }
        }*/

      
        if (map.GetPixel(x, y).a == 0) return;
        Color ground = new Color(0, 0, 0, 1);
        if (map.GetPixel(x, y) == ground) {
            if (map.GetPixel(x - 1, y) != ground && map.GetPixel(x, y + 1) == ground) Instantiate(colorMappings[5].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
            else if (map.GetPixel(x + 1, y) != ground && map.GetPixel(x, y + 1) == ground) Instantiate(colorMappings[4].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
            else if (map.GetPixel(x, y + 1) != ground && map.GetPixel(x - 1, y) == ground && map.GetPixel(x + 1, y) == ground) Instantiate(colorMappings[0].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
            else if (map.GetPixel(x, y + 1) != ground && map.GetPixel(x - 1, y) != ground && map.GetPixel(x + 1, y) == ground) Instantiate(colorMappings[2].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
            else if (map.GetPixel(x, y + 1) != ground && map.GetPixel(x - 1, y) == ground && map.GetPixel(x + 1, y) != ground) Instantiate(colorMappings[3].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
            else Instantiate(colorMappings[1].prefab, new Vector2(x * tileLength, y * tileLength), Quaternion.identity, transform);
        } else {    
            foreach (ColorToPrefab colorMapping in colorMappings) {
                if (colorMapping.color.Equals(map.GetPixel(x, y)) && map.GetPixel(x, y) != new Color(0, 0, 0, 1)) {
                    Vector2 position = new Vector2(x * tileLength, y * tileLength);


                    if (colorMapping == colorMappings[12]) Instantiate(colorMapping.prefab, new Vector2(position.x + 1.3f, position.y + 1.3f), Quaternion.identity, transform);
                    else if (colorMapping == colorMappings[13]) Instantiate(colorMapping.prefab, new Vector2(position.x, position.y + 0.5f), Quaternion.identity, transform);
                    else if (colorMapping == colorMappings[14]) Instantiate(colorMapping.prefab, new Vector2(position.x + 1.25f, position.y + 1f), Quaternion.identity, transform);
                    else Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
