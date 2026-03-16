using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController Instance;

    // Reference to the player's movement script
    public MonoBehaviour playerMovement;

    private bool paused = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public static void SetPaused(bool pause)
    {
        if (Instance == null) return;

        Instance.paused = pause;

        // Enable / disable player movement
        if (Instance.playerMovement != null)
            Instance.playerMovement.enabled = !pause;

        // Unlock cursor when paused (for UI)
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pause;
    }
}