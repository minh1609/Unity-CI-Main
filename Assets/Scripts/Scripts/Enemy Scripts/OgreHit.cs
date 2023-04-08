using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreHit : MonoBehaviour
{
    [SerializeField] private float thrust;
    [SerializeField] private float knockTime;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float damage;
    MeleeEnemy thisEnemy;

    private void OnEnable()
    {
        thisEnemy = GetComponentInParent<MeleeEnemy>();
        FindObjectOfType<AudioManager>().Play("strong-hit");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponentInParent<Rigidbody2D>();
            PlayerController player = other.GetComponentInParent<PlayerController>();
            GenericHealth temp = other.GetComponent<GenericHealth>();
            if (hit != null)
            {
                if (player.currentState != PlayerState.stagger && !player.invulnerable && player.currentState != PlayerState.parry)
                {
                    player.currentState = PlayerState.stagger;
                    player.Knock(knockTime);
                    hit.velocity = Vector2.zero;
                    Vector2 difference = (hit.transform.position - transform.position);
                    difference = difference.normalized * thrust;
                    hit.AddForce(difference, ForceMode2D.Impulse);
                    if (temp)
                        temp.Damage(damage);
                }
                if (player.currentState == PlayerState.parry)
                {
                    thisEnemy.ParriedOn(duration);
                }
            }
        }
    }
}
