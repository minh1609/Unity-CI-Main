using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : GenericHealth
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public override void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            enemy.DeathEffect();
            enemy.MakeLoot();
            if (enemy.roomSignal != null)
            {
                enemy.roomSignal.Raise();
            }
            this.transform.parent.gameObject.SetActive(false);
        }
        else 
            enemy.StaggerColor();
    }
}
