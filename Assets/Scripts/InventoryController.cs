using Unity.VisualScripting;
using UnityEngine;


//this script should go on the gamecontroller object!!!

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPage;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;  //array of all items

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < slotCount; i++)
        {
            //getting the Slot game object to populate with items
            InventorySlot slot = Instantiate(slotPrefab, inventoryPage.transform).GetComponent<InventorySlot>();

            if(i < itemPrefabs.Length)
            {
                //put the item in the slot
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                //item always centred
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
            }
        }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        //for every slot inside inventory page (inventory)   
        foreach(Transform slotTransform in inventoryPage.transform)
        {
            InventorySlot slot = slotTransform.GetComponent<InventorySlot>();      
            if (slot != null && slot.currentItem == null)     //do we have slot and is there no item in the slot?
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
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
}
