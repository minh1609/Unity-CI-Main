using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI Fields")]
    [SerializeField] private TextMeshProUGUI itemNumberText;
    [SerializeField] private Image itemImage;

    [Header("Variables from the item")]
    public InventoryItem thisItem;
    public InventoryManager thisManager;

    public void Setup(InventoryItem newItem, InventoryManager newManager)
    {
        thisItem = newItem;
        thisManager = newManager;
        if (thisItem != null)
        {
            itemImage.sprite = thisItem.itemImage;
            itemNumberText.text = thisItem.numberHeld + "";
        }
    }

    private void Update()
    {
        itemNumberText.text = thisItem.numberHeld + "";
    }



    public void ClickedOn()
    {
        if (thisItem != null)
        {
            thisManager.SetupDescriptionAndButton(thisItem.itemDescription, thisItem.itemName, thisItem.usable, thisItem);
        }
    }
}
