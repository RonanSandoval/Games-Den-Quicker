using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEnemy : Enemy
{
    private float attackCooldown;
    public float attackTime;

    public GameObject attack;

    protected override void Start() {
        base.Start();
        knockable = true;
        damageable = true;
        attackCooldown = 1f;
    }

    protected override void Update() {
         if (player.timeStop <= 0) {
            Vector3 dir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (attackCooldown > 0) {
                attackCooldown -= Time.deltaTime;
                
            } else {
                attackCooldown = attackTime + Random.Range(-0.3f, 0.3f);      
                GameObject attackObject = Instantiate(attack, transform.position, transform.rotation) as GameObject;       
            }
            
        }

        base.Update();
    }
}
