using UnityEngine;
using UnityEngine.UI;

public class InventorySelection : MonoBehaviour
{
    public static InventorySelection Instance;

    public Item selectedItem;
    public Button equipButton;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectItem(Item item)
    {
        selectedItem = item;

        if (equipButton != null)
            equipButton.gameObject.SetActive(true);
    }

    public void EquipSelected()
    {
        if (selectedItem == null) return;

        EquipmentManager.Instance.Equip(selectedItem.itemDataSO);
    }
}