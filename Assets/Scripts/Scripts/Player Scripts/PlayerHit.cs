using UnityEngine;

public class PlayerHit : MonoBehaviour
{
	[SerializeField] private string[] otherTag;
	[SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("breakable"))
		{
			other.GetComponent<Pot>().Smash();
		}

        foreach (string tag in otherTag)
        {
            if (other.gameObject.CompareTag(tag) && other.isTrigger)
            {
                GenericHealth temp = other.GetComponent<GenericHealth>();
                if (temp)
                {
                    temp.Damage(damage);
                }
            }
        }
    }
}