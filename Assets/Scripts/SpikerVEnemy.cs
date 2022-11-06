using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikerVEnemy : Enemy
{

    protected override void Start() {
        base.Start();
        knockable = false;
        damageable = false;

        if (player.transform.position.y > transform.position.y) {
            transform.position = new Vector3(transform.position.x, player.currentRoom[1] * player.roomHeight + player.roomHeight - 1.5f, 0);
        } else {
             transform.position = new Vector3(transform.position.x, player.currentRoom[1] * player.roomHeight + 1.5f, 0);
        }
    }

    protected override void Update() {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        if (Mathf.Abs(player.transform.position.y - transform.position.y) > 0.5f && player.timeStop <= 0) {
            if (player.transform.position.y - transform.position.y > 0) {
                rb.velocity = new Vector3(0, 20, 0);
            } else {
                rb.velocity = new Vector3(0, -20, 0);
            }
        }

        base.Update();
    }
}
