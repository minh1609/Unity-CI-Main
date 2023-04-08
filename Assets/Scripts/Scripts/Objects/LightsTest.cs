using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsTest : MonoBehaviour
{

    public Light2D light;
    private float r;
    private float g;
    private float b;
    public FloatValue timeOfDay;

    // Start is called before the first frame update
    void Start()
    {
        light.color = new Color(r, g, b);
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay.RuntimeValue += Time.deltaTime;
        light.color = new Color(r, g, b);

        if (timeOfDay.RuntimeValue < 0)
            timeOfDay.RuntimeValue = 0;

            if (timeOfDay.RuntimeValue <= 200)
            {
                r = 1-0.001f* timeOfDay.RuntimeValue;
                g = 1-0.0025f* timeOfDay.RuntimeValue;
                b = 1-0.0025f* timeOfDay.RuntimeValue;
            }

            else if (timeOfDay.RuntimeValue <= 400 && timeOfDay.RuntimeValue > 200)
            {
                r = 0.8f-0.0015f* (timeOfDay.RuntimeValue-200);
                b = 0.5f+0.0015f* (timeOfDay.RuntimeValue-200);
                g = 0.5f;
            }

            else if (timeOfDay.RuntimeValue <= 600 && timeOfDay.RuntimeValue > 400)
            {
                r = 0.5f+0.0025f* (timeOfDay.RuntimeValue - 400);
                g = 0.5f+0.0025f* (timeOfDay.RuntimeValue - 400);
                b = 0.8f+0.001f* (timeOfDay.RuntimeValue - 400);
            }

            else if (timeOfDay.RuntimeValue > 600)
            {
                timeOfDay.RuntimeValue = 0;
            }
    }
}
