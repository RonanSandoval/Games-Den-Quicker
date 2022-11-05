using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int ai;
    public bool knockable;
    public int maxHealth;
    public int health;
    private bool invincible;
    float invincibleCooldown;

    private Player player;
    

    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibleCooldown > 0) {
            invincibleCooldown -= Time.deltaTime;
            if (invincibleCooldown <= 0) {
                invincible = false;
            }
        }
        
        switch (ai) {
            case 1:
                if (!invincible) {
                    Vector3 moveDirection =  player.transform.position - transform.position;
                    moveDirection.Normalize();
                    rb.velocity = moveDirection * Time.deltaTime * 500;
                }
                break;
            case 2:
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                if (Mathf.Abs(player.transform.position.y - transform.position.y) > 0.2f) {
                    if (player.transform.position.y - transform.position.y > 0) {
                        rb.velocity = new Vector3(0, Time.deltaTime * 2000, 0);
                    } else {
                        rb.velocity = new Vector3(0, Time.deltaTime * -2000, 0);
                    }
                }
                break;
            case 3:
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                if (Mathf.Abs(player.transform.position.x - transform.position.x) > 0.2f) {
                    if (player.transform.position.x - transform.position.x > 0) {
                        rb.velocity = new Vector3(Time.deltaTime * 2000, 0, 0);
                    } else {
                        rb.velocity = new Vector3(Time.deltaTime * -2000, 0, 0);
                    }
                }
                break;
        }
    }

    public void onHit(int damage, Vector3 hitDirection) {
        if (!invincible && ai != 2 && ai != 3) {
            invincible = true;
            invincibleCooldown = 0.3f;

            health -= damage;
            Debug.Log(health);

            if (health <= 0) {
                die();
            }
            if (knockable) {
                knockback(hitDirection);
            }
        }
        
    }

    public void knockback(Vector3 hitDirection) {
        Vector3 knockDirection = transform.position - hitDirection;
        Debug.Log(knockDirection);
        rb.AddForce(knockDirection * 20, ForceMode2D.Impulse);
    }

    void die() {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().onHit(1, transform.position);
        }
    }
}
