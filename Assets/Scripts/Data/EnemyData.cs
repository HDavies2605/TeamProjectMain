using UnityEngine;

namespace Data
{
    /// <summary>
    /// Defines an enemy's stats and behavior in combat
    /// Can be used as a ScriptableObject template or runtime instance
    /// </summary>
    [System.Serializable]
    public class EnemyData
    {
        [Header("Basic Info")]
        public string enemyName = "Enemy";
        public Sprite enemySprite; // Visual representation
    
        [Header("Combat Stats")]
        public int maxHealth = 50;
        public int currentHealth = 50;
        public int attack = 8;
        public int defense = 3;
        public int speed = 5;
    
        [Header("Rewards")]
        public int experienceReward = 50;
        public int goldReward = 25;
    
        [Header("AI Behavior")]
        [Tooltip("Chance to use a special attack instead of basic attack (0-100)")]
        public int specialAttackChance = 30;
        public int specialAttackDamage = 15;
        public string specialAttackName = "Power Attack";
    
        /// <summary>
        /// Create a copy of enemy data for runtime use
        /// Prevents modifying the template
        /// </summary>
        public EnemyData Clone()
        {
            return new EnemyData
            {
                enemyName = this.enemyName,
                enemySprite = this.enemySprite,
                maxHealth = this.maxHealth,
                currentHealth = this.maxHealth, // Start at full health
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
        /// Calculate damage dealt to this enemy
        /// </summary>
        public int TakeDamage(int incomingDamage)
        {
            // Damage formula: damage - (defense / 2)
            int actualDamage = Mathf.Max(1, incomingDamage - (defense / 2));
            currentHealth -= actualDamage;
        
            if (currentHealth < 0)
                currentHealth = 0;
        
            return actualDamage;
        }
    
        /// <summary>
        /// Check if enemy is defeated
        /// </summary>
        public bool IsDefeated()
        {
            return currentHealth <= 0;
        }
    }
}