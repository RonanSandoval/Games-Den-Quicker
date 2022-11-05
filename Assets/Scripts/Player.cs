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

    public int maxHealth;
    public int health;
    public bool[] upgrades;

    public bool invincible;
    float invincibleCooldown;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        roomHeight = tileMaker.roomHeight;
        roomWidth = tileMaker.roomWidth;
        currentRoom = mapGen.startingPoint;
        transform.position = new Vector3(currentRoom[1] * roomWidth + (roomWidth / 2), currentRoom[0] * roomHeight + (roomHeight / 2), 0); 

        health = maxHealth;
        upgrades = new bool[]{false, false, false};
    }

    // Update is called once per frame
    void Update()
    {
        if (!invincible) {
            move();
        }   
        detectCurrentRoom();
        detectAttack();

        if (invincibleCooldown > 0) {
            invincibleCooldown -= Time.deltaTime;
            if (invincibleCooldown <= 0) {
                invincible = false;
            }
        }
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
            attackObject.GetComponent<Attack>().sharpened = upgrades[0];

        }
    }

    public void onHit(int damage, Vector3 hitDirection) {
        invincible = true;
        invincibleCooldown = 0.3f;
        health -= damage;
        Debug.Log(health);

        if (health <= 0) {
            die();
        }

        Vector3 knockDirection = transform.position - hitDirection;
        Debug.Log(knockDirection);
        rb.AddForce(knockDirection * 20, ForceMode2D.Impulse);
    }

    void die() {
        Destroy(gameObject);
    }
}
