using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForegroundClouds : MonoBehaviour
{
    public float speed;
    private float height, length;
    public Vector3 startpos;
    private float dist;
    private float timePassed = 0;
    private void Awake()
    {
        height = GetComponent<Image>().sprite.rect.height;
        length = GetComponent<Image>().sprite.rect.width;
        startpos = transform.position;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        transform.position = new Vector3(startpos.x - speed * Time.time, transform.position.y, transform.position.z);
        dist = speed * timePassed *200/1920;

        if (Math.Abs(dist) > length)
        {
            startpos.x -= transform.position.x * 2;
            timePassed = 0;
        }
    }
}
