using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNameComparer : IComparer<Item>
{
    public int Compare(Item x, Item y)
    {
        return x.itemName.CompareTo(y.itemName);
    }
}
