using UnityEngine;
using UnityEngine.EventSystems;
using Data;

public class ShopItemHandler : MonoBehaviour, IPointerClickHandler
{
    public ItemDataSO itemData;
    private bool isShopItem;
    public ShopSlot originalInventorySlot;

    /// <summary>
    /// Initialize the handler with slot and type (shop vs player inventory)
    /// </summary>
    public void Initialise(bool shopItem, ShopSlot slot = null)
    {
        isShopItem = shopItem;
        originalInventorySlot = slot;

        // assign itemData from slot if not already
        itemData = slot?.itemData;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
        {
            return;
        }

        if (isShopItem)
        {
            BuyItem();
        }
        else
        {
            SellItem();
        }
    }

    private void BuyItem()
    {
        if (originalInventorySlot == null || itemData == null)
        {
            Debug.LogWarning("BuyItem failed: slot or itemData is null");
            return;
        }

        if (CurrencyController.Instance == null)
        {
            Debug.LogError("CurrencyController missing!");
            return;
        }

        int price = originalInventorySlot.GetCurrentPrice();

        if (CurrencyController.Instance.GetMoney() < price)
        {
            Debug.Log("Not enough money to buy item.");
            return;
        }

        if (InventoryController.Instance == null)
        {
            Debug.LogError("InventoryController missing!");
            return;
        }

        // Add to player inventory
        if (InventoryController.Instance.AddItem(itemData, 1))
        {
            CurrencyController.Instance.SpendMoney(price);

            // Reduce shop stock if applicable
            ShopNPC shop = ShopController.Instance?.CurrentShop;
            if (shop != null)
            {
                if (!originalInventorySlot.IsInfiniteStock())
                {
                    shop.RemoveFromStock(itemData, 1);
                }
            }

            ShopController.Instance?.RefreshShopDisplay();
            ShopController.Instance?.RefreshPlayerInventoryDisplay();
        }
        else
        {
            Debug.Log("Inventory full or error adding item.");
        }
    }

    private void SellItem()
    {
        if (originalInventorySlot == null || itemData == null)
        {
            Debug.LogWarning("SellItem failed: slot or itemData is null");
            return;
        }

        if (InventoryController.Instance == null)
        {
            Debug.LogError("InventoryController missing!");
            return;
        }

        int price = originalInventorySlot.GetCurrentPrice();

        if (!InventoryController.Instance.RemoveItem(itemData, 1))
        {
            Debug.LogWarning("Failed to remove item from inventory.");
            return;
        }

        if (CurrencyController.Instance != null)
        {
            CurrencyController.Instance.AddMoney(price);
        }

        // Add back to shop stock
        ShopNPC shop = ShopController.Instance?.CurrentShop;
        if (shop != null)
        {
            shop.AddToStock(itemData, 1);
        }

        ShopController.Instance?.RefreshShopDisplay();
        ShopController.Instance?.RefreshPlayerInventoryDisplay();
    }
}