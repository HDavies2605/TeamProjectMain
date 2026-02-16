using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    /// <summary>
    /// Handles random battle encounters as player moves
    /// Triggers battles based on steps taken
    /// </summary>
    public class RandomEncounters : MonoBehaviour
    {
        [Header("Encounter Settings")]
        [Tooltip("Minimum steps before a battle can occur")]
        public int minSteps = 5;
        [Tooltip("Maximum steps before a battle must occur")]
        public int maxSteps = 15;
        [Tooltip("Chance of encounter per step after minimum (0-100)")]
        [Range(0, 100)]
        public int encounterChance = 10;
    
        [Header("Enemy Pool")]
        [Tooltip("List of possible enemies to encounter")]
        public EnemyDataSO[] possibleEnemies;
    
        [Header("Battle Scene")]
        [Tooltip("Name of the battle scene to load")]
        public string battleSceneName = "BattleScene";
    
        // Step tracking
        private int stepsSinceLastBattle = 0;
        private int stepsUntilNextCheck;
        private Vector2 lastPosition;
        private float stepDistance = 0.5f; // Distance to count as a "step"
    
        /// <summary>
        /// Initialize encounter system
        /// </summary>
        private void Start()
        {
            lastPosition = transform.position;
            stepsUntilNextCheck = Random.Range(minSteps, maxSteps);
        }
    
        /// <summary>
        /// Track player movement and check for encounters
        /// </summary>
        private void Update()
        {
            // Calculate distance moved since last position
            float distanceMoved = Vector2.Distance(transform.position, lastPosition);
        
            // If moved enough to count as a step
            if (distanceMoved >= stepDistance)
            {
                stepsSinceLastBattle++;
                lastPosition = transform.position;
            
                // Check if we should try to trigger an encounter
                if (stepsSinceLastBattle >= stepsUntilNextCheck)
                {
                    CheckForEncounter();
                }
            }
        }
    
        /// <summary>
        /// Check if a random encounter should occur
        /// </summary>
        private void CheckForEncounter()
        {
            // If past minimum steps, random chance to encounter
            if (stepsSinceLastBattle >= minSteps)
            {
                int roll = Random.Range(0, 100);
            
                if (roll < encounterChance || stepsSinceLastBattle >= maxSteps)
                {
                    TriggerBattle();
                }
                else
                {
                    // No battle this time, check again after a few more steps
                    stepsUntilNextCheck = stepsSinceLastBattle + Random.Range(2, 5);
                }
            }
        }
    
        /// <summary>
        /// Start a random battle encounter
        /// Uses weighted random selection based on encounter rates
        /// </summary>
        private void TriggerBattle()
        {
            if (possibleEnemies == null || possibleEnemies.Length == 0)
            {
                return;
            }
    
            EnemyDataSO randomEnemy = GetWeightedRandomEnemy();
            
            GameManager.Instance.playerData.lastOverworldScene = SceneManager.GetActiveScene().name;
    
            EnemyData enemy = randomEnemy.ToEnemyData();
            
            PlayerPrefs.SetString("CurrentEnemyID", randomEnemy.enemyName);
    
            PlayerPrefs.SetString("CurrentEnemyName", enemy.enemyName);
    
            SceneManager.LoadScene(battleSceneName);
        }
    
        /// <summary>
        /// Reset encounter counter (called after battle ends)
        /// </summary>
        public void ResetEncounterCounter()
        {
            stepsSinceLastBattle = 0;
            stepsUntilNextCheck = Random.Range(minSteps, maxSteps);
        }
    
        /// <summary>
        /// Select a random enemy based on encounter weights
        /// Higher weight = more likely to appear
        /// </summary>
        private EnemyDataSO GetWeightedRandomEnemy()
        {
            // Calculate total weight
            int totalWeight = 0;
            foreach (EnemyDataSO enemy in possibleEnemies)
            {
                totalWeight += enemy.encounterWeight;
            }
        
            // Pick a random value within total weight
            int randomValue = Random.Range(0, totalWeight);
        
            // Find which enemy this value corresponds to
            int currentWeight = 0;
            foreach (EnemyDataSO enemy in possibleEnemies)
            {
                currentWeight += enemy.encounterWeight;
            
                if (randomValue < currentWeight)
                {
                    return enemy;
                }
            }
        
            // Fallback (should never happen)
            return possibleEnemies[0];
        }
    }
}