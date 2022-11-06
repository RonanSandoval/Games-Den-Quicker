using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Info : MonoBehaviour
{
    public GameController gc;
    RectTransform rt;

    Image img;

    public Sprite[] images;
    public int currentImage;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        currentImage = 5;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.currentState != GameController.GameState.Playing) {
            rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, new Vector3(0.0f, 0.0f, 0f), Time.deltaTime * 10);
        } else {
            rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, new Vector3(0.0f, -600.0f, 0f), Time.deltaTime * 10);
        }

        img.sprite = images[currentImage];
    }
}