using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicReaction : MonoBehaviour
{
    public FloatValue playerMagic;
    public Signal magicSignal;
    public InventoryItem greenPotion;

    public void Use(int amountToIncrease)
    {
        if (greenPotion.numberHeld > 0)
        {
            playerMagic.RuntimeValue += amountToIncrease;
            greenPotion.numberHeld--;
            if (playerMagic.RuntimeValue > playerMagic.initialValue)
                playerMagic.RuntimeValue = playerMagic.initialValue;
            magicSignal.Raise();
        }
    }
}
