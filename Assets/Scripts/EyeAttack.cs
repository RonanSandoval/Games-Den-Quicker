using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAttack : MonoBehaviour
{
    public float shotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        //transform.position = Vector3.MoveTowards(transform.position, targetPos, 8 * Time.deltaTime);
        transform.Translate(transform.right * Time.deltaTime * shotSpeed, Space.World);
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.onHit(1, transform.position);
            Destroy(gameObject); 
        }
        if (collision.gameObject.tag == "wall" || collision.gameObject.tag == "crate") {
            Destroy(gameObject);
        }
        
        
    }  
}
