using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreHit : MonoBehaviour
{
    public float thrust;
    public float knockTime;
    public float damage;
    public float duration = 2f;
    MeleeEnemy thisEnemy;

    private void Awake()
    {
        thisEnemy = GetComponentInParent<MeleeEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                if (other.GetComponent<PlayerController>().currentState != PlayerState.stagger && !other.GetComponent<PlayerController>().invulnerable && other.GetComponent<PlayerController>().currentState != PlayerState.parry)
                {
                    hit.GetComponent<PlayerController>().currentState = PlayerState.stagger;
                    other.GetComponent<PlayerController>().Knock(knockTime, damage);
                    hit.velocity = Vector2.zero;
                    Vector2 difference = (hit.transform.position - transform.position);
                    difference = difference.normalized * thrust;
                    hit.AddForce(difference, ForceMode2D.Impulse);
                }
                if (other.GetComponent<PlayerController>().currentState == PlayerState.parry)
                {
                    thisEnemy.ParriedOn(duration);
                }
            }
        }
    }
}
