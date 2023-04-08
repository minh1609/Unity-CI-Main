using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public Sprite itemSprite;
    [TextArea(4, 10)]
    public string itemDescription;
    public bool isKey;
    public bool isCoin;

    public string itemName;
    public int numberHeld;
    public bool usable;
    public bool combineable;
    public bool unique;
    public UnityEvent thisEvent;

    public Item[] combineList;
    public Item[] combineProduct;

    public Hashtable combinations = new Hashtable();

    private void OnEnable()
    {
        if (combineList != null && combineProduct != null)
        {
            if (combineList.Length == combineProduct.Length && combineList.Length > 0 && combineProduct.Length > 0)
            {
                for (int i = 0; i < combineList.Length; i++)
                {
                    combinations.Add(combineList[i], combineProduct[i]);
                }
            }
        }
    }

    public void addAmount(int amount)
    {
        numberHeld += amount;
    }


    public void Use()
    {
        if (numberHeld>0)
            thisEvent.Invoke();
    }

    public void DecreaseAmount(int amount)
    {
        numberHeld -= amount;
        if (numberHeld < 0)
        {
            numberHeld = 0;
        }
    }
}
