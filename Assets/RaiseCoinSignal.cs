using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseCoinSignal : PowerUp
{
    // Start is called before the first frame update
    void Start()
    {
        powerUpSignal.Raise();
        Destroy(this.gameObject);
    }

}
