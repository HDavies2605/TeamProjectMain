using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public int quantity = 1;

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
            quantityText.text = quantity > 1 ? quantity.ToString() : ""; // show quantity if more than 1, otherwise show empty
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
        cloneItem.quantity = newQuantity;   //give new quanitty
        cloneItem.UpdateQuantityDisplay();
        return clone;
    }

}
