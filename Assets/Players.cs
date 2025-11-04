using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    public static Players players;

    public List<GameObject> PlayersList;

    void Awake()
    {
        players = this;
    }
}
