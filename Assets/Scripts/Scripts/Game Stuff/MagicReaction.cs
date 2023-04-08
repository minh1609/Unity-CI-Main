using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicReaction : MonoBehaviour
{
    public Inventory playerInventory;
    public Signal magicSignal;
    public Item greenPotion;
    public GameObject magicMeter;

    public void Use(int amountToIncrease)
    {
        if (greenPotion.numberHeld > 0)
        {
            FindObjectOfType<AudioManager>().Play("potion");
            playerInventory.currentMagic += amountToIncrease;
            if (playerInventory.currentMagic > playerInventory.maxMagic)
                playerInventory.currentMagic = playerInventory.maxMagic;
            magicSignal.Raise();

            if (magicMeter != null)
            {
                MagicManager meter = magicMeter.transform.GetComponentInChildren<MagicManager>();
                meter.AddMagic();
            }
        }
    }
}
