using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public float duration; // czas trwania fali
}

public class WaveManager : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 5f;

    private int currentWaveIndex = 0;
    private bool isWaveActive = false;

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave wave = waves[currentWaveIndex];
            isWaveActive = true;

            // Spawn przeciwników
            for (int i = 0; i < wave.enemyCount; i++)
            {
                Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(wave.enemyPrefab, spawn.position, Quaternion.identity);
            }

            // Czas trwania fali
            yield return new WaitForSeconds(wave.duration);

            // Zabij wszystkich pozostałych przeciwników
            foreach (Enemy e in FindObjectsOfType<Enemy>())
            {
                Destroy(e.gameObject);
            }

            isWaveActive = false;

            Debug.Log("Fala zakończona — otwórz sklep tutaj!");
            // 🔹 Tutaj możesz wywołać swój sklep

            yield return new WaitForSeconds(timeBetweenWaves);
            currentWaveIndex++;
        }

        Debug.Log("Wszystkie fale ukończone!");
    }
}
