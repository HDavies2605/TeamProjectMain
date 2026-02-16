using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// ScriptableObject template for creating enemy types
    /// </summary>
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Game/Enemy", order = 2)]
    public class EnemyDataSO : ScriptableObject
    {
        [Header("Basic Info")]
        [Tooltip("Name displayed in combat")]
        public string enemyName = "Enemy";
    
        [Header("Combat Stats")]
        [Tooltip("Maximum health points")]
        public int maxHealth = 50;
        [Tooltip("Base attack damage")]
        public int attack = 8;
        [Tooltip("Damage reduction")]
        public int defense = 3;
        [Tooltip("Determines turn order (higher = faster)")]
        public int speed = 5;
    
        [Header("Rewards")]
        [Tooltip("XP awarded on defeat")]
        public int experienceReward = 50;
        [Tooltip("Gold awarded on defeat")]
        public int goldReward = 25;
        
        [Header("Visual")]
        public Sprite enemySprite;
        [Tooltip("Animator with this enemy's animations")]
        public AnimatorOverrideController animatorController;
        public float displayScale = 5f;
    
        [Header("AI Behavior")]
        [Tooltip("Chance to use special attack instead of basic attack (0-100)")]
        [Range(0, 100)]
        public int specialAttackChance = 30;
        [Tooltip("Damage dealt by special attack")]
        public int specialAttackDamage = 15;
        [Tooltip("Name of special attack")]
        public string specialAttackName = "Power Attack";
        
        [Header("Encounter Settings")]
        [Tooltip("How common this enemy is (higher = more common).")]
        [Range(1, 100)]
        public int encounterWeight = 50;
        
        [Header("Item Drops")]
        [Tooltip("Items this enemy can drop")]
        public List<EnemyDropItem> possibleDrops = new List<EnemyDropItem>();
        
        [System.Serializable]
        public class EnemyDropItem
        {
            [Tooltip("Item that can drop")]
            public ItemDataSO item;
            [Tooltip("Drop chance percentage (0-100)")]
            [Range(0, 100)]
            public int dropChance = 50;
            [Tooltip("Minimum quantity to drop")]
            public int minQuantity = 1;
            [Tooltip("Maximum quantity to drop")]
            public int maxQuantity = 1;
        }
    
        /// <summary>
        /// Convert this ScriptableObject to runtime EnemyData
        /// </summary>
        public EnemyData ToEnemyData()
        {
            return new EnemyData
            {
                enemyName = this.enemyName,
                enemySprite = this.enemySprite,
                maxHealth = this.maxHealth,
                currentHealth = this.maxHealth, // Start at full HP
                attack = this.attack,
                defense = this.defense,
                speed = this.speed,
                experienceReward = this.experienceReward,
                goldReward = this.goldReward,
                specialAttackChance = this.specialAttackChance,
                specialAttackDamage = this.specialAttackDamage,
                specialAttackName = this.specialAttackName
            };
        }
        
        /// <summary>
        /// Roll for item drops when enemy is defeated
        /// Returns list of items that dropped
        /// </summary>
        public List<ItemData> RollForDrops()
        {
            List<ItemData> droppedItems = new List<ItemData>();
    
            foreach (EnemyDropItem dropItem in possibleDrops)
            {
                if (dropItem.item == null)
                {
                    continue;
                }
        
                // Roll for this item
                int roll = Random.Range(0, 100);

                if (roll < dropItem.dropChance)
                {
                    // Determine quantity of dropped item
                    int quantity = Random.Range(dropItem.minQuantity, dropItem.maxQuantity + 1);
            
                    for (int i = 0; i < quantity; i++)
                    {
                        droppedItems.Add(dropItem.item.ToItemData());
                    }
                }
            }

            return droppedItems;
        }
    }
}