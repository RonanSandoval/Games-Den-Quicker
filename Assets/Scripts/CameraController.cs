using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int currentX;
    public int currentY;

    private int roomHeight;
    private int roomWidth;

    public TileMaker tileMaker;
    public MapGenerator mapGen;

    // Start is called before the first frame update
    void Start()
    {
        roomHeight = tileMaker.roomHeight;
        roomWidth = tileMaker.roomWidth;
        currentX = mapGen.startingPoint[1];
        currentY = mapGen.startingPoint[0];
        transform.position = new Vector3(currentX * roomWidth + (roomWidth / 2), currentY * roomHeight + (roomHeight / 2), -10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(currentX * roomWidth + (roomWidth / 2), currentY * roomHeight + (roomHeight / 2), -10), Time.deltaTime * 8);
    }
}
