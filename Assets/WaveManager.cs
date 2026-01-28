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

    [Header("Ilość graczy")]
    public PlayerQuantity playerQuantity;

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
        foreach (var playerObj in playerQuantity.playerPrefabs)
        {
            if (playerObj.activeSelf)
            {
            int index = System.Array.IndexOf(playerQuantity.playerPrefabs, playerObj);
            if (index >= 0 && index < playerQuantity.playerUIs.Length)
                playerQuantity.playerUIs[index].SetActive(active);
            }
        }
        foreach (var obj in playerQuantity.additionalUI)
        {
            if (obj != null)
            obj.SetActive(active);
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

    void RevivePlayersForNewWave()
    {
        int count = (playerQuantity != null) ? playerQuantity.playerCount : 1;
        count = Mathf.Clamp(count, 1, playerQuantity.playerPrefabs.Length);

        for (int i = 0; i < count; i++)
        {
            var go = playerQuantity.playerPrefabs[i];
            if (go == null) continue;

            if (!go.activeSelf) go.SetActive(true);

            var p = go.GetComponent<Player>();
            if (p != null)
                p.ReviveFullHP();
        }
    }

    IEnumerator HandleWave(Wave wave)
    {
        isWaveActive = true;
        shopUI.SetActive(false);
        RevivePlayersForNewWave();


        int playerCount = SceneChanger.playerCount;
        float playerMultiplier = Mathf.Pow(1.5f, playerCount - 1);

        Debug.Log("Player count: " + playerCount);
        Debug.Log("Player multiplier: " + playerMultiplier);

        int totalEnemies = 0;
        Dictionary<GameObject, int> remainingByType = new Dictionary<GameObject, int>();

        foreach (var type in wave.enemies)
        {
            int scaledCount = Mathf.CeilToInt(type.count * playerMultiplier);
            totalEnemies += scaledCount;
            remainingByType[type.enemyPrefab] = scaledCount;
        }

        float spawnInterval = wave.duration / totalEnemies;
        float elapsedTime = 0f;

        while (elapsedTime < wave.duration)
        {
            if (waveUI != null)
                waveUI.UpdateWaveTimer(wave.duration - elapsedTime);

            GameObject prefab = GetNextAvailablePrefab(remainingByType);
            if (prefab != null)
            {
                Vector3 spawnPos = GetRandomSpawnPosition();
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
        return new Vector3(x, y, -0.001f);
    }
}
