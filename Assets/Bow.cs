using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow", menuName = "Items/Bow")]
public class Bow : Weapon
{
    public override int DMG(Stats stats)
    {
        float modifiedBase = BasicDamage + (stats.Agility * 4);
        float dmg = modifiedBase * ((stats.Attack + 100) / 100f);
        return Mathf.RoundToInt(dmg);
    }
}
