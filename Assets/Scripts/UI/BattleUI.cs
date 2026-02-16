using System.Collections.Generic;
using Data;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages all UI elements in the battle scene
/// </summary>
public class BattleUI : MonoBehaviour
{
    public static BattleUI Instance { get; private set; }

    [Header("Player UI")]
    [Tooltip("Player name display")]
    public TextMeshProUGUI playerNameText;
    [Tooltip("Player health bar")]
    public Slider playerHealthBar;
    [Tooltip("Player mana bar")]
    public Slider playerManaBar;

    [Header("Enemy UI")]
    [Tooltip("Enemy name display")]
    public TextMeshProUGUI enemyNameText;
    [Tooltip("Enemy health bar")]
    public Slider enemyHealthBar;

    [Header("Action Buttons")]
    [Tooltip("Attack button")]
    public Button attackButton;
    [Tooltip("Flee button")]
    public Button fleeButton;

    [Header("Battle Log")]
    [Tooltip("Text showing battle events")]
    public TextMeshProUGUI battleLogText;
    [Tooltip("Maximum number of log lines to keep")]
    public int maxLogLines = 10;

    // Battle log history
    private List<string> logLines = new List<string>();

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
    /// Set up button listeners
    /// </summary>
    private void Start()
    {
        // Hook up button click events
        if (attackButton != null)
            attackButton.onClick.AddListener(OnAttackClicked);
    
        if (fleeButton != null)
            fleeButton.onClick.AddListener(OnFleeClicked);
    }

    /// <summary>
    /// Initialize UI with battle participants
    /// </summary>
    public void InitializeBattle(string playerName, string enemyName)
    {
        playerNameText.text = playerName;
        enemyNameText.text = enemyName;
    
        UpdatePlayerUI();
        UpdateEnemyUI();
    }

    /// <summary>
    /// Update player health and mana bars
    /// </summary>
    public void UpdatePlayerUI()
    {
        if (GameManager.Instance == null)
            return;
    
        PlayerData player = GameManager.Instance.playerData;
    
        // Update health bar
        if (playerHealthBar != null)
        {
            playerHealthBar.maxValue = player.maxHealth;
            playerHealthBar.value = player.currentHealth;
        }
    
        // Update mana bar
        if (playerManaBar != null)
        {
            playerManaBar.maxValue = player.maxMana;
            playerManaBar.value = player.currentMana;
        }
    }

    /// <summary>
    /// Update enemy health bar
    /// </summary>
    public void UpdateEnemyUI()
    {
        if (BattleManager.Instance == null || BattleManager.Instance.currentEnemy == null)
            return;
    
        EnemyData enemy = BattleManager.Instance.currentEnemy;
    
        if (enemyHealthBar != null)
        {
            enemyHealthBar.maxValue = enemy.maxHealth;
            enemyHealthBar.value = enemy.currentHealth;
        }
    }

    /// <summary>
    /// Enable or disable action buttons
    /// </summary>
    public void SetButtonsInteractable(bool interactable)
    {
        if (attackButton != null)
            attackButton.interactable = interactable;
    
        if (fleeButton != null)
            fleeButton.interactable = interactable;
    }

    /// <summary>
    /// Add a message to the battle log
    /// </summary>
    public void AddLogMessage(string message)
    {
        logLines.Add(message);
    
        // Keep only the most recent messages
        if (logLines.Count > maxLogLines)
        {
            logLines.RemoveAt(0);
        }
    
        // Update log display
        battleLogText.text = string.Join("\n", logLines);
    }

    /// <summary>
    /// Clear the battle log
    /// </summary>
    public void ClearLog()
    {
        logLines.Clear();
        battleLogText.text = "";
    }
    
    /// <summary>
    /// Called when Attack button is clicked
    /// </summary>
    private void OnAttackClicked()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.PlayerAttack();
        }
    }

    /// <summary>
    /// Called when Flee button is clicked
    /// </summary>
    private void OnFleeClicked()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.AttemptFlee();
        }
    }
}