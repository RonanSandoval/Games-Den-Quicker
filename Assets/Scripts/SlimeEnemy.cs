using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    protected override void Start() {
        base.Start();
        knockable = true;
        damageable = true;
    }

    protected override void Update() {
         if (!invincible && player.timeStop <= 0) {
            Vector3 moveDirection =  player.transform.position - transform.position;
            moveDirection.Normalize();
            rb.velocity = moveDirection * 2;

            if (moveDirection.x > 0) {
                sr.flipX = true;
            } else {
                sr.flipX = false;
            }
        }

        base.Update();
    }
}
