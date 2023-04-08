using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public string[] choices;
    [SerializeField] private GameObject choiceBoxContainer;
    [SerializeField] private GameObject choicePrefab;
    public int selectedChoice;
    private bool firstChoice;

    private void Start()
    {
        firstChoice = true;
        if (choiceBoxContainer == null)
            choiceBoxContainer = GameObject.FindGameObjectWithTag("Player UI").transform.Find("Choice Box").gameObject;
    }
    public void makeChoicees()
    {
        choiceBoxContainer.SetActive(true);

        foreach (Transform children in choiceBoxContainer.transform)
            GameObject.Destroy(children.gameObject);

        int currentIndex = 1;
        foreach (string choiceText in choices)
        {

            GameObject temp = Instantiate(choicePrefab, choiceBoxContainer.transform.position, Quaternion.identity);
            temp.transform.SetParent(choiceBoxContainer.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
            TextMeshProUGUI text = temp.GetComponentInChildren<TextMeshProUGUI>();
            ChoiceIndex newChoice = temp.GetComponent<ChoiceIndex>();
            newChoice.choiceManager = this;
            if (newChoice != null)
            {
                newChoice.choiceIndex = currentIndex;
            }
            text.text = choiceText;
            Selectable btn = temp.GetComponent<Selectable>();
            if (firstChoice)
            {
                firstChoice = false;
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
            }
            currentIndex++;
        }
        firstChoice = true;
    }

    public void resetChoices()
    {
        selectedChoice = 0;
    }

    public void closeChoiceBox()
    {
        choiceBoxContainer.SetActive(false);
    }
}
