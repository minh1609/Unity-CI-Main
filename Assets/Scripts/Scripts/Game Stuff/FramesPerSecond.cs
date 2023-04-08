using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FramesPerSecond : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI frames;
    private float hudRefreshRate = 1;
    private float timer;
    
    void Start()
    {
        frames = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            frames.text = "FPS: " + fps;
            timer = Time.unscaledTime + hudRefreshRate;
        }
    }
}
