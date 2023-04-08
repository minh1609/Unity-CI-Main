using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public Item currentItem;
    public List<Item> items = new List<Item>();
    public int numberOfKeys;
    public int coins;
    public float maxMagic;
    public float currentMagic;


    public void ReduceMagic(float magicCost)
    {
        currentMagic -= magicCost;
    }

    public bool CheckForItem(Item item)
    {
        if (items.Contains(item))
        {
            return true;
        }
        else
            return false;
    }

    public void AddItem(Item itemToAdd, int amount)
    {
        if (itemToAdd.isKey)
        {
            numberOfKeys+=amount;
            if (!items.Contains(itemToAdd))
            {
                itemToAdd.numberHeld += amount;
                items.Add(itemToAdd);
            }
        }
        if (itemToAdd.isCoin)
            coins += amount;
        else
        {
            if (!items.Contains(itemToAdd))
            {
                itemToAdd.numberHeld += amount;
                items.Add(itemToAdd);
            }
            else
                items[items.FindIndex(a => a.itemName == itemToAdd.itemName)].numberHeld += amount;
        }
    }

    public Item GetItem(string itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemName == itemName)
            {
                return items[i];
            }
        }
        return null;
    }

    public void RemoveItem(Item itemToRemove, int amount)
    {
        if (items.Contains(itemToRemove))
        {
            itemToRemove.numberHeld -= amount;
            if (itemToRemove.numberHeld <= 0)
            {
                itemToRemove.numberHeld = 0;
                items.Remove(itemToRemove);
            }
        }
    }

    public void spendCoin(int amount)
    {
        coins -= amount;
    }
}
