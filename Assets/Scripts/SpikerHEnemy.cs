using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikerHEnemy : Enemy
{
    protected override void Start() {
        base.Start();
        knockable = false;
        damageable = false;

        if (player.transform.position.x > transform.position.x) {
            transform.position = new Vector3(player.currentRoom[0] * player.roomWidth + player.roomWidth - 1.5f, transform.position.y, 0);
        } else {
             transform.position = new Vector3(player.currentRoom[0] * player.roomWidth + 1.5f, transform.position.y, 0);
        }
    }

    protected override void Update() {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        if (Mathf.Abs(player.transform.position.x - transform.position.x) > 0.2f) {
            if (player.transform.position.x - transform.position.x > 0) {
                rb.velocity = new Vector3(Time.deltaTime * 2000, 0, 0);
            } else {
                rb.velocity = new Vector3(Time.deltaTime * -2000, 0, 0);
            }
        }

        base.Update();
    }
}
