using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    /// Manages turn-based combat between player and enemy
    /// Handles combat flow, damage calculation, and victory/defeat conditions
    /// </summary>
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance { get; private set; }
    
        [Header("Combat State")]
        public EnemyData currentEnemy;
        public bool isPlayerTurn = true;
        public bool battleActive = false;
    
        [Header("Battle Settings")]
        [Tooltip("Scene to return to after battle")]
        public string overworldSceneName = "SampleScene";
        
        [Header("Visual References")]
        public Animator enemyAnimator; // Drag EnemyDisplay here in Inspector
        public SpriteRenderer enemySprite; // Drag EnemyDisplay here too
    
        // Combat states
        public enum BattleState
        {
            Start,
            PlayerTurn,
            EnemyTurn,
            Victory,
            Defeat
        }
    
        public BattleState currentState;
    
        /// <summary>
        /// Initialize singleton
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Load enemy data and start battle when scene loads
        /// </summary>
        private void Start()
        {
            if (PlayerPrefs.HasKey("CurrentEnemyName"))
            {
                EnemyData enemy = new EnemyData
                {
                    enemyName = PlayerPrefs.GetString("CurrentEnemyName"),
                    maxHealth = PlayerPrefs.GetInt("CurrentEnemyHealth"),
                    currentHealth = PlayerPrefs.GetInt("CurrentEnemyHealth"),
                    attack = PlayerPrefs.GetInt("CurrentEnemyAttack"),
                    defense = PlayerPrefs.GetInt("CurrentEnemyDefense"),
                    speed = PlayerPrefs.GetInt("CurrentEnemySpeed", 5),
                    experienceReward = PlayerPrefs.GetInt("CurrentEnemyXP"),
                    specialAttackChance = PlayerPrefs.GetInt("CurrentEnemySpecialChance", 20),
                    specialAttackDamage = PlayerPrefs.GetInt("CurrentEnemySpecialDamage", 15),
                    specialAttackName = PlayerPrefs.GetString("CurrentEnemySpecialName", "Power Attack")
                };

                // Find the SO and pull sprite + animator BEFORE StartBattle
                string enemyID = PlayerPrefs.GetString("CurrentEnemyID", enemy.enemyName);
                EnemyDataSO enemySO = GameManager.Instance.availableEnemies.Find(e => e.enemyName == enemyID);

                Debug.Log($"Looking for enemy ID: '{enemyID}'");
                Debug.Log($"Available enemies count: {GameManager.Instance.availableEnemies.Count}");
                foreach (var e in GameManager.Instance.availableEnemies)
                    Debug.Log($"  - Available enemy: '{e.enemyName}'");
                Debug.Log($"enemySO found: {enemySO != null}");
                Debug.Log($"enemySO sprite: {(enemySO != null ? enemySO.enemySprite?.name ?? "NULL" : "N/A")}");
                Debug.Log($"enemySO animator: {(enemySO != null ? enemySO.animatorController?.name ?? "NULL" : "N/A")}");
                Debug.Log($"enemySprite reference: {(enemySprite != null ? "SET" : "NULL")}");
                Debug.Log($"enemyAnimator reference: {(enemyAnimator != null ? "SET" : "NULL")}");
                
                if (enemySO != null)
                {
                    // Inject the sprite from the SO into the runtime data
                    enemy.enemySprite = enemySO.enemySprite;

                    if (enemySO.animatorController != null && enemyAnimator != null)
                        enemyAnimator.runtimeAnimatorController = enemySO.animatorController;
                }

                PlayerPrefs.DeleteKey("CurrentEnemyName");

                StartBattle(enemy);
            }
        }
    
        /// <summary>
        /// Start battle with a specific enemy
        /// </summary>
        public void StartBattle(EnemyData enemy)
        {
            currentEnemy = enemy.Clone(); // Use a copy so we don't modify the template
            battleActive = true;
            currentState = BattleState.Start;
            
            Debug.Log($"StartBattle called for: {currentEnemy.enemyName}");
            Debug.Log($"currentEnemy.enemySprite: {(currentEnemy.enemySprite != null ? currentEnemy.enemySprite.name : "NULL")}");
            Debug.Log($"enemySprite SpriteRenderer: {(enemySprite != null ? "SET" : "NULL")}");
            Debug.Log($"enemyAnimator Animator: {(enemyAnimator != null ? "SET" : "NULL")}");
            
            // SET ENEMY SPRITE AND ANIMATOR
            if (enemySprite != null && currentEnemy.enemySprite != null)
            {
                enemySprite.sprite = currentEnemy.enemySprite;
                
                Debug.Log($"Sprite applied: {enemySprite.sprite.name}");
                Debug.Log($"SpriteRenderer enabled: {enemySprite.enabled}");
                Debug.Log($"GameObject active: {enemySprite.gameObject.activeSelf}");
            }
            else
            {
                Debug.Log("SPRITE BLOCK SKIPPED");
            }
            if (enemyAnimator != null)
            {
                Debug.Log($"Animator enabled: {enemyAnimator.enabled}");
                Debug.Log($"Current controller: {(enemyAnimator.runtimeAnimatorController != null ? enemyAnimator.runtimeAnimatorController.name : "NULL")}");
            }
            
            string enemyID = PlayerPrefs.GetString("CurrentEnemyID", "");
            
            // Initialize UI
            if (BattleUI.Instance != null)
            {
                BattleUI.Instance.InitializeBattle(
                    GameManager.Instance.playerData.playerName,
                    currentEnemy.enemyName
                );
            }

            DetermineFirstTurn();
        }
    
        /// <summary>
        /// Determine turn order based on player and enemy speed
        /// </summary>
        private void DetermineFirstTurn()
        {
            PlayerData player = GameManager.Instance.playerData;
    
            if (player.speed >= currentEnemy.speed)
            {
                currentState = BattleState.PlayerTurn;
                isPlayerTurn = true;
        
                if (BattleUI.Instance != null)
                {
                    BattleUI.Instance.SetButtonsInteractable(true);
                    BattleUI.Instance.AddLogMessage("Your turn!");
                }
            }
            else
            {
                currentState = BattleState.EnemyTurn;
                isPlayerTurn = false;
        
                if (BattleUI.Instance != null)
                {
                    BattleUI.Instance.SetButtonsInteractable(false);
                    BattleUI.Instance.AddLogMessage("Enemy's turn!");
                }
        
                StartCoroutine(EnemyTurnCoroutine());
            }
        }
    
        /// <summary>
        /// Player performs a basic attack
        /// </summary>
        public void PlayerAttack()
        {
            if (currentState != BattleState.PlayerTurn || !battleActive)
            {
                Debug.Log("Attack blocked by state check!");
                return;
            }

            PlayerData player = GameManager.Instance.playerData;
            Debug.Log($"Player attack stat: {player.attack}");
    
            int damage = CalculatePlayerDamage(player.attack);
            
            int actualDamage = currentEnemy.TakeDamage(damage);
            Debug.Log($"Dealt {actualDamage} damage. Enemy HP: {currentEnemy.currentHealth}/{currentEnemy.maxHealth}");
    
            if (BattleUI.Instance != null)
            {
                BattleUI.Instance.AddLogMessage($"You attack for {actualDamage} damage");
                BattleUI.Instance.UpdateEnemyUI();
            }
    
            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("Hurt");
            }
    
            if (currentEnemy.IsDefeated())
            {
                Debug.Log("Enemy defeated!");
                Victory();
            }
            else
            {
                Debug.Log("Ending player turn...");
                EndPlayerTurn();
            }
        }
    
        /// <summary>
        /// Calculate player damage with some randomness
        /// </summary>
        private int CalculatePlayerDamage(int baseAttack)
        {
            // Damage = attack * random(0.9 to 1.1)
            float variance = Random.Range(0.9f, 1.1f);
            return Mathf.RoundToInt(baseAttack * variance);
        }
    
        /// <summary>
        /// End player turn and start enemy turn
        /// </summary>
        private void EndPlayerTurn()
        {
            currentState = BattleState.EnemyTurn;
            isPlayerTurn = false;
    
            if (BattleUI.Instance != null)
            {
                BattleUI.Instance.SetButtonsInteractable(false);
                BattleUI.Instance.AddLogMessage("Enemy's turn...");
            }
    
            StartCoroutine(EnemyTurnCoroutine());
        }
    
        /// <summary>
        /// Enemy takes their turn (with delay for readability)
        /// </summary>
        private IEnumerator EnemyTurnCoroutine()
        {
            yield return new WaitForSeconds(1f); // Delay so player can read results
        
            EnemyTurn();
        }
    
        /// <summary>
        /// Enemy AI logic and attack
        /// </summary>
        private void EnemyTurn()
        {
            if (!battleActive)
                return;
    
            PlayerData player = GameManager.Instance.playerData;
    
            bool useSpecial = Random.Range(0, 100) < currentEnemy.specialAttackChance;
    
            int damage;
            string attackName;
    
            if (useSpecial)
            {
                damage = currentEnemy.specialAttackDamage;
                attackName = currentEnemy.specialAttackName;
                enemyAnimator.SetTrigger("SpecialAttack");
            }
            else
            {
                damage = currentEnemy.attack;
                attackName = "Attack";
                enemyAnimator.SetTrigger("Attack");
            }
    
            int actualDamage = Mathf.Max(1, damage - (player.defense / 2));
            player.currentHealth -= actualDamage;
    
            if (player.currentHealth < 0)
                player.currentHealth = 0;
    
            if (BattleUI.Instance != null)
            {
                BattleUI.Instance.AddLogMessage($"{currentEnemy.enemyName} uses {attackName} for {actualDamage} damage!");
                BattleUI.Instance.UpdatePlayerUI();
            }
    
            if (player.currentHealth <= 0)
            {
                Defeat();
            }
            else
            {
                EndEnemyTurn();
            }
        }
        
        /// <summary>
        /// End enemy turn and return to player turn
        /// </summary>
        private void EndEnemyTurn()
        {
            currentState = BattleState.PlayerTurn;
            isPlayerTurn = true;
    
            if (BattleUI.Instance != null)
            {
                BattleUI.Instance.SetButtonsInteractable(true);
                BattleUI.Instance.AddLogMessage("Your turn!");
            }
        }
    
        /// <summary>
        /// Handle victory - return to overworld
        /// </summary>
        private void Victory()
        {
            currentState = BattleState.Victory;
            battleActive = false;

            if (enemyAnimator != null)
            {
                enemyAnimator.SetTrigger("Death");
            }
    
            if (BattleUI.Instance != null)
            {
                BattleUI.Instance.AddLogMessage($"Victory! Gained {currentEnemy.experienceReward} XP!");
                BattleUI.Instance.SetButtonsInteractable(false);
            }
    
            StartCoroutine(ReturnToOverworldCoroutine(3f));
        }
    
        /// <summary>
        /// Handle defeat - return to overworld
        /// </summary>
        private void Defeat()
        {
            currentState = BattleState.Defeat;
            battleActive = false;
    
            if (BattleUI.Instance != null)
            {
                BattleUI.Instance.AddLogMessage("Defeat! You were defeated...");
                BattleUI.Instance.SetButtonsInteractable(false);
            }
    
            StartCoroutine(LoadGameOverCoroutine());
        }
        
        /// <summary>
        /// Load the Game Over scene
        /// </summary>
        private IEnumerator LoadGameOverCoroutine()
        {
            yield return new WaitForSeconds(2f);
    
            Debug.Log("Loading Game Over screen...");
            SceneManager.LoadScene("GameOverScene");
        }
    
        /// <summary>
        /// Return to the overworld scene
        /// </summary>
        private IEnumerator ReturnToOverworldCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
        
            string returnScene = GameManager.Instance.playerData.lastOverworldScene;
    
            if (string.IsNullOrEmpty(returnScene))
            {
                returnScene = "MainScene"; // Default fallback
            }
            
            SceneManager.LoadScene(returnScene);
        }
    
        /// <summary>
        /// Allow player to flee from battle
        /// </summary>
        public void AttemptFlee()
        {
            battleActive = false;
    
            string returnScene = GameManager.Instance.playerData.lastOverworldScene;
            if (string.IsNullOrEmpty(returnScene))
            {
                returnScene = "MainScene";
            }
    
            SceneManager.LoadScene(returnScene);
        }
    }
}