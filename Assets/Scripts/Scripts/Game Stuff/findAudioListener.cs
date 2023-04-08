using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class findAudioListener : MonoBehaviour
{

    public AudioListener audioListener;

    // Start is called before the first frame update
    void Start()
    {
        audioListener = FindObjectOfType<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
