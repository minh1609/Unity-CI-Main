using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MerchantPhase
{
    initial, buy, sell, exit 
}
public class Merchant : Sign
{
    [SerializeField] private ChoiceManager choiceManager;
    private string charName;
    private Animator anim;
    private bool lastDialogue = false;
    [Header("Inventory Information")]
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private GameObject coinSignalPrefab;
    [SerializeField] private Item redPotion;
    [SerializeField] private Item greenPotion;
    [SerializeField] private MerchantPhase currentState;

    private Text nameText;
    // Start is called before the first frame update
    public override void Start()
    {
        currentState = MerchantPhase.initial;

        lastDialogue = false;
        anim = GetComponent<Animator>();
        dialogueBox = GameObject.FindGameObjectWithTag("Player UI").transform.Find("Dialogue Box").gameObject;


        charName = "Merchant";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if (dialogueSets.Length > 0 && dialogueSets != null)
            {
                if (!dialogueSets[0].endOfDialogue && currentState == MerchantPhase.initial)
                {
                    stopOtherDialogues();
                    dialogueSets[0].toggleDialogue();
                }

                if (dialogueSets[0].endOfDialogue && currentState == MerchantPhase.initial)
                {
                    PrepareFirstChoices();
                    choiceManager.makeChoicees();
                }

                //Buy
                if (dialogueSets[0].endOfDialogue && currentState == MerchantPhase.initial  && choiceManager.selectedChoice == 1)
                {
                    stopOtherDialogues();
                    dialogueSets[1].toggleDialogue();
                    PrepareSecondChoices();
                    choiceManager.makeChoicees();
                    currentState = MerchantPhase.buy;
                    choiceManager.resetChoices();

                }

                //Buy Red Potion
                if (dialogueSets[1].endOfDialogue && currentState == MerchantPhase.buy && choiceManager.selectedChoice == 1)
                {
                    if (playerInventory.coins >= 10)
                    {
                        stopOtherDialogues();
                        dialogueSets[3].toggleDialogue();
                        if (!lastDialogue)
                        {
                            playerInventory.spendCoin(10);
                            playerInventory.AddItem(redPotion, 1);
                            GameObject temp = GameObject.Instantiate(coinSignalPrefab, this.transform.position, Quaternion.identity);
                            lastDialogue = true;
                        }
                    }
                    else if (playerInventory.coins < 10)
                    {
                        stopOtherDialogues();
                        dialogueSets[5].toggleDialogue();
                        choiceManager.closeChoiceBox();
                    }
                }

                //Buy Green Potion
                if (dialogueSets[1].endOfDialogue && currentState == MerchantPhase.buy && choiceManager.selectedChoice == 2)
                {
                    if (playerInventory.coins >= 10)
                    {
                        stopOtherDialogues();
                        dialogueSets[3].toggleDialogue();
                        if (!lastDialogue)
                        {
                            playerInventory.spendCoin(10);
                            playerInventory.AddItem(greenPotion, 1);
                            GameObject temp = GameObject.Instantiate(coinSignalPrefab, this.transform.position, Quaternion.identity);
                            lastDialogue = true;
                        }
                    }
                    else if (playerInventory.coins < 10)
                    {
                        stopOtherDialogues();
                        dialogueSets[5].toggleDialogue();
                        choiceManager.closeChoiceBox();
                    }
                }

                //Sell
                else if (dialogueSets[0].endOfDialogue && currentState == MerchantPhase.initial && choiceManager.selectedChoice == 2)
                {
                    stopOtherDialogues();
                    dialogueSets[7].toggleDialogue();
                    PrepareThirdChoices();
                    choiceManager.makeChoicees();
                    currentState = MerchantPhase.sell;
                    choiceManager.resetChoices();
                }

                //Sell Red Potion
                if (dialogueSets[7].endOfDialogue && currentState == MerchantPhase.sell && choiceManager.selectedChoice == 1)
                {
                    stopOtherDialogues();
                    dialogueSets[3].toggleDialogue();
                    if (playerInventory.CheckForItem(redPotion) && redPotion.numberHeld > 0 && !lastDialogue)
                    {
                        playerInventory.coins += 10;
                        playerInventory.RemoveItem(redPotion, 1);
                        GameObject temp = GameObject.Instantiate(coinSignalPrefab, this.transform.position, Quaternion.identity);
                        lastDialogue = true;
                    }
                    else
                    {
                        stopOtherDialogues();
                        dialogueSets[6].toggleDialogue();
                        choiceManager.closeChoiceBox();
                    }
                }

                //Sell Green Potion
                if (dialogueSets[7].endOfDialogue && currentState == MerchantPhase.sell && choiceManager.selectedChoice == 2)
                {
                    stopOtherDialogues();
                    dialogueSets[3].toggleDialogue();
                    if (playerInventory.CheckForItem(greenPotion) && greenPotion.numberHeld > 0)
                    {
                        playerInventory.coins += 10;
                        playerInventory.RemoveItem(greenPotion, 1);
                        GameObject temp = GameObject.Instantiate(coinSignalPrefab, this.transform.position, Quaternion.identity);
                        lastDialogue = true;
                    }
                    else
                    {
                        stopOtherDialogues();
                        dialogueSets[6].toggleDialogue();
                        choiceManager.closeChoiceBox();
                    }
                }

                //Exit
                if (dialogueSets[0].endOfDialogue && choiceManager.selectedChoice == 3)
                {
                    currentState = MerchantPhase.exit;
                    choiceManager.closeChoiceBox();
                }

                if (currentState == MerchantPhase.exit)
                {
                    stopOtherDialogues();
                    dialogueSets[2].toggleDialogue();
                }
            }
        }
        if (!playerInRange)
        {
            resetAllDialogIndexes();
            choiceManager.closeChoiceBox();
        }
        if (GameObject.FindGameObjectWithTag("Player UI").transform.Find("Dialogue Box").gameObject.activeInHierarchy == false || !playerInRange)
        {
            resetAllDialogIndexes();
            choiceManager.resetChoices();
            lastDialogue = false;
            currentState = MerchantPhase.initial;
            choiceManager.closeChoiceBox();
        }
    }

    private void PrepareFirstChoices()
    {
        string[] firstSetChoices = { "Buy", "Sell", "Exit" };
        choiceManager.choices = firstSetChoices;
    }

    private void PrepareSecondChoices()
    {
        string[] secondSetChoices = { "Red Potion - 10 gold", "Green Potion - 10 gold", "Exit" };
        choiceManager.choices = secondSetChoices;
    }

    private void PrepareThirdChoices()
    {
        string[] thirdSetChoices = { "Sell Red Potion - 5 gold", "Sell Green Potion - 5 gold", "Exit" };
        choiceManager.choices = thirdSetChoices;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            contextClue.Raise();
            playerInRange = false;
            dialogueBox.SetActive(false);
        }
    }
}
