using Data; // Needed for ItemDataSO
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

//this script should go on the GameController object people! !! !!!
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

    // Returns the full list of inventory items (for ShopController)
    public List<InventoryItem> GetInventoryItems()
    {
        return inventoryList;
    }

    // Adds an item to the inventory (SO-based) and stacks quantity if already exists
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

    public bool RemoveItem(ItemDataSO itemSO, int quantity = 1)
    {
        if (itemSO == null) return false;

        InventoryItem existing = inventoryList.Find(x => x.item == itemSO);
        if (existing == null) return false;

        if (existing.quantity <= quantity)
        {
            // Remove completely
            inventoryList.Remove(existing);
        }
        else
        {
            existing.quantity -= quantity;
        }

        return true;
    }

    // Refreshes the inventory UI (instantiates slots and item icons)
    // called when you open the shop or update inventory display
    public void RefreshInventoryUI()
    {
        // Keep all existing slots (we have a fixed number)
        InventorySlot[] slots = inventoryPage.GetComponentsInChildren<InventorySlot>();

        // Clear existing slot items but keep the slot objects
        foreach (var slot in slots)
        {
            if (slot.currentItem != null)
            {
                Destroy(slot.currentItem);
                slot.currentItem = null;
            }
        }

        // Fill slots with items from inventoryList
        for (int i = 0; i < inventoryList.Count && i < slots.Length; i++)
        {
            var invItem = inventoryList[i];
            if (invItem.item == null) continue;

            if (invItem.item.prefab != null)
            {
                // Use prefab if assigned
                GameObject itemObj = Instantiate(invItem.item.prefab, slots[i].transform);
                itemObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                Item itemComp = itemObj.GetComponent<Item>();
                if (itemComp != null)
                {
                    itemComp.quantity = invItem.quantity;
                    itemComp.itemDataSO = invItem.item;
                    itemComp.UpdateQuantityDisplay();
                }

                slots[i].currentItem = itemObj;
            }
            else
            {
                // Fallback: display item name + quantity if no prefab
                TMP_Text text = slots[i].GetComponentInChildren<TMP_Text>();
                if (text != null)
                {
                    text.text = $"{invItem.item.itemName} x{invItem.quantity}";
                }
            }
        }

        // Remaining slots stay empty
    }
}