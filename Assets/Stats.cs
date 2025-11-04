using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public int HP;
    public int MaxHP;
    public int RegenerationHP;
    public int Attack;
    public int Strength;
    public int Agility;
    public int Magic;
    public int AttackSpeed;
    public int CritChance;
    public int Range;
    public int Armor;
    public int Doge;
    public int Speed;
    public int Luck;
    public int Harveresting;

    public static Stats operator +(Stats a, Stats b)
    {
        return new Stats
        {
            HP = a.HP + b.HP,
            MaxHP = a.MaxHP + b.MaxHP,
            RegenerationHP = a.RegenerationHP + b.RegenerationHP,
            Strength = a.Strength + b.Strength,
            Agility = a.Agility + b.Agility,
            Magic = a.Magic + b.Magic,
            AttackSpeed = a.AttackSpeed + b.AttackSpeed,
            CritChance = a.CritChance + b.CritChance,
            Range = a.Range + b.Range,
            Armor = a.Armor + b.Armor,
            Doge = a.Doge + b.Doge > 60 ? 60 : a.Doge + b.Doge,
            Speed = a.Speed + b.Speed,
            Luck = a.Luck + b.Luck,
            Harveresting = a.Harveresting + b.Harveresting
        };
    }
}

