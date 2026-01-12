using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuantity : MonoBehaviour
{
    [Header("Prefaby graczy")]
    public GameObject[] playerPrefabs;

    [Header("UI graczy")]
    public GameObject[] playerUIs;

    [Header("Sklepy graczy")]
    public GameObject[] playerShops;

    [Header("Liczba graczy")]
    [Range(1,4)]
    public int playerCount = 1;

    [Header("Dodatkowe UI")]
    public List<GameObject> additionalUI;

    void Start()
    {
            if (SceneChanger.playerCount > 0)
            playerCount = SceneChanger.playerCount;
        UpdatePlayers();
    }

    public void UpdatePlayers()
    {
        for (int i = 0; i < playerPrefabs.Length; i++)
        {
            bool active = i < playerCount;

            if (playerPrefabs[i] != null)
                playerPrefabs[i].SetActive(active);

            if (playerUIs[i] != null)
                playerUIs[i].SetActive(active);

            if (playerShops[i] != null)
                playerShops[i].SetActive(active);
        }
    }

    public void SetPlayerCount(int count)
    {
        playerCount = Mathf.Clamp(count, 1, playerPrefabs.Length);
        UpdatePlayers();
    }  
}
