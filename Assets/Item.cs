using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Stats Stats;
    public int Cost;
    public Sprite Sprite;
    public int Tier;
}
