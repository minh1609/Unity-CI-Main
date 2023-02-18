using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CloudSilhouette : MonoBehaviour
{
    public float speed;
    private float height, length;
    public Vector3 startpos;
    private float yOffset = 0;
    private float moveUpDown = 1;
    private float dist;
    private float timePassed = 0;
    private void Awake()
    {
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        startpos = transform.position;
    }

    void Update()
    {
        timePassed += Time.deltaTime;
        yOffset += 0.4f*Time.deltaTime * moveUpDown;
        transform.position = new Vector3(startpos.x - speed * Time.time, startpos.y + yOffset, transform.position.z);
        dist = speed * timePassed;

        if (Math.Abs(dist) > length)
        {
            startpos.x -= transform.position.x * 2;
            moveUpDown = -moveUpDown;
            timePassed = 0;
        }
    }
}
