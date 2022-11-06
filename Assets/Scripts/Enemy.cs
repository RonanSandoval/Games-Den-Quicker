using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool knockable;
    public bool damageable;
    public int maxHealth;
    public int health;
    protected bool invincible;
    protected float invincibleCooldown;

    protected Player player;

    public GameObject heartDrop;
    

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected AudioSource audio;

    public AudioClip[] sounds;
    public GameObject boom;

    public Color boomColor;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (invincibleCooldown > 0) {
            invincibleCooldown -= Time.deltaTime;
            if (invincibleCooldown <= 0) {
                invincible = false;
            }
        }
        
        Color myColor = new Color(1f,1f,1f,1f);
        if (invincible) {
            myColor.a = 0.5f;
        }
        if (player.timeStop > 0) {
            myColor.b = 0f;
        }

        sr.color = myColor;

        
    }

    public void onHit(int damage, Vector3 hitDirection) {
        if (!invincible && damageable) {
            invincible = true;
            invincibleCooldown = 0.3f;

            health -= damage;
            Debug.Log(health);

            if (health <= 0) {
                audio.clip = sounds[1];
                StartCoroutine(die());
            } else {
                audio.clip = sounds[0];
                audio.Play();
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

    IEnumerator die() {
        GameObject ps = Instantiate(boom, transform.position, Quaternion.identity) as GameObject;
        var main = ps.GetComponent<ParticleSystem>().main;
        main.startColor = boomColor;
        if (player.health < player.maxHealth && Random.Range(0, player.health + 1) < 1) {
            Instantiate(heartDrop, transform.position, Quaternion.identity);
        }
        Destroy(GetComponent<Collider>());
        GetComponent<SpriteRenderer>().enabled = false;
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy") {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().onHit(1, transform.position);
        }
    }
}
