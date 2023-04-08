using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlitClone : MonoBehaviour
{
    public SpriteRenderer clone;
    public SpriteRenderer target;

    // Update is called once per frame
    void Update()
    {
        clone.sprite = target.sprite;
    }
}
