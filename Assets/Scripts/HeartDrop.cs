using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDrop : MonoBehaviour
{
    float lifespan;

    // Start is called before the first frame update
    void Start()
    {
        lifespan = 8.0f;
    }

    // Update is called once per frame
    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player.health < player.maxHealth) {
                player.health += 1;
                
                StartCoroutine(heal());
            } else {
                Destroy (gameObject);
            }
        }
    }

    IEnumerator heal() { 
        GetComponent<AudioSource>().Play();
        Destroy(GetComponent<Collider>());
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        Destroy (gameObject);
    }
}
