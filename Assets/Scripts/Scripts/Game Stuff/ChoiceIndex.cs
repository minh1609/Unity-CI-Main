using UnityEngine;

public class ChoiceIndex : MonoBehaviour
{
    public int choiceIndex;
    public ChoiceManager choiceManager;

    public void returnIndex()
    {
        choiceManager.selectedChoice = choiceIndex;
        choiceManager.closeChoiceBox();
    }
}
