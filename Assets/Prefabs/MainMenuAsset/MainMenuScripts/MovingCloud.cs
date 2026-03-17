using UnityEngine;

public class MovingClouds : MonoBehaviour
{
    public GameObject cloudPrefab; // The cloud prefab
    public float spawnInterval = 2f; // Time between spawns
    public Transform[] spawnPoints; // Array of spawn points

    private static bool hasSpawner = false;
    private Vector3[] initialPositions;

    private void Start()
    {
        if (hasSpawner)
        {
            Destroy(gameObject);
            return;
        }
        hasSpawner = true;
        // Store initial positions
        initialPositions = new Vector3[spawnPoints.Length];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            initialPositions[i] = spawnPoints[i].position;
        }
        // Start spawning clouds
        InvokeRepeating("SpawnCloud", 0f, spawnInterval);
    }

    void SpawnCloud()
    {
        if (initialPositions.Length == 0)
        {
            Debug.Log("No spawn positions available.");
            return; // No spawn positions available
        }

        // Choose a random spawn position
        Vector3 spawnPos = initialPositions[Random.Range(0, initialPositions.Length)];

        // Instantiate the cloud prefab at the spawn position
        Instantiate(cloudPrefab, spawnPos, Quaternion.identity);
        Debug.Log("Spawned cloud at: " + spawnPos);
    }

    private void OnDrawGizmos()
    {
        // Draw spawn positions in the scene view
        Gizmos.color = Color.red;
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                Gizmos.DrawSphere(spawnPoint.position, 0.5f);
            }
        }
    }
}
