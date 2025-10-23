using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public GameObject enemyPrefab;
    public int count;
}

[System.Serializable]
public class Wave
{
    public List<EnemyType> enemies;
    public float duration = 20f;
}

public class WaveManager : MonoBehaviour
{
    [Header("Ustawienia fal")]
    public List<Wave> waves;
    public Transform bottomLeft;
    public Transform topRight;
    public Transform player;
    public float minDistanceFromPlayer = 3f;
    public GameObject shopUI;

    [Header("UI")]
    public WaveUI waveUI;

    private int currentWaveIndex = -1;
    private bool isWaveActive = false;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        shopUI.SetActive(false);
        if (waveUI != null)
            waveUI.UpdateWaveTimer(20);
        StartNextWave();
    }

    public void StartNextWave()
    {
        if (isWaveActive) return;

        currentWaveIndex++;
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("Wszystkie fale ukończone!");
            if (waveUI != null)
                waveUI.UpdateWaveTimer(0);
            return;
        }

        StartCoroutine(HandleWave(waves[currentWaveIndex]));
    }

    IEnumerator HandleWave(Wave wave)
    {
        isWaveActive = true;
        shopUI.SetActive(false);

        int totalEnemies = 0;
        foreach (var type in wave.enemies)
            totalEnemies += type.count;

        float elapsedTime = 0f;
        float timeStep = 1f; // spawn co sekundę
        Dictionary<GameObject, int> remainingByType = new Dictionary<GameObject, int>();
        foreach (var type in wave.enemies)
            remainingByType[type.enemyPrefab] = type.count;

        while (elapsedTime < wave.duration)
        {
            if (waveUI != null)
                waveUI.UpdateWaveTimer(wave.duration - elapsedTime);

            // spawn 3 przeciwników co sekundę
            for (int i = 0; i < 3; i++)
            {
                GameObject prefab = GetNextAvailablePrefab(remainingByType);
                if (prefab == null) break;

                Vector3 spawnPos = GetRandomSpawnPosition();
                if (Vector3.Distance(spawnPos, player.position) < minDistanceFromPlayer)
                    continue;

                GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
                activeEnemies.Add(enemy);
                remainingByType[prefab]--;
            }

            yield return new WaitForSeconds(timeStep);
            elapsedTime += timeStep;
        }

        // Zakończenie fali
        foreach (var e in activeEnemies)
        {
            if (e != null) Destroy(e);
        }
        activeEnemies.Clear();

        if (waveUI != null)
            waveUI.UpdateWaveTimer(0); // 👈 Pokazuje "Fala zakończona!"

        isWaveActive = false;
        shopUI.SetActive(true);
        Debug.Log("Fala zakończona — sklep aktywny!");
    }

    GameObject GetNextAvailablePrefab(Dictionary<GameObject, int> remaining)
    {
        List<GameObject> available = new List<GameObject>();
        foreach (var kvp in remaining)
        {
            if (kvp.Value > 0) available.Add(kvp.Key);
        }
        if (available.Count == 0) return null;
        return available[Random.Range(0, available.Count)];
    }

    Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(bottomLeft.position.x, topRight.position.x);
        float y = Random.Range(bottomLeft.position.y, topRight.position.y);
        return new Vector3(x, y, 0);
    }
}
