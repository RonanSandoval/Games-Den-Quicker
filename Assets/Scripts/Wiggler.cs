using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wiggler : MonoBehaviour
{

    RectTransform rt;
    float rotator;

    public float offset;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        rotator = offset;
    }

    // Update is called once per frame
    void Update()
    {
        rotator += Time.deltaTime;
        rt.rotation = Quaternion.Euler(0,0, Mathf.Cos(rotator * 2) * 5f);
    }
}
