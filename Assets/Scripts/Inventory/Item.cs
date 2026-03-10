using TMPro;
using UnityEngine;
using Data; // for ItemDataSO

public class Item : MonoBehaviour
{
    public int ID;
    public int quantity = 1;

    // ADD THIS: ScriptableObject reference
    public ItemDataSO itemDataSO;

    private TMP_Text quantityText;

    private void Awake()
    {
        quantityText = GetComponentInChildren<TMP_Text>();
        UpdateQuantityDisplay();
    }

    public void UpdateQuantityDisplay()
    {
        if (quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString() : ""; // show quantity if more than 1
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