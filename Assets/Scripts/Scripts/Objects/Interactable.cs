using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool playerInRange = false;
    public bool showContextClue = true;
    public Signal contextClue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && showContextClue)
        {
            contextClue.Raise();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && showContextClue)
        {
            contextClue.Raise();
            playerInRange = false;
        }
    }
}
