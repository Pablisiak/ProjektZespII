using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class Weapon : ScriptableObject
{
    public float BasicDamage;
    public float FireRate;
    public float CritDamage;

    public virtual int DMG(Stats stats)
    {
        float dmg = BasicDamage * ((float)(stats.Attack + 100) / 100);
        return (int)dmg;
    }

}
