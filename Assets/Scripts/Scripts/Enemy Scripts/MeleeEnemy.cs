using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    private Rigidbody2D myRigidbody;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Animator animator;
    private Vector2 positionDif;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        animator.SetFloat("lastMoveX", 0);
        animator.SetFloat("lastMoveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        positionDif = target.position - transform.position;
        positionDif.Normalize();
        CheckDistance();
    }

    public void CheckDistance()
    {

        distance = Vector3.Distance(target.position, transform.position);
        if (distance <= chaseRadius && distance > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                myRigidbody.velocity = positionDif * moveSpeed;
                changeAnim(myRigidbody.velocity);
                ChangeState(EnemyState.walk);
                animator.SetBool("walking", true);
                animator.SetFloat("lastMoveX", myRigidbody.velocity.x);
                animator.SetFloat("lastMoveY", myRigidbody.velocity.y);
            }
        }
        else if (distance <= attackRadius && distance <= chaseRadius)
        {
            if (currentState != EnemyState.attack)
            {
                StartCoroutine(AttackCo());
            }
        }

        else if (distance > chaseRadius)
        {
            animator.SetBool("walking", false);
            ChangeState(EnemyState.idle);
            myRigidbody.velocity = Vector2.zero;
        }
    }

    public void changeAnim(Vector2 direction)
    {
        direction = direction.normalized;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    public IEnumerator AttackCo()
    {
        if (currentState != EnemyState.stagger)
        {
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.attack;
            animator.SetBool("attacking", true);
            yield return null;
            animator.SetBool("attacking", false);
            yield return new WaitForSeconds(1f);
            if (currentState != EnemyState.stagger)
                currentState = EnemyState.idle;
        }
    }

    public void ParriedOn(float duration)
    {
        StartCoroutine(ParriedOnCo(duration));
    }
    public IEnumerator ParriedOnCo(float duration)
    {
        ChangeState(EnemyState.stagger);
        animator.SetBool("stagger", true);
        StaggerColor();
        yield return new WaitForSeconds(duration);
        animator.SetBool("stagger", false);
        ChangeState(EnemyState.idle);
    }
}
