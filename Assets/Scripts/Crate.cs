using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject boom;
    public Color boomColor;

    AudioSource audio;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onHit() {
        GameObject ps = Instantiate(boom, transform.position, Quaternion.identity) as GameObject;
        var main = ps.GetComponent<ParticleSystem>().main;
        main.startColor = boomColor;
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
