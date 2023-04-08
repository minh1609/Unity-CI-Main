using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Information")]
    public Inventory playerInventory;
    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryScrollView;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Button useButton;
    [SerializeField] public Button combineButton;
    [SerializeField] public Sprite combineDefault;
    [SerializeField] private Sprite combineToggle;
    [SerializeField] private Button pageUpBtn;
    [SerializeField] private Button pageDownBtn;
    [SerializeField] private Button exit;
    public Item currentItem;
    public Item tempItem;
    public GameObject inventoryPanel;
    private int maxItemsDisplayed = 8;
    private int page = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            toggleInventory();
            clearCombine();
            clearText();
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            inventoryPanel.SetActive(false);
            clearCombine();
            clearText();
        }
        pageUpBtn.interactable = page*maxItemsDisplayed < playerInventory.items.Count ? true : false;

        pageDownBtn.interactable = page > 1 ? true : false;
    }

    public void toggleInventory()
    {
        FindObjectOfType<AudioManager>().Play("ButtonPress");
        clearCombine();
        clearText();
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        foreach (Transform children in inventoryScrollView.transform)
            GameObject.Destroy(children.gameObject);
        clearEmpty();
        MakeInventorySlot();
        exit.Select();
    }

    public void SetTextAndButton(string description, bool useBtn, bool combineBtn)
    {
        descriptionText.text = description;
        if (useBtn)
        {
            useButton.interactable = true;
        }
        else
        {
            useButton.interactable = false;
        }

        if (combineBtn)
        {
            combineButton.interactable = true;
        }
        else
        {
            combineButton.interactable = false;
        }
    }

    void MakeInventorySlot()
    {
        if (playerInventory != null)
        {

            for (int i = maxItemsDisplayed * page - maxItemsDisplayed; i < playerInventory.items.Count && i < page* maxItemsDisplayed; i++)
            {
                if (playerInventory.items[i].numberHeld > 0 && playerInventory.items[i] != null)
                {
                    GameObject temp = Instantiate(blankInventorySlot, inventoryScrollView.transform.position, Quaternion.identity);
                    temp.transform.SetParent(inventoryScrollView.transform);
                    temp.transform.localScale = new Vector3(1, 1, 1);
                    InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                    if (newSlot != null)
                    {
                        newSlot.Setup(playerInventory.items[i], this);
                    }
                }
            }
        }
    }

    private void clearEmpty()
    {
        for (int i = 0; i < playerInventory.items.Count; i++)
        {
            if (playerInventory.items[i].numberHeld <= 0)
            {
                playerInventory.items.Remove(playerInventory.items[i]);
            }
        }
    }

    public void MakeCombinedItem(Item combinedItem)
    {
        if (playerInventory != null)
        {
                GameObject temp = Instantiate(blankInventorySlot, inventoryScrollView.transform.position, Quaternion.identity);
                temp.transform.SetParent(inventoryScrollView.transform);
                temp.transform.localScale = new Vector3(1, 1, 1);
                InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                if (newSlot != null)
                {
                    newSlot.Setup(combinedItem, this);
                    Selectable btn = temp.GetComponent<Selectable>();
                    EventSystem.current.SetSelectedGameObject(btn.gameObject);
                }
         }
    }

    public GameObject searchForItem(Item item)
    {
        foreach (Transform children in inventoryScrollView.transform)
        {
            if (children.GetComponent<InventorySlot>().thisItem == item)
            {
                Selectable btn = children.GetComponent<Selectable>();
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
                return children.gameObject;
            }
        }
        return null;
    }

    public void setSlots()
    {
        foreach (Transform children in inventoryScrollView.transform)
            GameObject.Destroy(children.gameObject);
        MakeInventorySlot();
    }

    public void pageUp()
    {
        if (page + maxItemsDisplayed <= playerInventory.items.Count)
        {
            page++;
            FindObjectOfType<AudioManager>().Play("ButtonPress");
        }
        foreach (Transform children in inventoryScrollView.transform)
            GameObject.Destroy(children.gameObject);
            MakeInventorySlot();
        if (page * maxItemsDisplayed < playerInventory.items.Count)
            pageUpBtn.Select();
        else
            pageDownBtn.Select();
    }

    public void pageDown()
    {
        if (page > 1)
        {
            page--;
            FindObjectOfType<AudioManager>().Play("ButtonPress");
        }
        foreach (Transform children in inventoryScrollView.transform)
        GameObject.Destroy(children.gameObject);
        MakeInventorySlot();

        if (page > 1)
            pageDownBtn.Select();
        else
            pageUpBtn.Select();
    }   

    // Start is called before the first frame update
    void Start()
    {
        inventoryPanel.SetActive(false);
        SetTextAndButton("", false, false);
    }

    public void SetupDescriptionAndButton(string newDescriptionString, string itemName, bool useBtn, bool combineBtn, Item newtItem)
    {
        currentItem = newtItem;
        descriptionText.text = newDescriptionString;
        this.itemName.text = itemName;
        if (useBtn)
            useButton.interactable = true;
        else
            useButton.interactable = false;

        if (combineBtn)
            combineButton.interactable = true;
        else
            combineButton.interactable = false;
    }

    public void UseButtonPressed()
    {
        if (currentItem != null)
        {
            FindObjectOfType<AudioManager>().Play("ButtonPress");
            currentItem.Use();
            setSlots();
        }
        clearCombine();
    }

    public void CombineButtonPressed()
    {
        if (tempItem == null)
        {
            FindObjectOfType<AudioManager>().Play("ButtonPress");
            combineButton.image.sprite = combineToggle;
            tempItem = currentItem;
        }

        else
            clearCombine();
    }

    private void clearCombine()
    {
        combineButton.image.sprite = combineDefault;
        tempItem = null;
    }

    private void clearText()
    {
        descriptionText.text = "";
        itemName.text = "";
        useButton.interactable = false;
        combineButton.interactable = false;
    }
}
