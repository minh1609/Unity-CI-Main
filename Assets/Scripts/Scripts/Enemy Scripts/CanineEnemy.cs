using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CanineEnemy : Enemy
{
    [Header("A* Script")]
    public Transform target;
    public float speed = 200f;
    public float nextWayPointDistance = 3f;

    public Transform enemy;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public float chaseRadius;
    public float attackRadius;
    public Animator animator;
    private Vector2 positionDif;
    private bool jump = true;
    private float jumpTimer = 0;
    [SerializeField] private float jumpInterval = 2f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        jumpTimer = 0f;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (jumpTimer <= Time.time)
        {
            jump = true;
        }


        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                changeAnim(rb.velocity);
                ChangeState(EnemyState.walk);
                animator.SetBool("run", true);
                animator.SetFloat("lastMoveX", rb.velocity.x);
                animator.SetFloat("lastMoveY", rb.velocity.y);

                if (path == null)
                    return;
                if (currentWaypoint >= path.vectorPath.Count)
                {
                    reachedEndOfPath = true;
                    return;
                }
                else
                    reachedEndOfPath = false;

                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized; ;
                Vector2 force = direction * speed * Time.deltaTime;

                rb.AddForce(force);

                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWayPointDistance)
                {
                    currentWaypoint++;
                }

                if (target.position.x > rb.position.x)
                {
                    enemy.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (target.position.x < rb.position.x)
                {
                    enemy.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
           
        }

        else if (Vector3.Distance(target.position, transform.position) <= attackRadius && Vector3.Distance(target.position, transform.position) <= chaseRadius && currentState != EnemyState.stagger)
        {
            StartCoroutine(AttackCo());
        }

        else if (Vector3.Distance(target.position, transform.position) > chaseRadius && currentState != EnemyState.stagger)
        {
            animator.SetBool("run", false);
            ChangeState(EnemyState.idle);
            rb.velocity = Vector2.zero;
        }
    }

    public override void Knock(Rigidbody2D myRigidbody, float knockTime)
    {
        FindObjectOfType<AudioManager>().Play("damage" + Random.Range(1, 3));
        animator.SetBool("attacking", false);
        animator.SetBool("idle", false);
        animator.SetBool("run", false);
        rb.velocity = Vector2.zero;
        StartCoroutine(StaggerCo(knockTime));
        base.Knock(myRigidbody, knockTime);
    }

    public void changeAnim(Vector2 direction)
    {
        direction = direction.normalized;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
    }

    public IEnumerator AttackCo()
    {
        if (currentState != EnemyState.stagger && jump)
        {
            FindObjectOfType<AudioManager>().Play("bark");
            Vector2 direction = new Vector2(target.position.x - rb.position.x, target.position.y - rb.position.y);
            direction.Normalize();
            Vector2 force = direction * 500;
            rb.AddForce(force);
            currentState = EnemyState.attack;
            animator.SetBool("attacking", true);
            jumpTimer = Time.time + jumpInterval;
            jump = false;
            yield return new WaitForSeconds(1f);
            currentState = EnemyState.idle;
            animator.SetBool("attacking", false);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator StaggerCo(float duration)
    {
        animator.SetBool("stagger", true);
        yield return new WaitForSeconds(duration);
        animator.SetBool("stagger", false);
        yield return null;
    }
}
