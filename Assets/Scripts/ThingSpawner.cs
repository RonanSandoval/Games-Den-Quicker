using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingSpawner : MonoBehaviour
{
    public MapGenerator mapGen;
    private int roomHeight;
    private int roomWidth;
    public TileMaker tileMaker;

    public GameObject[] thingTypes;

    // Start is called before the first frame update
    void Start()
    {
        roomHeight = tileMaker.roomHeight;
        roomWidth = tileMaker.roomWidth;
        spawnThings();
    }

    void spawnThings() {
        MapGenerator.Room[,] map = mapGen.map;

        for (int i = 0; i < mapGen.maxMapHeight; i++) {
            for (int j = 0; j < mapGen.maxMapWidth; j++) {
                foreach (MapGenerator.Entity entity in map[i, j].things) {
                    Vector3 thingPosition = new Vector3(j * roomWidth + entity.x, i * roomHeight + entity.y, 0);
                    GameObject newThing = Instantiate(thingTypes[entity.type], thingPosition, Quaternion.identity) as GameObject;
                    newThing.transform.parent = gameObject.transform;
                }

                if (map[i,j].level != 0 && map[i,j].level % 3 == 0) {
                    Vector3 thingPosition = new Vector3(j * roomWidth + (roomWidth/2), i * roomHeight + (roomHeight/2), 0);
                    GameObject newThing = Instantiate(thingTypes[2], thingPosition, Quaternion.identity) as GameObject;
                    if (map[i,j].level == 12) {
                        newThing.GetComponent<Upgrade>().upgradeType = 3;
                    } else {
                        newThing.GetComponent<Upgrade>().upgradeType = mapGen.upgradeOrder[(map[i,j].level / 3) - 1];
                    }
                    
                    newThing.transform.parent = gameObject.transform;
                }
            }
        }
    }
}
