using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParriableDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private string[] otherTag;
    [SerializeField] private float parryStaggerTime;


    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string tag in otherTag)
        {
            if (other.gameObject.CompareTag(tag) && other.isTrigger)
            {
                GenericHealth temp = other.GetComponent<GenericHealth>();
                if (other.gameObject.CompareTag("Player"))
                {
                    PlayerController player = other.GetComponentInParent<PlayerController>();
                    if (player.currentState == PlayerState.parry && temp)
                    {
                        Enemy thisEnemy = GetComponentInParent<Enemy>();
                        Rigidbody2D thisRB = GetComponentInParent<Rigidbody2D>();
                        Collider2D thisCollider = GetComponent<Collider2D>();

                        thisEnemy.ChangeState(EnemyState.stagger);
                        thisEnemy.removeCollider(thisCollider, parryStaggerTime);
                        thisEnemy.Knock(thisRB, parryStaggerTime);
                        thisEnemy.StaggerColor();
                    }
                    else if (temp)
                    {
                        temp.Damage(damage);
                    }
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
