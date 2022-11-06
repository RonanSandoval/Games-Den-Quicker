using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartCounter : MonoBehaviour
{
    public int heartID;
    Image img;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health >= heartID) {
            img.color = new Color(1f,1f,1f,1f);
        } else {
            img.color = new Color(0f,0f,0f,1f);
        }
    }
}
