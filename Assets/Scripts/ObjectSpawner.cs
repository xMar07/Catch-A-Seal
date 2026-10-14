using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] goodItemPrefabs; // e.g., Seals
    [SerializeField] private GameObject[] badItemPrefabs;  // e.g., Orcas
    [SerializeField] private float spawnInterval = 1.5f;

    [Header("Spawn Position Boundaries")]
    [SerializeField] private float minX = -8f; // Left boundary outside camera view
    [SerializeField] private float maxX = 8f;  // Right boundary outside camera view
    [SerializeField] private float spawnY = 6f; // Spawn height above screen top

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            DecideSpawnWave();
            timer = 0f;
        }
    }

    private void DecideSpawnWave()
    {
        // 60% chance to spawn a single item, 40% chance to spawn a dual pair (Good + Bad)
        float roll = Random.value;

        if (roll < 0.6f || (goodItemPrefabs.Length == 0 || badItemPrefabs.Length == 0))
        {
            SpawnSingleRandomItem();
        }
        else
        {
            SpawnGoodAndBadPair();
        }
    }

    private void SpawnSingleRandomItem()
    {
        // Randomly pick between good or bad
        bool chooseGood = Random.value > 0.5f;
        GameObject[] targetArray = chooseGood ? goodItemPrefabs : badItemPrefabs;

        GameObject prefabToSpawn = GetRandomPrefab(targetArray);
        if (prefabToSpawn != null)
        {
            float randomX = Random.Range(minX, maxX);
            Instantiate(prefabToSpawn, new Vector3(randomX, spawnY, 0f), Quaternion.identity);
        }
    }

    private void SpawnGoodAndBadPair()
    {
        GameObject goodPrefab = GetRandomPrefab(goodItemPrefabs);
        GameObject badPrefab = GetRandomPrefab(badItemPrefabs);

        if (goodPrefab != null && badPrefab != null)
        {
            // Split the screen width in half so the seal and orca spawn in separate areas
            float midPoint = (minX + maxX) / 2f;
            float x1 = Random.Range(minX, midPoint - 1f);
            float x2 = Random.Range(midPoint + 1f, maxX);

            // Instantiate both at different horizontal positions
            Instantiate(goodPrefab, new Vector3(x1, spawnY, 0f), Quaternion.identity);
            Instantiate(badPrefab, new Vector3(x2, spawnY, 0f), Quaternion.identity);
        }
        else
        {
            SpawnSingleRandomItem(); // Fallback if an array is empty
        }
    }

    private GameObject GetRandomPrefab(GameObject[] prefabArray)
    {
        if (prefabArray == null || prefabArray.Length == 0) return null;
        int randomIndex = Random.Range(0, prefabArray.Length);
        return prefabArray[randomIndex];
    }
}