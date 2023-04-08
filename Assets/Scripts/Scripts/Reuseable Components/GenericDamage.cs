
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GenericDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private string[] otherTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
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

    private void OnTriggerStay2D(Collider2D other)
    {
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
