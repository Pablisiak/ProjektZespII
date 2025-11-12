using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("Rzeczy do ukrycia w sklepie")]
    public List<GameObject> PlayerInterfaces;

    [Header("Teksty pieniędzy graczy w sklepie")]
    public List<TMP_Text> playerMoneyTexts;

    public static int currentWaveIndex = -1;
    private bool isWaveActive = false;
    public List<GameObject> activeEnemies = new List<GameObject>();
    public ShopMenager ShopMenager;
    public static WaveManager waveManager;

    void Awake()
    {
        waveManager = this;
    }

    void Start()
    {
        shopUI.SetActive(false);
        if (waveUI != null)
            waveUI.UpdateWaveTimer(20);
        StartNextWave();
    }

    public void GiveKaska()
    {
        foreach (var playerObj in Players.players.PlayersList)
        {
            Player player = playerObj.GetComponent<Player>();
            player.Money += player.Stats.Harveresting;
        }
    }

    public void StartNextWave()
    {
        if (isWaveActive) return;
        // TODO: Po skończeniu wszystkich fal nakłada się HUD na sklep - docelowo podmienić na jakąś scenę Game Over czy coś takiego
        SetPlayerInterfacesActive(true);

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

    public void SetPlayerInterfacesActive(bool active)
    {
        foreach (var ui in PlayerInterfaces)
        {
            if (ui != null)
                ui.SetActive(active);
        }
    }

    public void UpdatePlayerMoneyTexts()
    {
        for (int i = 0; i < playerMoneyTexts.Count; i++)
        {
            if (playerMoneyTexts[i] != null)
            {
                var player = Players.players.PlayersList[i].GetComponent<Player>();
                int displayIndex = i + 1;
                playerMoneyTexts[i].text = $"Gracz {displayIndex} - Pieniądze: {player.Money}$";
            }
        }
    }


  IEnumerator HandleWave(Wave wave)
    {
        isWaveActive = true;
        shopUI.SetActive(false);

        // oblicz łączną liczbę przeciwników
        int totalEnemies = 0;
        foreach (var type in wave.enemies)
            totalEnemies += type.count;

        float spawnInterval = wave.duration / totalEnemies; // odstęp czasu między spawnami
        Dictionary<GameObject, int> remainingByType = new Dictionary<GameObject, int>();
        foreach (var type in wave.enemies)
            remainingByType[type.enemyPrefab] = type.count;

        float elapsedTime = 0f;

        while (elapsedTime < wave.duration)
        {
            if (waveUI != null)
                waveUI.UpdateWaveTimer(wave.duration - elapsedTime);

            GameObject prefab = GetNextAvailablePrefab(remainingByType);
            if (prefab != null)
            {
                Vector3 spawnPos = GetRandomSpawnPosition();

                // upewnij się, że nie pojawi się zbyt blisko gracza
                if (Vector3.Distance(spawnPos, player.position) >= minDistanceFromPlayer)
                {
                    GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
                    activeEnemies.Add(enemy);
                    remainingByType[prefab]--;
                }
            }

            yield return new WaitForSeconds(spawnInterval);
            elapsedTime += spawnInterval;
        }

        // koniec fali – usuń pozostałych przeciwników, jeśli trzeba
        foreach (var e in activeEnemies)
        {
            if (e != null) Destroy(e);
        }
        activeEnemies.Clear();

        if (waveUI != null)
            waveUI.UpdateWaveTimer(0);
        
        GiveKaska();
        isWaveActive = false;
        shopUI.SetActive(true);
        SetPlayerInterfacesActive(false);
        UpdatePlayerMoneyTexts();
        ShopMenager.Roll();
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
