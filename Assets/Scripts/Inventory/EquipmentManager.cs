using UnityEngine;
using Data;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public ItemDataSO equippedWeapon;

    private void Awake()
    {
        Instance = this;
    }

    public void Equip(ItemDataSO item)
    {
        if (item == null) return;

        if (item.itemType != ItemType.Weapon)
        {
            Debug.Log("Item is not a weapon");
            return;
        }

        PlayerData player = GameManager.Instance.playerData;

        // Remove old weapon stats
        if (equippedWeapon != null)
        {
            player.attack -= equippedWeapon.attackBonus;
            player.defense -= equippedWeapon.defenseBonus;
            player.speed -= equippedWeapon.speedBonus;
        }

        // Equip new weapon
        equippedWeapon = item;

        // Apply new stats
        player.attack += item.attackBonus;
        player.defense += item.defenseBonus;
        player.speed += item.speedBonus;

        Debug.Log("Equipped: " + item.itemName);
        Debug.Log("New Attack: " + player.attack);
    }
}