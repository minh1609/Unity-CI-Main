using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk, attack, interact, stagger, idle, parry
}

public class PlayerController : MonoBehaviour
{
    public PlayerState currentState;

    [Header("Player Movement")]
    public float speed;
    private Vector2 playerVelocity;
    private Rigidbody2D myRigidbody;
    private Animator animator;

    [Header("Health Properties")]
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    private float invul_time = 0;
    public bool invulnerable;

    [Header("Starting Location / Face Direction")]
    public VectorValue startingPosition;
    public StartDirection startDirection;
    public GameObject sceneStartPos;

    [Header("Inventory/Items")]
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;

    [Header("Script for Old(Unused) Camera Script")]
    public Signal screenKick;

    [Header("Projectiles")]
    public Signal reduceMagic;
    public GameObject projectile;
    public Item bow;
    public Item sword;

    [Header("UI")]
    public BoolValue UIActive;

    [Header("Dagger")]
    public float daggerCD = 0;
    public float daggerDelay = 2f;
    public Item dagger;

    // Start is called before the first frame update
    void Awake()
    {
        currentState = PlayerState.idle;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        sceneStartPos.GetComponent<SceneStartPosition>().movePlayer();
        animator.SetFloat("lastMoveX", startDirection.startX);
        animator.SetFloat("lastMoveY", startDirection.startY);
        transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (invul_time >= Time.time)
        {
            StaggerColor();
        }
        else
            invulnerable = false;

        animator.SetBool("moving", false);
        if (UIActive.RunTimeValue)
            return;
        if (currentState == PlayerState.interact)
        {
            return;
        }
        if (myRigidbody.velocity == Vector2.zero && currentState != PlayerState.attack && currentState != PlayerState.parry)
            currentState = PlayerState.idle;
        if ((Input.GetButton("attack")) && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            if (playerInventory.CheckForItem(sword))
                StartCoroutine(AttackCo());
        }
        else if (Input.GetButtonDown("Second Weapon") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            if (playerInventory.CheckForItem(bow))
            {
                StartCoroutine(SecondAttackCo());
            }
        }
        else if (Input.GetKeyDown(KeyCode.V) && currentState != PlayerState.attack && currentState != PlayerState.stagger && daggerUsable())
        {
            StartCoroutine(DaggerParryCo());
        }
        if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAndMove();
        }
    }

    private void UpdateAndMove()
    {
        playerVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        playerVelocity.Normalize();
        myRigidbody.velocity = playerVelocity * speed;
        animator.SetFloat("moveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("moveY", Input.GetAxisRaw("Vertical"));
        if (myRigidbody.velocity != Vector2.zero)
        {
            animator.SetBool("moving", true);
            currentState = PlayerState.walk;
        }
        if(playerVelocity != Vector2.zero)
        {
            animator.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }
    }

    private IEnumerator AttackCo()
    {
        myRigidbody.velocity = Vector2.zero;
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(2f/3f);
        if (currentState != PlayerState.interact && currentState != PlayerState.stagger)
        {
            currentState = PlayerState.idle;
        }
    }

    private IEnumerator SecondAttackCo()
    {
        if (myRigidbody.velocity.x != 0 && myRigidbody.velocity.y != 0)
            animator.SetFloat("lastMoveY", 0);
        //animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        //yield return null;
        //animator.SetBool("attacking", false);
        MakeArrow();
        myRigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.33f);
        if (currentState != PlayerState.interact && currentState != PlayerState.stagger)
        {
            currentState = PlayerState.idle;
        }
    }

    private void MakeArrow()
    {
        if (playerInventory.currentMagic > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            Vector2 temp2 = new Vector2(animator.GetFloat("lastMoveX"), animator.GetFloat("lastMoveY"));
            Arrow arrow = GameObject.Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>();
            if (temp != Vector2.zero)
                arrow.Setup(temp, ChooseArrowDirection());
            else
                arrow.Setup(temp2, ChooseArrowDirection());
            playerInventory.ReduceMagic(arrow.magicCost);
            reduceMagic.Raise();
        }
    }

    private IEnumerator DaggerParryCo()
    {
        myRigidbody.velocity = Vector2.zero;
        daggerUsed();
        currentState = PlayerState.parry;
        animator.SetBool("Dagger", true);
        invulnerable = true;
        invul_time = Time.time + 0.3f;
        yield return null;
        animator.SetBool("Dagger", false);
        yield return new WaitForSeconds(2f / 3f);
        if (currentState != PlayerState.interact && currentState != PlayerState.stagger)
        {
            currentState = PlayerState.idle;
        }
    }

    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg;
        if (temp == 0)
            temp = Mathf.Atan2(animator.GetFloat("lastMoveY"), animator.GetFloat("lastMoveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    public void RaiseItem()
    {
        if (playerInventory.currentItem != null)
        {
            if (currentState != PlayerState.interact)
            {
                animator.SetBool("receiveItem", true);
                myRigidbody.velocity = Vector2.zero;
                currentState = PlayerState.interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                animator.SetBool("receiveItem", false);
                currentState = PlayerState.idle;
                receivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
    }

    public void Knock(float knockTime, float damage)
    {
        if (!invulnerable)
        {
            currentHealth.RuntimeValue -= damage;
            animator.SetBool("staggered", true);
            playerHealthSignal.Raise();
            invulnerable = true;
            invul_time = Time.time + 2f;

            if (currentHealth.RuntimeValue > 0)
            {
                StartCoroutine(KnockCo(knockTime));
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        //screenKick.Raise();
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            animator.SetBool("staggered", false);
            currentState = PlayerState.idle;
        }
    }

    private void StaggerColor()
    {
        StartCoroutine(StaggerColorCo());
    }

    private IEnumerator StaggerColorCo()
    {
        GetComponent<SpriteRenderer>().material.color = new Vector4(1, 1, 1, 1);
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().material.color = new Vector4(0, 0, 0, 0);
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().material.color = new Vector4(1, 1, 1, 1);
        yield return null;
    }

    public void daggerUsed()
    {
        daggerCD = Time.time + daggerDelay;
    }

    public bool daggerUsable()
    {
        if (daggerCD <= Time.time && playerInventory.CheckForItem(dagger))
        {
            return true;
        }
        else
            return false;
    }
}
