using System.Collections;
using UnityEngine;

public enum EnemyState
{
    idle, walk, attack, stagger
}

public class Enemy : MonoBehaviour
{
    [Header("State Machine")]
    public EnemyState currentState;

    [Header("Enemy Stats")]
    //public FloatValue maxHealth;
    //protected float health;
    public string enemyName;
    public float moveSpeed;
    public Vector2 homePosition;
    [SerializeField] private  bool spawnAtTransform = true;

    [Header("On Death")]
    public GameObject deathEffect;
    private float deathEffectDelay = 1f;
    public LootTable thisLoot;

    [Header("Death Signals")]
    public Signal roomSignal;

    private void Awake()
    {
        //health = maxHealth.initialValue;
    }

    public virtual void OnEnable()
    {
        Vector2 temp = transform.position;

        transform.position = homePosition;
        //health = maxHealth.initialValue;
        currentState = EnemyState.idle;

        if (spawnAtTransform)
        {
            transform.position = temp;
        }
    }

    //private void TakeDamage(float damage)
    //{
    //    health -= damage;
    //    if (health <= 0)
    //    {
    //        DeathEffect();
    //        MakeLoot();
    //        if (roomSignal != null)
    //            roomSignal.Raise();
    //        this.gameObject.SetActive(false);
    //    }
    //    else
    //        StaggerColor();
    //}

    public void MakeLoot()
    {
        if (thisLoot != null)
        {
            PowerUp current = thisLoot.LootPowerUp();
            if(current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    public void DeathEffect()
    {
        if(deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDelay);
        }
    }

    public virtual void Knock(Rigidbody2D myRigidbody, float knockTime)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        //TakeDamage(damage);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null)
        {
            FindObjectOfType<AudioManager>().Play("damage" + Random.Range(1, 3));
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }

    public void removeCollider(Collider2D collider, float duration)
    {
        StartCoroutine(removeColliderCo(collider, duration));
    }

    private IEnumerator removeColliderCo(Collider2D collider, float duration)
    {
        collider.enabled = false;
        yield return new WaitForSeconds(duration);
        collider.enabled = true;
        yield return null;
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
            currentState = newState;
    }

    public void StaggerColor()
    {
        StartCoroutine(StaggerColorCo());
    }

    private IEnumerator StaggerColorCo()
    {
        GetComponent<SpriteRenderer>().material.color = new Vector4(1, 0.5f, 0.5f, 1);
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().material.color = new Vector4(0, 0, 0, 0.5f);
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().material.color = new Vector4(1, 1, 1, 1);
        yield return null;
    }
}
