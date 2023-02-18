using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundNPC : Sign
{
    Vector3 moveDirection;
    private Transform myTransform;
    private Rigidbody2D myRB;
    private Animator anim;
    public float speed;
    public Collider2D bounds;
    private bool isMoving;
    public float maxMoveTime;
    public float minMoveTime;
    private float moveTimeSeconds;
    public float maxWaitTime;
    public float minWaitTime;
    private float WaitTimeSeconds;

    // Start is called before the first frame update
    void Start()
    {
        moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
        WaitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
        myTransform = GetComponent<Transform>();
        myRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        ChangeDirection();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (isMoving)
        {
            moveTimeSeconds -= Time.deltaTime;
            if (moveTimeSeconds <= 0)
            {
                moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
                isMoving = false;
                ChooseDifferentDirection();
            }
            if (!playerInRange)
                Move();
            else
                isMoving = false;
        }
        else
        {
            anim.SetBool("moving", false);
            WaitTimeSeconds -= Time.deltaTime;
            if (WaitTimeSeconds <= 0)
            {
                isMoving = true;
                WaitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
            }
        }
    }

    private void ChooseDifferentDirection()
    {
        Vector3 temp = moveDirection;
        ChangeDirection();
        int loops = 0;
        while (temp == moveDirection && loops < 100)
        {
            loops++;
            ChangeDirection();
        }
    }

    private void Move()
    {
        anim.SetBool("moving", true);
        Vector3 temp = myTransform.position + moveDirection * speed * Time.deltaTime;
        if (bounds.bounds.Contains(temp))
            myRB.MovePosition(myTransform.position + moveDirection * speed * Time.deltaTime);
        else
            ChangeDirection();
    }

    void ChangeDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                moveDirection = Vector3.right;
                break;

            case 1:
                moveDirection = Vector3.left;
                break;

            case 2:
                moveDirection = Vector3.up;
                break;

            case 3:
                moveDirection = Vector3.down;
                break;
            default:
                break;
        }
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        anim.SetFloat("moveX", moveDirection.x);
        anim.SetFloat("moveY", moveDirection.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ChooseDifferentDirection();
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("enemy"))
            myRB.bodyType = RigidbodyType2D.Static;
        else
            myRB.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("enemy"))
            myRB.bodyType = RigidbodyType2D.Dynamic;
    }
}
