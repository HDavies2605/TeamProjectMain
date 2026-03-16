using UnityEngine;
using TMPro;
using Data;

public class ShopController : MonoBehaviour
{
    public static ShopController Instance;

    [Header("UI")]
    public GameObject shopPanel;
    public Transform shopInventoryGrid, playerInventoryGrid;
    public GameObject shopSlotPrefab;
    public TMP_Text playerMoneyText, shopTitleText;

    private ShopNPC currentShop;
    public ShopNPC CurrentShop => currentShop;

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

    private void Start()
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
            shopTitleText.text = shop.shopkeeperName + "'s Shop";
        }

        RefreshShopDisplay();
        RefreshPlayerInventoryDisplay();
        PauseController.SetPaused(true);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        currentShop = null;
        PauseController.SetPaused(false);

        // Refresh the real inventory UI after closing the shop
        if (InventoryController.Instance != null)
        {
            InventoryController.Instance.RefreshInventoryUI();
        }
    }

    public void RefreshShopDisplay()
    {
        if (currentShop == null)
        {
            return;
        }

        foreach (Transform child in shopInventoryGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var stockItem in currentShop.GetShopStock())
        {
            if (stockItem.quantity == 0 && !stockItem.infiniteStock)
            {
                continue;
            }

            CreateSlot(shopInventoryGrid, stockItem.item, stockItem.quantity, stockItem.infiniteStock, 1f, true);
        }
    }

    public void RefreshPlayerInventoryDisplay()
    {
        if (InventoryController.Instance == null)
        {
            return;
        }

        foreach (Transform child in playerInventoryGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (var invItem in InventoryController.Instance.GetInventoryItems())
        {
            if (invItem.item == null || invItem.quantity <= 0)
            {
                continue;
            }

            CreateSlot(playerInventoryGrid, invItem.item, invItem.quantity, false, 0.75f, false);
        }
    }

    private void CreateSlot(Transform grid, ItemDataSO itemData, int quantity, bool infinite, float multiplier, bool isShop)
    {
        GameObject slotObj = Instantiate(shopSlotPrefab, grid);
        ShopSlot slot = slotObj.GetComponent<ShopSlot>();

        if (slot != null)
        {
            slot.priceMultiplier = multiplier;
            slot.SetItem(itemData, quantity, infinite);

            if (itemData.prefab != null)
            {
                GameObject itemObj = Instantiate(itemData.prefab, slotObj.transform);
                itemObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                Item itemComp = itemObj.GetComponent<Item>();
                if (itemComp != null)
                {
                    itemComp.quantity = quantity;
                    itemComp.itemDataSO = itemData;
                    itemComp.UpdateQuantityDisplay();
                }

                slot.currentItem = itemObj;
            }
        }

        ShopItemHandler handler = slotObj.AddComponent<ShopItemHandler>();
        handler.Initialise(isShop, slot);
    }
}