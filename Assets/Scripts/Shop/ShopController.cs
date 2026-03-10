using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public static ShopController Instance;

    [Header("UI")]
    public GameObject shopPanel;
    public Transform shopInventoryGrid, playerInventoryGrid;
    public GameObject shopSlotPrefab;
    public TMP_Text playerMoneyText, shopTitleText;

    private ShopNPC currentShop;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        shopPanel.SetActive(false);
        if (CurrencyController.Instance != null)
        {

            CurrencyController.Instance.OnMoneyChanged += UpdateMoneyDisplay;
            UpdateMoneyDisplay(CurrencyController.Instance.GetMoney());

        }
    }

    private void UpdateMoneyDisplay(int amount)
    {
        if (playerMoneyText != null)
        {
            playerMoneyText.text = amount.ToString();
        }
    }

    public void OpenShop(ShopNPC shop)
    {
        currentShop = shop;
        shopPanel.SetActive(true);
        if (shopTitleText != null)
        {
            shopTitleText.text = shop.shopkeeperName + "'s Shop";   //e.g., "(NPC NAME)'s Shop"
        }

        //PopulateShop();
        RefreshShopDisplay();
        //RefreshPlayerInventoryDisplay();

        PauseController.SetPaused(true);    //player cant run around while in shop

    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        currentShop = null;
        PauseController.SetPaused(false);
    }


    public void RefreshShopDisplay()
    {
        if (currentShop == null)
        {
            return;
        }

        // Clear existing slots
        foreach (Transform child in shopInventoryGrid)
        {
            Destroy(child.gameObject);
        }

        // Create slots from ScriptableObjects
        foreach (var stockItem in currentShop.GetShopStock())
        {
            if (stockItem.quantity == 0)
            {
                continue; // skip out of stock
            }

            CreateShopSlot(shopInventoryGrid, stockItem.item, stockItem.quantity, stockItem.infiniteStock);
        }
    }


    /*public void RefreshPlayerInventoryDisplay()
    {
        if (InventoryController.Instance != null)
        {
            return;
        }
        foreach (Transform child in playerInventoryGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform slotTransform in InventoryController.Instance.inventoryPage.transform)
        {
            InventorySlot inventorySlot = slotTransform.GetComponent<InventorySlot>();
            if (inventorySlot?.currentItem != null)
            {
                Item originalItem = inventorySlot.currentItem.GetComponent<Item>();
                CreateShopSlot(playerInventoryGrid, originalItem.ID, originalItem.quantity, false, inventorySlot);
            }
        } 
    } */

    private void CreateShopSlot(Transform grid, ItemDataSO itemData, int quantity, bool infinite)
    {
        GameObject slotObj = Instantiate(shopSlotPrefab, grid);

        ShopSlot slot = slotObj.GetComponent<ShopSlot>();

        if (slot != null)
        {
            slot.SetItem(itemData, quantity, infinite);
        }
    }

}