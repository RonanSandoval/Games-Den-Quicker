using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Vector3 shootDirection;
    public float shotSpeed;

    public float lifespan;

    // Start is called before the first frame update
    void Start()
    {
        shootDirection = Input.mousePosition;
        shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);
        shootDirection = shootDirection - transform.position;
        shootDirection.z = 0.0f;
        shootDirection.Normalize();
        transform.rotation = Quaternion.LookRotation(Vector3.forward, shootDirection);

        lifespan = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {   
        //transform.position = Vector3.MoveTowards(transform.position, targetPos, 8 * Time.deltaTime);
        transform.Translate(shootDirection * Time.deltaTime * shotSpeed, Space.World);
        lifespan -= Time.deltaTime;

        if (lifespan <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "wall")
        {
            Destroy (gameObject);
        }
    }  
}