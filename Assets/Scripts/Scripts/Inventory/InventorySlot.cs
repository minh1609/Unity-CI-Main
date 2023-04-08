using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI Fields")]
    [SerializeField] private TextMeshProUGUI itemNumberText;
    [SerializeField] private Image itemImage;
    [SerializeField] public Button button;

    [Header("Variables from the item")]
    public Item thisItem;
    public InventoryManager thisManager;

    public void Setup(Item newItem, InventoryManager newManager)
    {
        thisItem = newItem;
        thisManager = newManager;
        if (thisItem != null)
        {
            itemImage.sprite = thisItem.itemSprite;
            itemNumberText.text = thisItem.numberHeld + "";
        }
    }

    private void Update()
    {
        itemNumberText.text = thisItem.numberHeld + "";
    }



    public void ClickedOn()
    {
        FindObjectOfType<AudioManager>().Play("ButtonPress");
        if (thisItem != null && thisManager.tempItem == null)
        {
            thisManager.SetupDescriptionAndButton(thisItem.itemDescription, thisItem.itemName, thisItem.usable, thisItem.combineable, thisItem);
        }

        if (thisManager.tempItem != null)
        {
            if (thisItem.combinations != null && thisItem.numberHeld > 0)
            {
                if (thisItem.combinations.ContainsKey(thisManager.tempItem))
                {
                    thisItem.numberHeld--;
                    thisManager.tempItem.numberHeld--;
                    Item comboItem = (Item)thisItem.combinations[thisManager.tempItem];
                    if (!thisManager.playerInventory.items.Contains(comboItem))
                    {
                        thisManager.MakeCombinedItem(comboItem);
                    }
                    else
                    {
                        thisManager.searchForItem(comboItem);
                    }
                    thisManager.SetupDescriptionAndButton(comboItem.itemDescription, comboItem.itemName, comboItem.usable, comboItem.combineable, comboItem);
                    thisManager.playerInventory.AddItem(comboItem, 1);
                    FindObjectOfType<AudioManager>().Play("combined");

                    if (thisItem.numberHeld <= 0)
                    {
                        GameObject thisSlot = thisManager.searchForItem(thisItem);
                        GameObject.Destroy(thisSlot);
                    }

                    if (thisManager.tempItem.numberHeld <= 0)
                    {
                        GameObject thisSlot = thisManager.searchForItem(thisManager.tempItem);
                        GameObject.Destroy(thisSlot);
                    }
                }
            }
        }
        thisManager.tempItem = null;
        thisManager.combineButton.image.sprite = thisManager.combineDefault;
    }
}
