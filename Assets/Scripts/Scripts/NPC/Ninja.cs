using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ninja : Sign
{
    [SerializeField] private ChoiceManager choiceManager;
    private string charName;
    private Animator anim;
    private bool lastDialogue;
    [Header("Inventory Information")]
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private GameObject coinSignalPrefab;
    [SerializeField] private Item ninjaBookDash;

    [Header("Unactive if book has sold")]
    [SerializeField] private BoolValue boughtBook;

    private Text nameText;

    private void Start()
    {
        if (boughtBook.RunTimeValue)
        {
            this.gameObject.SetActive(false);
        }
        anim = GetComponent<Animator>();
        dialogueBox = GameObject.FindGameObjectWithTag("Player UI").transform.Find("Dialogue Box").gameObject;


        charName = "Ninja";
        string[] openingText = { "You there, Young man--!\n This is amazing...",
                                 "Truly one in a million! \n You have the skeletal structure of a ninja genius!",
                                 "Do you want to learn the secret technique of ninja movement?\n I can teach this to you for, say about 25 coins."};

        string[] offerAccepted = { "You will become the greatest warrior of this era!" };
        string[] offerDeclined = { "Aww... what a wasted gift." };
        string[] insufficientMoney = { "My friend, this is not a charity. You have no money." };

        dialogueSets[0].dialogue = new Dialogues(openingText);
        dialogueSets[1].dialogue = new Dialogues(offerAccepted);
        dialogueSets[2].dialogue = new Dialogues(offerDeclined);
        dialogueSets[3].dialogue = new Dialogues(insufficientMoney);

        foreach (DialogueTrigger dialogue in dialogueSets)
        {
            dialogue.CharacterName = charName;
        }

        lastDialogue = false;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if (dialogueSets.Length > 0 && dialogueSets != null)
            {
                if (!dialogueSets[0].endOfDialogue && choiceManager.selectedChoice == 0)
                {
                    stopOtherDialogues();
                    dialogueSets[0].toggleDialogue();                    
                }

                if (dialogueSets[0].endOfDialogue && choiceManager.selectedChoice == 0)
                {
                    PrepareFirstChoices();
                    choiceManager.makeChoicees();
                }

                else if (dialogueSets[0].endOfDialogue && choiceManager.selectedChoice == 1)
                {
                    stopOtherDialogues();
                    if (playerInventory.coins >= 25 && !lastDialogue)
                    {
                        dialogueSets[1].toggleDialogue();
                        playerInventory.spendCoin(25);
                        playerInventory.AddItem(ninjaBookDash, 1);

                        anim.SetTrigger("Exit");

                        StartCoroutine(ninjaExit());
                        dialogueSets[1].resetIndex();
                        lastDialogue = true;
                    }
                    else if (playerInventory.coins < 25)
                    {
                        dialogueSets[3].toggleDialogue();
                        dialogueSets[0].endOfDialogue = false;
                        choiceManager.resetChoices();
                    }
                }
                else if (dialogueSets[0].endOfDialogue && choiceManager.selectedChoice == 2)
                {
                    stopOtherDialogues();
                    dialogueSets[2].toggleDialogue();
                    dialogueSets[0].endOfDialogue = false;
                    choiceManager.resetChoices();
                }
            }
        }
        if (!playerInRange)
        {
            foreach (DialogueTrigger dialogue in dialogueSets)
            {
                dialogue.index = 0;
            }
        }
        if (GameObject.FindGameObjectWithTag("Player UI").transform.Find("Dialogue Box").gameObject.activeInHierarchy == false)
        {
            resetAllDialogIndexes();
        }
    }
    private void PrepareFirstChoices()
    {
        string[] firstSetChoices = { "Heck yeah I do!", "That sounds like a scam." };
        choiceManager.choices = firstSetChoices;
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

    private void stopOtherDialogues()
    {
        for (int i = 0; i < dialogueSets.Length; i++)
        {
            if (i != choiceManager.selectedChoice)
            {
                dialogueSets[i].stopDialogueCoroutines();
            }
        }
    }

    private void resetAllDialogIndexes()
    {
        foreach (DialogueTrigger dialogue in dialogueSets)
        {
            dialogue.index = 0;
        }
    }

    private IEnumerator ninjaExit()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(2f);
        boughtBook.RunTimeValue = true;
        this.gameObject.SetActive(false);
    }
}
