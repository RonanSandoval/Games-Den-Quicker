using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject heartDrop;
    public GameObject boom;
    public Color boomColor;

    AudioSource audio;

    Player player;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHit() {
        GameObject ps = Instantiate(boom, transform.position, Quaternion.identity) as GameObject;
        var main = ps.GetComponent<ParticleSystem>().main;
        main.startColor = boomColor;
        if (player.health < player.maxHealth && Random.Range(0, player.health + 1) < 1) {
            Instantiate(heartDrop, transform.position, Quaternion.identity);
        }
        StartCoroutine(explode());
        
    }

    IEnumerator explode() {
        audio.Play();
        Destroy(GetComponent<Collider>());
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(audio.clip.length);
        Destroy(gameObject);
    }
}
