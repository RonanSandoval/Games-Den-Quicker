using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMaker : MonoBehaviour
{
    public Tilemap groundMap;
    public Tilemap wallMap;

    public Tile floor;
    public Tile wall;

    public int roomWidth;
    public int roomHeight;

    public MapGenerator mapGen; 

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < mapGen.maxMapHeight; i++) {
            for (int j = 0; j < mapGen.maxMapWidth; j++) {
                if (mapGen.map[i,j].level != 0) {
                    drawMap(j,i);
                }
            }
        }
    }

    void drawMap(int offsetX, int offsetY) {
        int scaledOffX = offsetX * roomWidth;
        int scaledOffY = offsetY * roomHeight;

        for (int x = scaledOffX; x < roomWidth + scaledOffX; x++) {
            for (int y = scaledOffY; y < roomHeight + scaledOffY; y++) {
                groundMap.SetTile(new Vector3Int(x,y,0), floor);
            }
        } 

        for (int x = 0 + scaledOffX; x < roomWidth + scaledOffX; x++) {
            wallMap.SetTile(new Vector3Int(x, scaledOffY, 0), wall);
            wallMap.SetTile(new Vector3Int(x, roomHeight - 1 + scaledOffY, 0), wall);
        }
        for (int y = 1 + scaledOffY; y < roomHeight - 1 + scaledOffY; y++) {
            wallMap.SetTile(new Vector3Int(scaledOffX, y, 0), wall);
            wallMap.SetTile(new Vector3Int(roomWidth - 1 + scaledOffX, y, 0), wall);
        }

        // create doorways
        if (mapGen.map[offsetY, offsetX].doors[1]) {
            wallMap.SetTile(new Vector3Int(scaledOffX + (roomWidth / 2), scaledOffY + roomHeight - 1, 0), null);
            wallMap.SetTile(new Vector3Int(scaledOffX + (roomWidth / 2) - 1, scaledOffY + roomHeight - 1, 0), null);
        }
        if (mapGen.map[offsetY, offsetX].doors[0]) {
            wallMap.SetTile(new Vector3Int(scaledOffX + (roomWidth / 2), scaledOffY, 0), null);
            wallMap.SetTile(new Vector3Int(scaledOffX + (roomWidth / 2) - 1, scaledOffY, 0), null);
        }
        if (mapGen.map[offsetY, offsetX].doors[2]) {
            wallMap.SetTile(new Vector3Int(scaledOffX, scaledOffY + (roomHeight / 2), 0), null);
            wallMap.SetTile(new Vector3Int(scaledOffX, scaledOffY + (roomHeight / 2) - 1, 0), null);
        }
        if (mapGen.map[offsetY, offsetX].doors[3]) {
            wallMap.SetTile(new Vector3Int(scaledOffX + roomWidth - 1, scaledOffY + (roomHeight / 2), 0), null);
            wallMap.SetTile(new Vector3Int(scaledOffX + roomWidth - 1, scaledOffY + (roomHeight / 2) - 1, 0), null);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
