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
         if (!invincible) {
            Vector3 moveDirection =  player.transform.position - transform.position;
            moveDirection.Normalize();
            rb.velocity = moveDirection * Time.deltaTime * 500;
        }

        base.Update();
    }
}
