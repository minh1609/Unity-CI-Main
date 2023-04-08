using UnityEngine;

public class HeartContainer : PowerUp
{
    public FloatValue heartContainers;
    public FloatValue playerHealth;
    public int maxHearts;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !other.isTrigger)
        {
            if (heartContainers.RuntimeValue<maxHearts)
                heartContainers.RuntimeValue++;
            playerHealth.RuntimeValue = heartContainers.RuntimeValue * 2;
            playerHealth.initialValue = heartContainers.RuntimeValue * 2;
            powerUpSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
