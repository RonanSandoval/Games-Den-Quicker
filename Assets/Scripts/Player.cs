using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public MapGenerator mapGen;
    public int roomHeight;
    public int roomWidth;
    public TileMaker tileMaker;

    public int playerSpeed;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    public int[] currentRoom;

    public GameObject attack;

    public int maxHealth;
    public int health;
    public bool[] upgrades;

    public float speedCoeff;

    public bool invincible;
    float invincibleCooldown;
    float knockbackCooldown;
    float attackCooldown;

    public float timestopCooldown;
    public float timeStop;

    public GameObject boom;
    public Color boomColor;

    AudioSource audio;
    public AudioClip[] sounds;

    Animator anim;

    GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();

        roomHeight = tileMaker.roomHeight;
        roomWidth = tileMaker.roomWidth;
        currentRoom = mapGen.startingPoint;
        transform.position = new Vector3(currentRoom[1] * roomWidth + (roomWidth / 2), currentRoom[0] * roomHeight + (roomHeight / 2), 0); 

        health = maxHealth;
        upgrades = new bool[]{false, false, false};
        speedCoeff = 1f;

        knockbackCooldown = 0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        detectCurrentRoom();
        if (gc.currentState == GameController.GameState.Playing) {
            if (knockbackCooldown <= 0f) {
                move();
            }   
            
            detectAttack();
            detectTimeStop();

            if (invincibleCooldown > 0) {
                invincibleCooldown -= Time.deltaTime;
                if (invincibleCooldown <= 0) {
                    invincible = false;
                }
            }

            if (timestopCooldown > 0) {
                timestopCooldown -= Time.deltaTime;
                if (timestopCooldown <= 0) {
                    audio.clip = sounds[2];
                    audio.Play();
                }
            }

            if (timeStop > 0 ) {
                timeStop -= Time.deltaTime;
            }

            if (invincible) {
                sr.color = new Color(1f,1f,1f,.5f);
            } else {
                sr.color = new Color(1f,1f,1f,1f);
            }

            if (knockbackCooldown > 0) {
                knockbackCooldown -= Time.deltaTime;
            }

            if (attackCooldown > 0) {
                attackCooldown -= Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && gc.currentState == GameController.GameState.Info) {
            gc.currentState = GameController.GameState.Playing;
            audio.clip = sounds[3];
            audio.Play();
        }
        

    }

    void move() {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");
        if (horizontalInput < 0) {
            sr.flipX = true;
        } else if (horizontalInput > 0) {
            sr.flipX = false;
        }
        if (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0) {
            anim.SetBool("isWalking", true);
        } else {
            anim.SetBool("isWalking", false);
        }
        rb.velocity = new Vector2(horizontalInput * playerSpeed * speedCoeff, verticalInput * playerSpeed * speedCoeff);
    }

    void detectTimeStop() {
        if (upgrades[2] && timestopCooldown <= 0 && Input.GetKeyDown(KeyCode.Space)) {
            timestopCooldown = 15f;
            timeStop = 3f;
            audio.clip = sounds[1];
            audio.Play();
            // Vector3 dashDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            // dashDirection.z = 0.0f;
            // dashDirection.Normalize();
            // rb.AddForce(dashDirection * 45, ForceMode2D.Impulse);

        }
        
    }

    void detectCurrentRoom() {
        currentRoom[0] = (int)(transform.position.x / roomWidth);
        currentRoom[1] = (int)(transform.position.y / roomHeight);
    }

    void detectAttack() {
        if (Input.GetMouseButton(0) && attackCooldown <= 0) {
            GameObject attackObject = Instantiate(attack, transform.position, Quaternion.identity) as GameObject;
            attackObject.GetComponent<Attack>().sharpened = upgrades[0];
            attackCooldown = 0.4f;

        }
    }

    public void onHit(int damage, Vector3 hitDirection) {
        if (!invincible) {
            invincible = true;
            invincibleCooldown = 0.7f;
            
            health -= damage;
            
            audio.clip = sounds[0];
            audio.Play();

            GameObject ps = Instantiate(boom, transform.position, Quaternion.identity) as GameObject;
            var main = ps.GetComponent<ParticleSystem>().main;
            main.startColor = boomColor;

            if (health <= 0) {
                die();
            }

            
        }

        knockbackCooldown = 0.3f;
        Vector3 knockDirection = transform.position - hitDirection;
        Debug.Log(knockDirection);
        rb.AddForce(knockDirection * 20, ForceMode2D.Impulse);
        
    }

    void die() {
        GameObject.Find("Info").GetComponent<Info>().currentImage = 4;
        sr.enabled = false;
        gc.currentState = GameController.GameState.Dead;
    }
}
