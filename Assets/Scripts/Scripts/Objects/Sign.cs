using UnityEngine;

public class Sign : Interactable
{
    [SerializeField] public DialogueTrigger[] dialogueSets;
    [HideInInspector] public GameObject dialogueBox;
    [SerializeField] public ChoiceManager choiceManager;

    public virtual void Start()
    {
        dialogueBox = GameObject.FindGameObjectWithTag("Player UI").transform.Find("Dialogue Box").gameObject;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if (dialogueSets.Length > 0 && dialogueSets != null)
            {
                dialogueSets[0].toggleDialogue();
            }
        }
        else if (!playerInRange)
        {
            foreach (DialogueTrigger dialogue in dialogueSets)
            {
                dialogue.index = 0;
            }
        }
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

    public void resetAllDialogIndexes()
    {
        foreach (DialogueTrigger dialogue in dialogueSets)
        {
            dialogue.index = 0;
            dialogue.endOfDialogue = false;
        }
    }

    public void stopOtherDialogues()
    {
        for (int i = 0; i < dialogueSets.Length; i++)
        {
            if (i != choiceManager.selectedChoice)
            {
                dialogueSets[i].stopDialogueCoroutines();
            }
        }
    }

}
