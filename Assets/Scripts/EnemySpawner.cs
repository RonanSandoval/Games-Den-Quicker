using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public MapGenerator mapGen;
    private int roomHeight;
    private int roomWidth;
    public TileMaker tileMaker;
    public Player player;

    public GameObject enemyObject;

    public int[] spawnedRoom;

    // Start is called before the first frame update
    void Start()
    {
        spawnedRoom = new int[]{mapGen.startingPoint[0], mapGen.startingPoint[1]};
        roomHeight = tileMaker.roomHeight;
        roomWidth = tileMaker.roomWidth;
        spawnRoom(spawnedRoom);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedRoom[0] != player.currentRoom[0] || spawnedRoom[1] != player.currentRoom[1]) {
            removeEnemies();
            spawnRoom(player.currentRoom);
            spawnedRoom[0] = player.currentRoom[0];
            spawnedRoom[1] = player.currentRoom[1];
        }
    }

    void spawnRoom(int[] roomCoords) { 
        foreach (MapGenerator.Entity entity in mapGen.map[roomCoords[1], roomCoords[0]].enemies) {
            Vector3 enemyPosition = new Vector3(roomCoords[0] * roomWidth + entity.x, roomCoords[1] * roomHeight + entity.y, 0);
            GameObject newEnemy = Instantiate(enemyObject, enemyPosition, Quaternion.identity) as GameObject;
            newEnemy.GetComponent<Enemy>().ai = entity.type;
            newEnemy.transform.parent = gameObject.transform;

        }
       
    }

    void removeEnemies() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
