using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Data; //scriptable objects script namespace

public class ShopSlot : MonoBehaviour
{
    [Header("Item Data")]
    public ItemDataSO itemData;   // drag sciptable object here in inspector!!! ! !!!!

    [Header("UI")]
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    private void Awake()
    {
        //find PriceText if not assigned
        if (priceText == null)
        {
            priceText = transform.Find("PriceText")?.GetComponent<TMP_Text>();
        }

        // Find QuantityText if not assigned
        if (quantityText == null)
        {
            quantityText = transform.Find("QuantityText")?.GetComponent<TMP_Text>();
        }

        if (itemImage == null)
        {
            itemImage = transform.Find("ItemImage")?.GetComponent<Image>();  //getting the image on the child object named "ItemImage"
        }

        Debug.Log("QuantityText assigned? " + (quantityText != null));
    }


    // Allows ShopController to assign a ScriptableObject item when the slot is created
    public void SetItem(ItemDataSO newItem, int quantity, bool infinite)
    {
        itemData = newItem;
        Debug.Log("Setting shop slot item: " + newItem.itemName + " quantity: " + quantity);


        if (itemData == null)
        {
            return;
        }

        // Quantity display
        if (quantityText != null)
        {
            if (infinite)
            {
                quantityText.text = "∞"; // hide number for infinite
            }
            else
            {
                quantityText.text = "" + quantity;
            }
        }

        UpdateDisplay();
    }
 

    //all comes directly from the scriptable objects.
    public void UpdateDisplay()
    {
        if (itemData == null)
        {
            return;  //do nothing if no item
        }

        // PRICE
        if (priceText != null)
        {
            priceText.text = itemData.value.ToString();  //update price
        }

        // IMAGE
        if (itemImage != null)
        {
            itemImage.sprite = itemData.itemIcon;   //update image
        }
    }
}