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
    [SerializeField] private Image itemImage;

    private void Awake()
    {
        //find any UI components, (dont need to manually drag them in inspector)
        if (priceText == null)
        {
            priceText = GetComponentInChildren<TMP_Text>();
        }

        if (itemImage == null)
        {
            itemImage = transform.Find("ItemIcon")?.GetComponent<Image>();  //getting the image on the child object named "ItemIcon"
        }
    }

    private void Start()
    {
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