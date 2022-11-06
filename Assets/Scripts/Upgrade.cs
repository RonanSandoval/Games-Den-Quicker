using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public int upgradeType;

    private SpriteRenderer sr;
    public Sprite[] sprites;
    
    private float rotator;

    public AudioClip[] sounds;

    GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();

        sr.sprite = sprites[upgradeType];
    }

    // Update is called once per frame
    void Update()
    {
        if (upgradeType != 3) {
            rotator += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0,0, Mathf.Cos(rotator * 2) * 5f);
        }
        
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("Info").GetComponent<Info>().currentImage = upgradeType;
            if (upgradeType == 3) {
                GetComponent<AudioSource>().clip = sounds[1];
                GetComponent<AudioSource>().Play();
                gc.currentState = GameController.GameState.Win;
            } else {
                Player player = collision.gameObject.GetComponent<Player>();
                player.upgrades[upgradeType] = true;
                GameObject.Find("GameController").GetComponent<GameController>().currentState = GameController.GameState.Info;
                StartCoroutine(disappear());
            }
        }
    }

    IEnumerator disappear() {
        GetComponent<AudioSource>().clip = sounds[0];
        GetComponent<AudioSource>().Play();
        Destroy(GetComponent<Collider>());
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        Destroy(gameObject);
    }
}
