using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int maxMapWidth = 8;
    public int maxMapHeight = 8;

    public int[] startingPoint;
    public int[] upgradeOrder;

    public struct Entity {
        public float x;
        public float y;
        public int type;

        public Entity(float myX, float myY, int myType) {
            x = myX;
            y = myY;
            type = myType;
        }
    }

    public class Room {
        public int level;
        public bool[] doors;
        public int barricadeDirection;
        public int layout;

        public List<Entity> enemies;
        public List<Entity> things;

        public Room(int myLevel, bool[] myDoors) {
            level = myLevel;
            doors = myDoors;
            enemies = new List<Entity>();
            things = new List<Entity>();
            barricadeDirection = -1;
            layout = 0;
        }

        public Room() {
            level = 0;
            doors = new bool[4];
            enemies = new List<Entity>();
            things = new List<Entity>();
            barricadeDirection = -1;
            layout = 0;
        }
    } 

    public Room[,] map;

    void Awake()
    {
        upgradeOrder = new int[]{0,1,2};
        shuffleUpgrades();

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

        placeThings();
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
                        map[tryCoords[0], tryCoords[1]].barricadeDirection = 0;
                        tryCoords[0] -= 1;
                        break;
                    case 1:
                        map[tryCoords[0], tryCoords[1]].doors[1] = true;
                        map[tryCoords[0] + 1, tryCoords[1]].doors[0] = true;
                        map[tryCoords[0], tryCoords[1]].barricadeDirection = 1;
                        tryCoords[0] += 1;
                        break;
                    case 2:
                        map[tryCoords[0], tryCoords[1]].doors[2] = true;
                        map[tryCoords[0], tryCoords[1] - 1].doors[3] = true;
                        map[tryCoords[0], tryCoords[1]].barricadeDirection = 2;
                        tryCoords[1] -= 1;
                        break;
                    case 3:
                        map[tryCoords[0], tryCoords[1]].doors[3] = true;
                        map[tryCoords[0], tryCoords[1] + 1].doors[2] = true;
                        map[tryCoords[0], tryCoords[1]].barricadeDirection = 3;
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
                if (map[i,j].level % 3 == 2 || (map[i,j].level % 3 == 1 && map[i,j].layout >= 0 && map[i,j].level != 1)) {
                    
                    switch (map[i,j].layout) {
                        case 0:
                            if (Random.Range(0,2) == 0) {
                                map[i,j].enemies.Add(new Entity(7f,6f, 0));
                                map[i,j].enemies.Add(new Entity(7f,7f, 0));
                                map[i,j].enemies.Add(new Entity(7f,5f, 0));
                                map[i,j].enemies.Add(new Entity(2.5f,6f, 4));
                                map[i,j].enemies.Add(new Entity(11.5f,6f, 4));
                            } else {
                                map[i,j].enemies.Add(new Entity(7f,6f, 1));
                                map[i,j].enemies.Add(new Entity(3.5f,3.5f,0));
                                map[i,j].enemies.Add(new Entity(10.5f,8.5f, 0));
                                map[i,j].enemies.Add(new Entity(3.5f,8.5f,0));
                                map[i,j].enemies.Add(new Entity(10.5f,3.5f,0));
                            }
                            break;
                        case 1:
                            if (Random.Range(0,2) == 0) {
                                map[i,j].enemies.Add(new Entity(7f,2.5f, 5));
                                map[i,j].enemies.Add(new Entity(7f,9.5f, 5));
                                map[i,j].enemies.Add(new Entity(5.5f,6f, 0));
                                map[i,j].enemies.Add(new Entity(8.5f,6f, 0));
                            } else {
                                map[i,j].enemies.Add(new Entity(4.5f,4.5f, 4));
                                map[i,j].enemies.Add(new Entity(7f,6f, 0));
                                map[i,j].enemies.Add(new Entity(9.5f,7.5f, 1));
                            }
                            break;
                        case 2:
                            if (Random.Range(0,2) == 0) {
                                map[i,j].enemies.Add(new Entity(4.5f,6f, 1));
                                map[i,j].enemies.Add(new Entity(9.5f,6f, 1));
                            } else {
                                map[i,j].enemies.Add(new Entity(10.5f,3.5f,5));
                                map[i,j].enemies.Add(new Entity(3.5f,8.5f,5));
                                map[i,j].enemies.Add(new Entity(7f,6f, 0));
                            }
                            break;
                        case 3:
                            if (Random.Range(0,2) == 0) {
                                map[i,j].enemies.Add(new Entity(4f,2.5f,0));
                                map[i,j].enemies.Add(new Entity(4f,9.5f,0));
                                map[i,j].enemies.Add(new Entity(10f,2.5f,0));
                                map[i,j].enemies.Add(new Entity(10f,9.5f,0));
                            } else {
                                map[i,j].enemies.Add(new Entity(4f,2.5f,4));
                                map[i,j].enemies.Add(new Entity(10f,9.5f,4));
                            }
                            break;
                        case 4:
                            if (Random.Range(0,2) == 0) {
                                map[i,j].enemies.Add(new Entity(3.5f,3.5f,1));
                                map[i,j].enemies.Add(new Entity(10.5f,8.5f, 1));
                            } else {
                                map[i,j].enemies.Add(new Entity(4.5f,4.5f,0));
                                map[i,j].enemies.Add(new Entity(9.5f,7.5f, 0));
                                map[i,j].enemies.Add(new Entity(4.5f,7.5f,0));
                                map[i,j].enemies.Add(new Entity(9.5f,4.5f,0));
                            }
                            break;
                        default:
                            int variety = Random.Range(0,5);
                            if (variety == 0) {
                                map[i,j].enemies.Add(new Entity(3.5f,3.5f,0));
                                map[i,j].enemies.Add(new Entity(3.5f,8.5f,0));
                                map[i,j].enemies.Add(new Entity(10.5f,3.5f,0));
                                map[i,j].enemies.Add(new Entity(10.5f,8.5f,0));
                            } else if (variety == 1) {
                                map[i,j].enemies.Add(new Entity(7f,6f, 1));
                                map[i,j].enemies.Add(new Entity(3.5f,2.5f, 4));
                                map[i,j].enemies.Add(new Entity(10.5f,9.5f, 4));
                            } else if (variety == 2) {
                                map[i,j].enemies.Add(new Entity(9.5f,3.5f, 4));
                                map[i,j].enemies.Add(new Entity(4.5f,8.5f, 4));
                                map[i,j].enemies.Add(new Entity(4.5f,3.5f, 5));
                                map[i,j].enemies.Add(new Entity(9.5f,8.5f, 5));
                            } else if (variety == 3) {
                                map[i,j].enemies.Add(new Entity(9.5f,3.5f, 0));
                                map[i,j].enemies.Add(new Entity(4.5f,8.5f, 0));
                                map[i,j].enemies.Add(new Entity(5.5f,3.5f, 5));
                                map[i,j].enemies.Add(new Entity(8.5f,8.5f, 5));
                            } else {
                                map[i,j].enemies.Add(new Entity(3.5f,6.5f, 1));
                                map[i,j].enemies.Add(new Entity(10.5f,5.5f, 1));
                            }
                            break;
                    }
                }

                if (map[i,j].layout == -1) {
                    if (Random.Range(0,2) == 0) {
                         map[i,j].enemies.Add(new Entity(7f,6f, 1));
                    } else {
                         map[i,j].enemies.Add(new Entity(5.5f,6f, 0));
                          map[i,j].enemies.Add(new Entity(8.5f,6f, 0));
                    }
                }
            }
        }
    }

    public void placeThings() {
        for (int i = 0; i < maxMapHeight; i++) {
            for (int j = 0; j < maxMapWidth; j++) {
                // create barricades
                if (map[i,j].level != 1 && map[i,j].level % 3 == 1) {
                    
                    int barricadeDirection = map[i,j].barricadeDirection;
                    int barricadeType = upgradeOrder[(map[i,j].level / 3) - 1];

                    // if a dodger...
                    if (barricadeType == 2) {
                        map[i,j].layout = -1;
                        switch(barricadeDirection) {
                            case 0:
                                map[i,j].enemies.Add(new Entity(6.5f,3.5f, 3));
                                break;
                            case 1:
                                map[i,j].enemies.Add(new Entity(6.5f,8.5f, 3));
                                break;
                            case 2:
                                map[i,j].enemies.Add(new Entity(3.5f,5.5f, 2));
                                break;
                            case 3:
                                map[i,j].enemies.Add(new Entity(10.5f,5.5f, 2));
                                break;
                        }
                    } else {
                        switch(barricadeDirection) {
                            case 0:
                                map[i,j].things.Add(new Entity(6.5f,1.5f, barricadeType));
                                map[i,j].things.Add(new Entity(7.5f,1.5f, barricadeType));
                                break;
                            case 1:
                                map[i,j].things.Add(new Entity(6.5f,10.5f, barricadeType));
                                map[i,j].things.Add(new Entity(7.5f,10.5f, barricadeType));
                                break;
                            case 2:
                                map[i,j].things.Add(new Entity(1.5f,5.5f, barricadeType));
                                map[i,j].things.Add(new Entity(1.5f,6.5f, barricadeType));
                                break;
                            case 3:
                                map[i,j].things.Add(new Entity(12.5f,5.5f, barricadeType));
                                map[i,j].things.Add(new Entity(12.5f,6.5f, barricadeType));
                                break;
                        }
                    }
                }

                // Create random obstacles
                if ((map[i,j].level % 3 == 2 || map[i,j].level % 3 == 1) && map[i,j].level != 1 && map[i,j].layout != -1) {

                    int roomType = Random.Range(0,8);
                    map[i,j].layout = roomType;

                    switch(roomType) {
                        case 0:
                            map[i,j].things.Add(new Entity(4.5f,5.5f,0));
                            map[i,j].things.Add(new Entity(4.5f,6.5f,0));
                            map[i,j].things.Add(new Entity(9.5f,5.5f,0));
                            map[i,j].things.Add(new Entity(9.5f,6.5f,0));
                            break;
                        case 1:
                            map[i,j].things.Add(new Entity(2.5f,2.5f,0));
                            map[i,j].things.Add(new Entity(2.5f,9.5f,0));
                            map[i,j].things.Add(new Entity(11.5f,2.5f,0));
                            map[i,j].things.Add(new Entity(11.5f,9.5f,0));
                            break;
                        case 2:
                            for (int k = 4; k <= 9; k++) {
                                map[i,j].things.Add(new Entity(k + 0.5f,3.5f,1));
                                map[i,j].things.Add(new Entity(k + 0.5f,8.5f,1));
                            }
                            break;
                        case 3:
                            map[i,j].things.Add(new Entity(3.5f,4.5f,1));
                            map[i,j].things.Add(new Entity(4.5f,4.5f,1));
                            map[i,j].things.Add(new Entity(3.5f,7.5f,1));
                            map[i,j].things.Add(new Entity(4.5f,7.5f,1));
                            map[i,j].things.Add(new Entity(10.5f,4.5f,1));
                            map[i,j].things.Add(new Entity(9.5f,4.5f,1));
                            map[i,j].things.Add(new Entity(10.5f,7.5f,1));
                            map[i,j].things.Add(new Entity(9.5f,7.5f,1));
                            break;
                        case 4:
                            map[i,j].things.Add(new Entity(6.5f,5.5f,0));
                            map[i,j].things.Add(new Entity(6.5f,6.5f,0));
                            map[i,j].things.Add(new Entity(7.5f,5.5f,0));
                            map[i,j].things.Add(new Entity(7.5f,6.5f,0));
                            map[i,j].things.Add(new Entity(3.5f,5.5f,1));
                            map[i,j].things.Add(new Entity(3.5f,6.5f,1));
                            map[i,j].things.Add(new Entity(10.5f,5.5f,1));
                            map[i,j].things.Add(new Entity(10.5f,6.5f,1));
                            break;
                            
                    }

                }
            }
        }
    }


    public void Print2DArray()
    {
        for (int i = map.GetLength(0) - 1; i >= 0; i--)
        {
            string line = "";
            for (int j = 0; j < map.GetLength(1); j++)
            {
                //line += " " + map[i,j].doors[0] + "" + map[i,j].doors[1]+ "" + map[i,j].doors[2] + "" + map[i,j].doors[3];
                line += " " + map[i,j].level + map[i,j].layout;
            }
            Debug.Log(line);
        }
    }

    // https://answers.unity.com/questions/1189736/im-trying-to-shuffle-an-arrays-order.html
    public void shuffleUpgrades() {
         for (int i = 0; i < upgradeOrder.Length; i++) {
             int rnd = Random.Range(0, upgradeOrder.Length);
             int tempInt = upgradeOrder[rnd];
             upgradeOrder[rnd] = upgradeOrder[i];
             upgradeOrder[i] = tempInt;
         }
     }
}