using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (!player.upgrades[1]) {
                player.onHit(1, transform.position);
            } else {
                player.speedCoeff = 0.35f;
            }
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
                Player player = collision.gameObject.GetComponent<Player>();
                player.speedCoeff = 1f;
        }
    }
}
