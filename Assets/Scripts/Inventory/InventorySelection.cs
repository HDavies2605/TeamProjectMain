using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;

public class InventorySelection : MonoBehaviour
{
    public static InventorySelection Instance;

    public Item selectedItem;

    // Description box references
    public GameObject descriptionBox;
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public TMP_Text itemStatsText;
    public Image itemImageDisplay;
    public Button equipButton;

    private void Awake()
    {
        Instance = this;
        if (descriptionBox != null)
            descriptionBox.SetActive(false);
    }

    public void SelectItem(Item item)
    {
        
        selectedItem = item;
        if (item == null || descriptionBox == null) return;

        itemNameText.text = item.itemDataSO.itemName;
        itemDescriptionText.text = item.itemDataSO.description;


        //if you want to add anything else to the description box, add it here.
        string stats = "";
        if (item.itemDataSO.attackBonus > 0)
            stats += $"Attack: {item.itemDataSO.attackBonus}\n";
        if (item.itemDataSO.defenseBonus > 0)
            stats += $"Defense: {item.itemDataSO.defenseBonus}\n";
        if (item.itemDataSO.speedBonus > 0)
            stats += $"Speed: {item.itemDataSO.speedBonus}\n";
        itemStatsText.text = stats;

        if (itemImageDisplay != null)
            itemImageDisplay.sprite = item.itemDataSO.itemIcon;

        descriptionBox.SetActive(true);
        equipButton.gameObject.SetActive(item.itemDataSO.itemType == ItemType.Weapon);
    }

    public void EquipSelected()
    {
        if (selectedItem == null) return;
        EquipmentManager.Instance.Equip(selectedItem.itemDataSO);
        descriptionBox.SetActive(false);
    }

    public void CloseDescriptionBox()
    {
        descriptionBox.SetActive(false);
        selectedItem = null;
    }
}