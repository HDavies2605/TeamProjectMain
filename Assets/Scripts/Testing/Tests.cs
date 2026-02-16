using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tests : MonoBehaviour
    {
        
        [Header("Test Settings")]
        [Tooltip("Enemy to fight when pressing B")]
        public EnemyDataSO testEnemy; // Drag an enemy ScriptableObject here
        
        void Update()
        {
            // Press H to take damage
            if (Input.GetKeyDown(KeyCode.H))
            {
                GameManager.Instance.playerData.currentHealth -= 20;
                if (GameManager.Instance.playerData.currentHealth < 0)
                    GameManager.Instance.playerData.currentHealth = 0;
            }
            
    
            // Press M to use mana
            if (Input.GetKeyDown(KeyCode.M))
            {
                GameManager.Instance.playerData.currentMana -= 10;
                if (GameManager.Instance.playerData.currentMana < 0)
                    GameManager.Instance.playerData.currentMana = 0;
            }
            
            // Press B to start a test battle
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (testEnemy != null)
                {
                    StartBattle(testEnemy);
                }
                else
                {
                    Debug.LogWarning("No test enemy assigned! Drag an enemy into the Test Enemy slot.");
                }
            }
        }
        
        /// <summary>
        /// Start a battle with the given enemy
        /// </summary>
        void StartBattle(EnemyDataSO enemySO)
        {
            EnemyData enemy = enemySO.ToEnemyData();
        
            // Store enemy data in PlayerPrefs for battle scene to load
            PlayerPrefs.SetString("CurrentEnemyID", enemySO.enemyName); // For finding animator
            PlayerPrefs.SetString("CurrentEnemyName", enemy.enemyName);
            PlayerPrefs.SetInt("CurrentEnemyHealth", enemy.maxHealth);
            PlayerPrefs.SetInt("CurrentEnemyAttack", enemy.attack);
            PlayerPrefs.SetInt("CurrentEnemyDefense", enemy.defense);
            PlayerPrefs.SetInt("CurrentEnemySpeed", enemy.speed);
            PlayerPrefs.SetInt("CurrentEnemyXP", enemy.experienceReward);
            PlayerPrefs.SetInt("CurrentEnemySpecialChance", enemy.specialAttackChance);
            PlayerPrefs.SetInt("CurrentEnemySpecialDamage", enemy.specialAttackDamage);
            PlayerPrefs.SetString("CurrentEnemySpecialName", enemy.specialAttackName);
        
            // Load battle scene
            SceneManager.LoadScene("BattleScene");
        }
    }