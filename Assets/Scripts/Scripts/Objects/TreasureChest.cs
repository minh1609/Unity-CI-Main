using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreasureChest : InteractableOnce
{
    public GameObject coinSignal_Prefab;

    [Header("Contents")]
    public BoolValue storedOpen;
    public Item contents;
    public int amount;
    public Inventory playerInventory;
    public string description;

    [Header("Signals and Dialogue")]
    public Signal raiseItem;
    public GameObject dialogueBox;
    public Text dialogueText;

    [Header("Animation")]
    private Animator animator;

    void Start()
    {
        dialogueBox = GameObject.FindGameObjectWithTag("Player UI").transform.Find("Dialogue Box").gameObject;
        if (dialogueBox != null)
        {
            dialogueText = dialogueBox.transform.Find("Dialogue Text").GetComponent<Text>();
        }
        animator = GetComponent<Animator>();
        triggered = storedOpen.RunTimeValue;
        if (triggered)
        {
            animator.Play("treasureOpen", 0, 1.0f);
            animator.SetBool("Opened", true);
        }
        
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!triggered)
                OpenChest();
            else
                ChestIsOpened();
        }
    }

    public void OpenChest()
    {
        FindObjectOfType<AudioManager>().Play("chest-opening");
        dialogueBox.SetActive(true);
        dialogueText.text = description;
        playerInventory.currentItem = contents;
        if (!contents.isCoin)
        {
            playerInventory.AddItem(contents, amount);
        }
        else
        {
            playerInventory.AddItem(contents, amount);
            GameObject temp = GameObject.Instantiate(coinSignal_Prefab, this.transform.position, Quaternion.identity);
        }
        raiseItem.Raise();
        animator.SetBool("Opened", true);
        triggerItem();
        storedOpen.RunTimeValue = triggered;
    }

    public void ChestIsOpened()
    {
        dialogueBox.SetActive(false);
        playerInRange = false;
        raiseItem.Raise();
    }
}