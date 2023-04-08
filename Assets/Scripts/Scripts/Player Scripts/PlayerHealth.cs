using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerHealth : GenericHealth
{
    [SerializeField] private Signal healthSignal;
    private PlayerController player;
    [SerializeField] private Signal gameOver;
    private Collider2D hurtBox;
    [SerializeField] private Material spriteLitDefault;
    [SerializeField] private Material dissolve;
    private float fade = 1f;
    private SpriteRenderer playerRenderer;
    private bool dead = false;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        hurtBox = GetComponent<Collider2D>();
        playerRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        hurtBox.enabled = true;
    }

    private void Update()
    {
        if (currentHealth != maxHealth.RuntimeValue)
            currentHealth = maxHealth.RuntimeValue;
        if (currentHealth <= 0)
        {
            dead = true;
            gameOver.Raise();
            hurtBox.enabled = false;
        }

        if (dead)
        {
            playerRenderer.material = dissolve;
            fade -= Time.deltaTime/2;
            if (fade <= 0f)
            {
                fade = 0f;
            }
            dissolve.SetFloat("_Fade", fade);
        }
    }

    public override void Damage(float damage)
    {
        if (!player.invulnerable)
        {
            base.Damage(damage);
            maxHealth.RuntimeValue = currentHealth;
            healthSignal.Raise();        
        }
    }
}
