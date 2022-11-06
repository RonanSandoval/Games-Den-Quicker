using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Image image;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.timestopCooldown > 0) {
            image.fillAmount = player.timestopCooldown / 15f;
        } else {
            image.fillAmount = 0;
        }
        
    }
}
