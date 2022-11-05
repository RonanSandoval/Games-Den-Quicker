using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int maxMapWidth = 8;
    public int maxMapHeight = 8;

    public int[] startingPoint;

    public struct Entity {
        public int x;
        public int y;
        public int type;

        public Entity(int myX, int myY, int myType) {
            x = myX;
            y = myY;
            type = myType;
        }
    }

    public class Room {
        public int level;
        public bool[] doors;

        public List<Entity> enemies;
        public List<Entity> things;

        public Room(int myLevel, bool[] myDoors) {
            level = myLevel;
            doors = myDoors;
            enemies = new List<Entity>();
            things = new List<Entity>();
        }

        public Room() {
            level = 0;
            doors = new bool[4];
            enemies = new List<Entity>();
            things = new List<Entity>();
        }
    } 

    public Room[,] map;

    void Awake()
    {
        generateMap();
        Print2DArray();

    }

    void generateMap() {
        map = new Room[maxMapHeight, maxMapWidth];
        for (int i = 0; i < maxMapHeight; i++) {
            for (int j = 0; j < maxMapWidth; j++) {
                map[i,j] = new Room();
            }
        }

        int[] keyCoords = createInitial();
        createPath(2, Random.Range(2,5), keyCoords);
        keyCoords = createBranchPoint(4);
        createPath(5, Random.Range(2,5), keyCoords);
        keyCoords = createBranchPoint(7);
        createPath(8, Random.Range(2,5), keyCoords);
        keyCoords = createBranchPoint(10);
        createPath(11, Random.Range(2,5), keyCoords);

        placeEnemies();

    }

    int randomDirection(int[] pos) {
            List<int> validDirections = new List<int>();
            if (pos[0] - 1 >= 0 &&  map[pos[0] - 1, pos[1]].level == 0) {
                validDirections.Add(0);
            }
            if (pos[0] + 1 < maxMapHeight && map[pos[0] + 1, pos[1]].level == 0) {
                validDirections.Add(1);
            }
            if (pos[1] - 1 >= 0 &&  map[pos[0], pos[1] - 1].level == 0) {
                validDirections.Add(2);
            }
            if (pos[1] + 1 < maxMapWidth && map[pos[0] , pos[1] + 1].level == 0) {
                validDirections.Add(3);
            }
            
            // Stop if there are no more valid paths
            if (validDirections.Count == 0) {
                return -1;
            }

            return validDirections[Random.Range(0, validDirections.Count)];
    }

    int[] createInitial() {
        int[] start = new int[]{Random.Range(0, maxMapHeight), Random.Range(0, maxMapWidth)};
        startingPoint = new int[]{start[0], start[1]};
        map[start[0], start[1]].level = 1;
        
        int direction = randomDirection(start);

        switch (direction) {
            case 0:
                map[start[0], start[1]].doors[0] = true;
                map[start[0] - 1, start[1]].doors[1] = true;
                start[0] -= 1;
                break;
            case 1:
                map[start[0], start[1]].doors[1] = true;
                map[start[0] + 1, start[1]].doors[0] = true;
                start[0] += 1;
                break;
            case 2:
                map[start[0], start[1]].doors[2] = true;
                map[start[0], start[1] - 1].doors[3] = true;
                start[1] -= 1;
                break;
            case 3:
                map[start[0], start[1]].doors[3] = true;
                map[start[0], start[1] + 1].doors[2] = true;
                start[1] += 1;
                break;
        }
        return start;

    }

    void createPath(int level, int length, int[] startPos) {
        
        int[] currentPos = startPos;
        map[currentPos[0], currentPos[1]].level = level;

        

        for (int i = 0; i < length; i++) {
            int direction = randomDirection(currentPos);
            if (direction == -1) {
                map[currentPos[0], currentPos[1]].level = level + 1;
                return;
            }
            
            switch (direction) {
                case 0:
                    map[currentPos[0] - 1, currentPos[1]].level = level;
                    map[currentPos[0], currentPos[1]].doors[0] = true;
                    map[currentPos[0] - 1, currentPos[1]].doors[1] = true;
                    currentPos[0] -= 1;
                    break;
                case 1:
                    map[currentPos[0] + 1, currentPos[1]].level = level;
                    map[currentPos[0], currentPos[1]].doors[1] = true;
                    map[currentPos[0] + 1, currentPos[1]].doors[0] = true;
                    currentPos[0] += 1;
                    break;
                case 2:
                    map[currentPos[0], currentPos[1] - 1].level = level;
                    map[currentPos[0], currentPos[1]].doors[2] = true;
                    map[currentPos[0], currentPos[1] - 1].doors[3] = true;
                    currentPos[1] -= 1;
                    break;
                case 3:
                    map[currentPos[0], currentPos[1] + 1].level = level;
                    map[currentPos[0], currentPos[1]].doors[3] = true;
                    map[currentPos[0], currentPos[1] + 1].doors[2] = true;
                    currentPos[1] += 1;
                    break;
            }
        }
        map[currentPos[0], currentPos[1]].level = level + 1;
    }

    int[] createBranchPoint(int level) {
        while (true) {
            int[] tryCoords = new int[]{Random.Range(0, maxMapHeight), Random.Range(0, maxMapWidth)};
            int direction = randomDirection(tryCoords);
            if (map[tryCoords[0], tryCoords[1]].level % 3 == 2 && direction != -1) {
                map[tryCoords[0], tryCoords[1]].level = level;
                switch (direction) {
                    case 0:
                        map[tryCoords[0], tryCoords[1]].doors[0] = true;
                        map[tryCoords[0] - 1, tryCoords[1]].doors[1] = true;
                        tryCoords[0] -= 1;
                        break;
                    case 1:
                        map[tryCoords[0], tryCoords[1]].doors[1] = true;
                        map[tryCoords[0] + 1, tryCoords[1]].doors[0] = true;
                        tryCoords[0] += 1;
                        break;
                    case 2:
                        map[tryCoords[0], tryCoords[1]].doors[2] = true;
                        map[tryCoords[0], tryCoords[1] - 1].doors[3] = true;
                        tryCoords[1] -= 1;
                        break;
                    case 3:
                        map[tryCoords[0], tryCoords[1]].doors[3] = true;
                        map[tryCoords[0], tryCoords[1] + 1].doors[2] = true;
                        tryCoords[1] += 1;
                        break;
                }
                return tryCoords;
            }
        }
    }

    public void placeEnemies() {
        for (int i = 0; i < maxMapHeight; i++) {
            for (int j = 0; j < maxMapWidth; j++) {
                if (map[i,j].level % 3 == 2) {
                    map[i,j].enemies.Add(new Entity(3,3,0));
                }
            }
        }
    }

    public void Print2DArray()
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            string line = "";
            for (int j = 0; j < map.GetLength(1); j++)
            {
                //line += " " + map[i,j].doors[0] + "" + map[i,j].doors[1]+ "" + map[i,j].doors[2] + "" + map[i,j].doors[3];
                line += " " + map[i,j].level;
            }
            Debug.Log(line);
        }
    }
}