using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullEnemy : Enemy
{
    Vector3 moveDirection; 
    public int moveMode;

    protected override void Start() {
        base.Start();
        knockable = false;
        damageable = true;
        if (moveMode == 0) {
            moveDirection =  new Vector3(0,1,0);
        } else {
            moveDirection =  new Vector3(1,0,0);
        }
        
    }

    protected override void Update() {
         if (player.timeStop <= 0) {
            rb.velocity = moveDirection * 6;
        }

        base.Update();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "wall" || collision.gameObject.tag == "crate") {
            moveDirection.y *= -1;
            moveDirection.x *= -1;
        }

        base.OnCollisionEnter2D(collision);

    }
}
