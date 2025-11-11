using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Players AllPlayers;
    public List<TMP_Text> Health;
    public List<TMP_Text> Money;

    void Update()
    {
        for (int i = 0; i < AllPlayers.PlayersList.Count; i++)
        {
            Player player = AllPlayers.PlayersList[i].GetComponent<Player>();
            if (player != null)
            {
                Health[i].text = player.Stats.HP.ToString() + "/" + player.Stats.MaxHP.ToString();
                Money[i].text = player.Money.ToString();
            }
        }
    }
}
