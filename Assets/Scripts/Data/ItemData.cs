using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public string description;
        public ItemType itemType;
        public Sprite itemIcon;
        public int value; // For selling/buying
    
        // For consumables
        public int healthRestore;
        public int manaRestore;
    
        // For equipment
        public int attackBonus;
        public int defenseBonus;
        public int speedBonus;
    }

    public enum ItemType
    {
        Consumable,
        Weapon,
        Armor,
        KeyItem
    }
}