using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [HideInInspector] public GameObject dialogueBox;
    [HideInInspector] public Text dialogueText;
    [HideInInspector] public Text nameText;
    public string CharacterName;
    [HideInInspector] public int index = 0;
    [HideInInspector] public bool endOfDialogue;
    public Dialogues dialogue;

    // Start is called before the first frame update
    void Awake()
    {
        index = 0;
        endOfDialogue = false;
        dialogueBox = GameObject.FindGameObjectWithTag("Player UI").transform.Find("Dialogue Box").gameObject;
        if (dialogueBox != null)
        {
            dialogueText = dialogueBox.transform.Find("Dialogue Text").GetComponent<Text>();
            nameText = dialogueBox.transform.Find("Name Text").GetComponent<Text>();
        }
    }

    public void toggleDialogue()
    {
        if (dialogueBox.activeInHierarchy && index >= dialogue.sentences.Length)
        {
            dialogueBox.SetActive(false);
            resetIndex();
        }
        else if (index >= dialogue.sentences.Length)
        {
            resetIndex();
        }
        else
        {
            endOfDialogue = false;
            dialogueBox.SetActive(true);
            if (CharacterName != null && CharacterName != "")
            {
                nameText.text = CharacterName + ":";
            }
            stopDialogueCoroutines();
            StartCoroutine(TypeSentence(dialogue.sentences[index]));
            index++;
            if (index == dialogue.sentences.Length)
            {
                endOfDialogue = true;
            }
        }
    }
    public void stopDialogueCoroutines()
    {
        StopAllCoroutines();
    }

    public void resetIndex()
    {
        index = 0;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        if (index < dialogue.sentences.Length)
        {
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(0.01f);
                if (!FindObjectOfType<AudioManager>().isPlaying("Add Letter"))
                {
                    FindObjectOfType<AudioManager>().PlayRandomPitch("Add Letter", 0.8f, 1.2f);
                }
            }
        }
        else if (index == dialogue.sentences.Length)
        {
            dialogueText.text = sentence;
        }
    }

}
