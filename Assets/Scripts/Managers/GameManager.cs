using UnityEngine;

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
}
