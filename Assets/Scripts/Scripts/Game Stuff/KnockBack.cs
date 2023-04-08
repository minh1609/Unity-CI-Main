using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{

    [SerializeField] private float thrust;
    [SerializeField] private float knockTime;


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.isTrigger)
        {
            Rigidbody2D hit = other.GetComponentInParent<Rigidbody2D>();
            PlayerController player = other.GetComponentInParent<PlayerController>();
            if (hit != null)
            {
                if (player.currentState != PlayerState.stagger && !player.invulnerable && player.currentState != PlayerState.parry)
                {
                    player.currentState = PlayerState.stagger;
                    player.Knock(knockTime);
                    CreateThrust(hit);
                    player.enterInvulnerable();
                }
                
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Pot>().Smash();
        }

        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player") && other.isTrigger)
        {
            Rigidbody2D hit = other.GetComponentInParent<Rigidbody2D>();
            if(hit != null)
           {
                if (other.gameObject.CompareTag("enemy"))
                {
                    Enemy enemy = other.GetComponentInParent<Enemy>();
                    CreateThrust(hit);
                    enemy.currentState = EnemyState.stagger;
                    enemy.Knock(hit, knockTime);
                }
                if (other.gameObject.CompareTag("Player"))
                {
                    PlayerController player = other.GetComponentInParent<PlayerController>();
                    if (player.currentState != PlayerState.stagger && !player.invulnerable && player.currentState != PlayerState.parry)
                    {
                        player.currentState = PlayerState.stagger;
                        player.Knock(knockTime);
                        CreateThrust(hit);
                        player.enterInvulnerable();
                    }
                }
            }
        }
    }

    void CreateThrust(Rigidbody2D hit)
    {
        hit.velocity = Vector2.zero;
        Vector2 difference = (hit.transform.position - transform.position);
        difference = difference.normalized * thrust;
        hit.AddForce(difference, ForceMode2D.Impulse);
    }



}
