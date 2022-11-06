using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public int upgradeType;

    private SpriteRenderer sr;
    public Sprite[] sprites;
    
    private float rotator;

    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();

        sr.sprite = sprites[upgradeType];
    }

    // Update is called once per frame
    void Update()
    {
        rotator += Time.deltaTime;
        transform.Rotate(0,0, Mathf.Cos(rotator * 2) * 0.2f);
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.upgrades[upgradeType] = true;
            disappear();
        }
    }

    void disappear() {
        Destroy(gameObject);
    }
}
