using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public MapGenerator mapGen;
    private int roomHeight;
    private int roomWidth;

    public TileMaker tileMaker;

    public int playerSpeed;

    private Rigidbody2D rb;

    public int[] currentRoom;

    public GameObject attack;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        roomHeight = tileMaker.roomHeight;
        roomWidth = tileMaker.roomWidth;
        currentRoom = mapGen.startingPoint;
        transform.position = new Vector3(currentRoom[1] * roomWidth + (roomWidth / 2), currentRoom[0] * roomHeight + (roomHeight / 2), 0);   
    }

    // Update is called once per frame
    void Update()
    {
        move();   
        detectCurrentRoom();
        detectAttack();
    }

    void move() {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(horizontalInput * playerSpeed, verticalInput * playerSpeed);
    }

    void detectCurrentRoom() {
        currentRoom[0] = (int)(transform.position.x / roomWidth);
        currentRoom[1] = (int)(transform.position.y / roomHeight);
    }

    void detectAttack() {
        if (Input.GetMouseButtonDown(0)) {
            GameObject attackObject = Instantiate(attack, transform.position, Quaternion.identity) as GameObject;

        }
    }
}
