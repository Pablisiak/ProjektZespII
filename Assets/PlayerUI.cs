using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Players AllPlayers;
    public TMP_Text Health;

    void Update()
    {
        Health.text = AllPlayers.PlayersList[0].GetComponent<Player>().Stats.HP.ToString() + "/" + AllPlayers.PlayersList[0].GetComponent<Player>().Stats.MaxHP.ToString();
    }
}
