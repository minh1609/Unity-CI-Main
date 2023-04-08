using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{

    public float thrust;
    public float knockTime;
    public float damage;
    string thisTag;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                if (other.GetComponent<PlayerController>().currentState != PlayerState.stagger && !other.GetComponent<PlayerController>().invulnerable && other.GetComponent<PlayerController>().currentState != PlayerState.parry)
                {
                    CreateThrust(hit);
                    hit.GetComponent<PlayerController>().currentState = PlayerState.stagger;
                    other.GetComponent<PlayerController>().Knock(knockTime, damage);
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

        if (other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if(hit != null)
           {               
                if (other.gameObject.CompareTag("enemy") && other.isTrigger)
                {
                    CreateThrust(hit);
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    other.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                }
                if (other.gameObject.CompareTag("Player"))
                {
                    if (other.GetComponent<PlayerController>().currentState != PlayerState.stagger && !other.GetComponent<PlayerController>().invulnerable && other.GetComponent<PlayerController>().currentState != PlayerState.parry)
                    {
                        CreateThrust(hit);
                        hit.GetComponent<PlayerController>().currentState = PlayerState.stagger;
                        other.GetComponent<PlayerController>().Knock(knockTime, damage);
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
