using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using Data; // Needed for ItemDataSO

//this script should go on the GameController object!!!
public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPage;    // parent object for inventory slots
    public GameObject slotPrefab;       // prefab for empty inventory slot
    public int slotCount;               // total number of slots
    public GameObject[] itemPrefabs;    // array of all item prefabs (optional for legacy)

    // Store inventory as ScriptableObject + quantity
    [System.Serializable]
    public class InventoryItem
    {
        public ItemDataSO item;         // ScriptableObject reference
        public int quantity;            // how many the player has
    }

    public List<InventoryItem> inventoryList = new List<InventoryItem>();

    public static InventoryController Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            //getting the Slot game object to populate with items
            InventorySlot slot = Instantiate(slotPrefab, inventoryPage.transform).GetComponent<InventorySlot>();

            if (i < itemPrefabs.Length)
            {
                //put the item in the slot
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                //item always centred
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;

                // Add item to inventoryList (SO-based)
                Item itemComp = item.GetComponent<Item>();
                if (itemComp != null)
                {
                    AddItem(itemComp.itemDataSO, itemComp.quantity);
                }
            }
        }
    }

    /// Returns the full list of inventory items (for ShopController)
    public List<InventoryItem> GetInventoryItems()
    {
        return inventoryList;
    }

    /// <summary>
    /// Adds an item to the inventory (SO-based) and stacks quantity if already exists
    /// </summary>
    public bool AddItem(ItemDataSO itemSO, int quantity = 1)
    {
        if (itemSO == null) return false;

        //check if item is already in inventory and stack it
        InventoryItem existing = inventoryList.Find(x => x.item == itemSO);
        if (existing != null)
        {
            existing.quantity += quantity;
            return true;
        }

        // Add new item to inventory
        inventoryList.Add(new InventoryItem { item = itemSO, quantity = quantity });
        return true;
    }

    /// <summary>
    /// Old prefab-based AddItem for legacy support (optional)
    /// </summary>
    public bool AddItem(GameObject itemPrefab)
    {
        Item itemToAdd = itemPrefab.GetComponent<Item>();
        if (itemToAdd == null)
        {
            return false;   //we don't want to add non-item objects to the inventory
        }

        //check if item is already in inventory and if so, add to stack
        foreach (Transform slotTransform in inventoryPage.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if (slotItem != null && slotItem.ID == itemToAdd.ID)
                {
                    //if same item, stack
                    slotItem.AddToStack();

                    // also update SO inventoryList
                    AddItem(slotItem.itemDataSO, 1);

                    return true;
                }
            }
        }

        //for every slot inside inventory page (inventory)   
        foreach (Transform slotTransform in inventoryPage.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;

                // also add to SO inventoryList
                Item newItemComp = newItem.GetComponent<Item>();
                if (newItemComp != null)
                {
                    AddItem(newItemComp.itemDataSO, newItemComp.quantity);
                }

                return true;
            }
        }
        Debug.Log("Inventory is full!");
        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Refreshes the inventory UI (instantiates slots and item icons)
    /// Call this when you open the shop or update inventory display
    /// </summary>
    public void RefreshInventoryUI()
    {
        // Clear existing slots
        foreach (Transform child in inventoryPage.transform)
            Destroy(child.gameObject);

        foreach (var invItem in inventoryList)
        {
            if (invItem.item == null) continue;

            // Instantiate slot UI
            InventorySlot slot = Instantiate(slotPrefab, inventoryPage.transform).GetComponent<InventorySlot>();

            // Instantiate the actual item prefab (with Item component, icon, TMP text, etc.)
            GameObject itemPrefab = invItem.item.prefab; // <-- you need a reference in your SO to prefab
            if (itemPrefab == null) continue;

            GameObject itemObj = Instantiate(itemPrefab, slot.transform);
            itemObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            // Update Item component quantity
            Item itemComp = itemObj.GetComponent<Item>();
            if (itemComp != null)
            {
                itemComp.quantity = invItem.quantity;
                itemComp.itemDataSO = invItem.item;
                itemComp.UpdateQuantityDisplay();
            }

            slot.currentItem = itemObj;
        }
    }
}