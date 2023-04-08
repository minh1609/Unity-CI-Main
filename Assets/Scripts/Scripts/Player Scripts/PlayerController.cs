using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    walk, attack, interact, stagger, idle, parry, dash
}

public class PlayerController : MonoBehaviour
{
    public PlayerState currentState;

    [Header("Player Movement")]
    public float speed;
    private Vector2 playerVelocity;
    private Rigidbody2D myRigidbody;
    private Animator animator;
    [HideInInspector] public float dashDuration;
    public float dashMana;
    public bool dashUsed;
    [SerializeField] private Item ninjaBookDash;
    [SerializeField] private AfterImage afterImage;


    [Header("Health Properties")]
    //public FloatValue currentHealth;
    //public Signal playerHealthSignal;
    [HideInInspector]  public float invul_time = 0;
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

    [Header("Sword")]
    public Item sword;

    [Header("Dagger")]
    [HideInInspector] public float daggerCD = 0;
    public float daggerDelay = 2f;
    public Item dagger;

    [Header("UI")]
    public BoolValue UIActive;

    [Header("Materials")]
    [SerializeField] private Material spriteLitDefault;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<SpriteRenderer>().material = spriteLitDefault;
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

        if (dashDuration < Time.time)
        {
            dashUsed = false;
        }

        if (UIActive.RunTimeValue)
            return;

        if (currentState == PlayerState.interact)
        {
            return;
        }

        if (animator.GetBool("Dash"))
        {
            if (dashDuration < Time.time || myRigidbody.velocity == Vector2.zero || currentState == PlayerState.stagger && currentState != PlayerState.interact)
            {
                animator.SetBool("Dash", false);
                afterImage.makeImages = false;
                if (currentState != PlayerState.stagger && currentState != PlayerState.interact)
                {
                    currentState = PlayerState.idle;
                }
            }
        }
        else if (!animator.GetBool("Dash"))
        {
            afterImage.makeImages = false;
        }

        if (myRigidbody.velocity == Vector2.zero && currentState != PlayerState.attack && currentState != PlayerState.parry && currentState != PlayerState.stagger && currentState != PlayerState.dash)
        {
            currentState = PlayerState.idle;
            animator.SetBool("moving", false);
        }

        if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAndMove();
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (UIActive.RunTimeValue)
            return;
        myRigidbody.velocity = Vector2.zero;
        if (context.performed && currentState != PlayerState.attack && currentState != PlayerState.stagger && currentState != PlayerState.dash)
        {
            if (playerInventory.CheckForItem(sword))
            {
                dashHasBeenUsed();
                dashDuration = Time.time + 0.6f;
                StartCoroutine(AttackCo());
            }
        }
        if (context.canceled)
            return;
    }

    public void Parry(InputAction.CallbackContext context)
    {
        if (UIActive.RunTimeValue)
            return;
        if (context.performed && currentState != PlayerState.attack && currentState != PlayerState.stagger && daggerUsable() && currentState != PlayerState.dash)
        {
            StartCoroutine(DaggerParryCo());
        }
    }

    public void Arrow(InputAction.CallbackContext context)
    {
        if (UIActive.RunTimeValue)
            return;
        if (context.performed && currentState != PlayerState.attack && currentState != PlayerState.stagger && currentState != PlayerState.dash)
        {
            if (playerInventory.CheckForItem(bow))
            {
                StartCoroutine(SecondAttackCo());
            }
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
        animator.SetTrigger("meleeOpener");
        currentState = PlayerState.attack;
        yield return null;
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
            FindObjectOfType<AudioManager>().PlayWithSettings("woosh", 0.1f, 2f);
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
        animator.SetTrigger("Dagger");
        invulnerable = true;
        invul_time = Time.time + 10f;
        yield return new WaitForSeconds(2f / 3f);
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

    public void Knock(float knockTime)
    {
        if (!invulnerable)
        {
            //currentHealth.RuntimeValue -= damage;
            animator.SetBool("staggered", true);
            //playerHealthSignal.Raise();
            StartCoroutine(KnockCo(knockTime));

            //if (currentHealth.RuntimeValue > 0)
            //{
            //    StartCoroutine(KnockCo(knockTime));
            //}
            //else
            //{
            //    this.gameObject.SetActive(false);
            //}
        }
    }

    public void enterInvulnerable()
    {
        invulnerable = true;
        invul_time = Time.time + 2f;
    }

    private IEnumerator KnockCo(float knockTime)
    {
        //screenKick.Raise();
        if (myRigidbody != null)
        {
            FindObjectOfType<AudioManager>().Play("damage" + Random.Range(1, 3));
            StaggerColor();
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
        GetComponent<SpriteRenderer>().material.color = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
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
    private void dashHasBeenUsed()
    {
        dashUsed = true;
    }

    public bool dashUsable()
    {

        if (!dashUsed && playerInventory.CheckForItem(ninjaBookDash))
            return true;
        else
            return false;
    }

    public void startDash(Vector2 direction)
    {
        StartCoroutine(dashCo(direction));
    }
    private IEnumerator dashCo(Vector2 direction)
    {
        currentState = PlayerState.dash;
        dashHasBeenUsed();
        myRigidbody.AddForce(direction * 10, ForceMode2D.Impulse);
        animator.SetBool("Dash", true);
        afterImage.makeImages = true;
        playerInventory.ReduceMagic(dashMana);
        reduceMagic.Raise();
        yield return null;
    }
}
