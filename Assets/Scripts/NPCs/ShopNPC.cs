using UnityEngine;
using System.Collections.Generic;
using Data; // Needed to access your ItemDataSO ScriptableObjects

public class ShopNPC : MonoBehaviour, IInteractable
{
    public string shopID = "starter_shop"; // Unique identifier for the shop
    public string shopkeeperName = "Bob"; // Name of the shopkeeper

    public List<ShopStockItem> shopStock = new(); // List of items available in the shop

    private bool isInitialized = false; // Flag to check if the shop has been initialized

    [System.Serializable]
    public class ShopStockItem
    {
        public ItemDataSO item; // ScriptableObject reference instead of itemID
        public int quantity; // Quantity of the item in stock (0 or negative = infinite stock)
        public bool infiniteStock; // checkbox in inspector
    }

    void Start()
    {
        InitializeShop();
    }

    private void InitializeShop()
    {
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;
    }

    public bool CanInteract()
    {
        return true; // The player can always interact with the shop
    }

    public void Interact()
    {
        if (ShopController.Instance == null)
        {
            return;
        }

        if (ShopController.Instance.shopPanel.activeSelf)   // is panel visible right now?
        {
            ShopController.Instance.CloseShop();
        }
        else
        {
            ShopController.Instance.OpenShop(this);
        }
    }

    public List<ShopStockItem> GetShopStock()
    {
        return shopStock;
    }

    public void SetStock(List<ShopStockItem> stock)
    {
        shopStock = stock;
    }

    //changed slightly to use ScriptableObject items instead of itemID !!
    public void AddToStock(ItemDataSO item, int quantity)
    {
        ShopStockItem existingItem = shopStock.Find(i => i.item == item);

        if (existingItem != null)
        {
            // If stock is infinite, we don't modify quantity
            if (existingItem.quantity > 0)
            {
                existingItem.quantity += quantity;
            }
        }
        else
        {
            shopStock.Add(new ShopStockItem { item = item, quantity = quantity });
        }
    }

    //supports infinite stock
    public bool RemoveFromStock(ItemDataSO item, int quantity)
    {
        ShopStockItem existingItem = shopStock.Find(i => i.item == item);

        if (existingItem != null)
        {
            // If quantity <= 0 we treat it as infinite stock
            if (existingItem.quantity <= 0)
            {
                return true;
            }

            if (existingItem.quantity >= quantity)
            {
                existingItem.quantity -= quantity;

                if (existingItem.quantity == 0)
                {
                    shopStock.Remove(existingItem);
                }

                return true;
            }
        }

        return false; // Not enough stock to remove
    }
}