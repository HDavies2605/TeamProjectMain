using TMPro;
using UnityEngine;
using UnityEngine.UI; // For UI Image
using Data; // for ItemDataSO

public class Item : MonoBehaviour
{
    public int ID;
    public int quantity = 1;

    // ADD THIS: ScriptableObject reference
    public ItemDataSO itemDataSO;

    private TMP_Text quantityText;
    private Image itemImage;
    private SpriteRenderer spriteRenderer; // World sprite

    private void Awake()
    {
        quantityText = GetComponentInChildren<TMP_Text>();
        itemImage = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateQuantityDisplay();
        UpdateSpriteFromSO();
    }

    public void UpdateQuantityDisplay()
    {
        if (quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString() : ""; // show quantity if more than 1
        }
    }

    private void UpdateSpriteFromSO()
    {
        if (itemDataSO == null) return;

        if (itemImage != null)
        {
            itemImage.sprite = itemDataSO.itemIcon;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = itemDataSO.itemIcon;
        }
    }

    public void AddToStack(int amount = 1)
    {
        quantity += amount;
        UpdateQuantityDisplay();
    }

    public int RemoveFromStack(int amount = 1)
    {
        int removedAmount = Mathf.Min(amount, quantity);
        quantity -= removedAmount;
        UpdateQuantityDisplay();
        return removedAmount;
    }

    //splitting items
    public GameObject CloneItem(int newQuantity)
    {
        GameObject clone = Instantiate(gameObject);   //copy game object
        Item cloneItem = clone.GetComponent<Item>();
        cloneItem.quantity = newQuantity;   //give new quantity
        cloneItem.UpdateQuantityDisplay();
        return clone;
    }
}