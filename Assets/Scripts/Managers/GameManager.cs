using System.Collections.Generic;
using Data;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Player Data
    public PlayerData playerData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        if (playerData == null)
        {
            playerData = new PlayerData();
        }
    }
        
    [Header("Enemy System")]
    [Tooltip("All enemy types in the game")]
    public List<EnemyDataSO> availableEnemies;
        
    [Header("Item System")]
    [Tooltip("All available items in the game")]
    public List<ItemDataSO> availableItems;

    public static void TriggerBattle(EnemyDataSO enemySO)
        {
            EnemyData enemy = enemySO.ToEnemyData();
            GameManager.Instance.playerData.lastOverworldScene = SceneManager.GetActiveScene().name;

            PlayerPrefs.SetString("CurrentEnemyID", enemy.enemyName);
            PlayerPrefs.SetString("CurrentEnemyName", enemy.enemyName);
            PlayerPrefs.SetInt("CurrentEnemyHealth", enemy.maxHealth);
            PlayerPrefs.SetInt("CurrentEnemyAttack", enemy.attack);
            PlayerPrefs.SetInt("CurrentEnemyDefense", enemy.defense);
            PlayerPrefs.SetInt("CurrentEnemySpeed", enemy.speed);
            PlayerPrefs.SetInt("CurrentEnemyXP", enemy.experienceReward);
            PlayerPrefs.SetInt("CurrentEnemySpecialChance", enemy.specialAttackChance);
            PlayerPrefs.SetInt("CurrentEnemySpecialDamage", enemy.specialAttackDamage);
            PlayerPrefs.SetString("CurrentEnemySpecialName", enemy.specialAttackName);

            SceneManager.LoadScene("BattleScene");
        }
}

[System.Serializable]
public class PlayerData
{
    // Basic Info
    public string playerName = "Hero";
    public int level = 1;
    public int experience = 0;

    // HP and MP
    public int currentHealth = 100;
    public int maxHealth = 100;
    public int currentMana = 30;
    public int maxMana = 30;

    // Base Stats
    public int baseMaxHealth = 100;
    public int baseMaxMana = 30;
    public int baseAttack = 10;
    public int baseDefense = 5;
    public int baseSpeed = 8;

    // Combat Stats
    public int attack = 10;
    public int defense = 5;
    public int speed = 8;

    // Skill Points
    public int availableSkillPoints = 0;

    // Returning to overworld
    public string lastOverworldScene = "Overworld";
    public Vector2 lastPosition;
}