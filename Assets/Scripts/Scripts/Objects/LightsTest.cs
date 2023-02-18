using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsTest : MonoBehaviour
{

    public Light2D light;
    public float r = 0;
    public float g = 0;
    public float b = 0;
    public float timeOfDay;
    public int lastTemp = 0;
    public float timeScale = 1;


    // Start is called before the first frame update
    void Start()
    {
        light.color = new Color(r, g, b);
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay += Time.deltaTime;
        int temp = (int)timeOfDay;
        light.color = new Color(r, g, b);
        Time.timeScale = timeScale;
        
        if (temp > lastTemp)
        {
            if (temp <= 200)
            {
                r -= 0.001f;
                g -= 0.0025f;
                b -= 0.0025f;
            }

            else if (temp <= 400 && temp > 200)
            {
                r -= 0.0015f;
                b += 0.0015f;
            }

            else if (temp <= 600 && temp > 400)
            {
                r += 0.0025f;
                g += 0.0025f;
                b += 0.001f;
            }

            else if (temp > 600)
            {
                timeOfDay = 0;
                temp = 0;
            }

            lastTemp = temp;
        }

    }
}
