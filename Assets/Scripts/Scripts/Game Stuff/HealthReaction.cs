using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReaction : MonoBehaviour
{
    public FloatValue playerHealth;
    public Signal healthSignal;
    public Item redPotion;

    public void Use(int amountToIncrease)
    {
        if (redPotion.numberHeld > 0)
        {
            playerHealth.RuntimeValue += amountToIncrease;
            if (playerHealth.RuntimeValue > playerHealth.initialValue)
                playerHealth.RuntimeValue = playerHealth.initialValue;
            healthSignal.Raise();
        }
    }
}
