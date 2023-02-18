using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Information")]
    public PlayerInventory playerInventory;
    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryScrollView;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject useButton;
    public InventoryItem currentItem;

    public void SetTextAndButton(string description, bool buttonActive)
    {
        descriptionText.text = description;
        if (buttonActive)
        {
            useButton.SetActive(true);
        }
        else
        {
            useButton.SetActive(false);
        }
    }

    void MakeInventorySlot()
    {
        if (playerInventory != null)
        {
            for (int i = 0; i < playerInventory.myInventory.Count; i++)
            {
                GameObject temp = Instantiate(blankInventorySlot, inventoryScrollView.transform.position, Quaternion.identity);
                temp.transform.SetParent(inventoryScrollView.transform);
                temp.transform.localScale = new Vector3(1, 1, 1);
                InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                if (newSlot != null)
                {
                    newSlot.Setup(playerInventory.myInventory[i], this);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MakeInventorySlot();
        SetTextAndButton("", false);
    }

    public void SetupDescriptionAndButton(string newDescriptionString, string itemName, bool isButtonUsable, InventoryItem newtItem)
    {
        currentItem = newtItem;
        descriptionText.text = itemName + Environment.NewLine + newDescriptionString;
        useButton.SetActive(isButtonUsable);
    }

    public void UseButtonPressed()
    {
        if (currentItem != null)
        {
            currentItem.Use();
        }
    }
}
