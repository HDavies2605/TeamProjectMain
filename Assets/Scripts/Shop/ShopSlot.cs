using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Data;

public class ShopSlot : MonoBehaviour
{
    [Header("Item Data")]
    public ItemDataSO itemData;

    [Header("UI")]
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    [Header("Economy")]
    public float priceMultiplier = 1f; // modifies the base item price

    private int quantity;
    private bool infiniteStock;

    public GameObject currentItem;

    public void SetItem(ItemDataSO newItem, int qty, bool infinite)
    {
        itemData = newItem;
        quantity = qty;
        infiniteStock = infinite;

        if (quantityText != null)
        {
            quantityText.text = infinite ? "∞" : qty.ToString();
        }

        UpdateDisplay();
    }

    public int GetCurrentPrice()
    {
        if (itemData == null)
        {
            return 0;
        }

        return Mathf.Max(1, Mathf.FloorToInt(itemData.value * priceMultiplier));
    }

    public bool IsInfiniteStock()
    {
        return infiniteStock;
    }

    public void UpdateDisplay()
    {
        if (itemData == null)
        {
            return;
        }

        if (priceText != null)
        {
            priceText.text = GetCurrentPrice().ToString();
        }

        if (itemImage != null)
        {
            itemImage.sprite = itemData.itemIcon;
        }
    }

    public void UpdateQuantity(int newQuantity)
    {
        quantity = newQuantity;

        if (quantityText != null && !infiniteStock)
        {
            quantityText.text = quantity.ToString();
        }
    }
}