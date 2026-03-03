using UnityEngine;

namespace Data
{
    /// <summary>
    /// ScriptableObject template for creating items
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Game/Item", order = 3)]
    public class ItemDataSO : ScriptableObject
    {
        [Header("Basic Info")]
        [Tooltip("Item name displayed to player")]
        public string itemName;
        [TextArea(2, 4)]
        [Tooltip("Description of what item does")]
        public string description;
        [Tooltip("Icon displayed in inventory")]
        public Sprite itemIcon;
    
        [Header("Item Type")]
        public ItemType itemType;
    
        [Header("Economy")]
        [Tooltip("Buy/sell price")]
        public int value = 10;
    
        [Header("Consumable Effects (Potions, Food)")]
        [Tooltip("HP restored when used")]
        public int healthRestore = 0;
        [Tooltip("MP restored when used")]
        public int manaRestore = 0;
    
        [Header("Equipment Bonuses (Weapons, Armor)")]
        [Tooltip("Attack bonus when equipped")]
        public int attackBonus = 0;
        [Tooltip("Defense bonus when equipped")]
        public int defenseBonus = 0;
        [Tooltip("Speed bonus when equipped")]
        public int speedBonus = 0;
    
        /// <summary>
        /// Convert this ScriptableObject to runtime ItemData
        /// </summary>
        public ItemData ToItemData()
        {
            return new ItemData
            {
                itemName = this.itemName,
                description = this.description,
                itemType = this.itemType,
                value = this.value,
                healthRestore = this.healthRestore,
                manaRestore = this.manaRestore,
                attackBonus = this.attackBonus,
                defenseBonus = this.defenseBonus,
                speedBonus = this.speedBonus
            };
        }
    }
}