using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerItemCollector : MonoBehaviour
{

    private InventoryController inventoryController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
        if (inventoryController == null)
        {
            Debug.LogError("No InventoryController found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //did player collide with item
        if (collision.CompareTag("Item"))
        {
            //get item script
            Item item = collision.GetComponent<Item>();
            if(item != null) //check if we got item script
            {
                //add item to inventyory
                bool itemAdded = inventoryController.AddItem(collision.gameObject);

                if (itemAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
