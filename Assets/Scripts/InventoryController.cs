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

    // Update is called once per frame
    void Update()
    {
        
    }
}
