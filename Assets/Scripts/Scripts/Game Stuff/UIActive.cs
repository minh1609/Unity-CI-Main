using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIActive : MonoBehaviour
{
    [Header("UI")]
    public BoolValue UI;

    private void OnEnable()
    {
        UI.RunTimeValue = true;
    }

    private void OnDisable()
    {
        UI.RunTimeValue = false;
    }
}
