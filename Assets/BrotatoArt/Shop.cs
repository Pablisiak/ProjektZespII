using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Reflection;

public class Shop : MonoBehaviour
{
    public Item Item;
    public TMP_Text Name;
    public TMP_Text Cost;
    public TMP_Text Descript;
    public Image Art;
    public Image Rare;

    void Update()
    {
        Name.text = Item.Name;
        Cost.text = Item.Cost.ToString();
        Art.sprite = Item.Sprite;
        switch (Item.Tier)
        {
            case 0:
                Rare.color = Color.gray;
                break;
            case 1:
                Rare.color = Color.blue;
                break;
            case 2:
                Rare.color = new Color(0.5f, 0f, 0.5f); // fioletowy (brak gotowego w Color)
                break;
            case 3:
                Rare.color = Color.yellow;
                break;
            default:
                Rare.color = Color.white;
                break;
        }
        string opis = FormatStats(Item.Stats);
        Descript.text = opis;   
    }

    public string FormatStats(Stats stats)
    {
        StringBuilder positive = new StringBuilder();
        StringBuilder negative = new StringBuilder();

        // Pobranie wszystkich pól z klasy Stats
        FieldInfo[] fields = typeof(Stats).GetFields();

        foreach (var field in fields)
        {
            int value = (int)field.GetValue(stats);

            if (value > 0)
            {
                positive.AppendLine($"<color=#00FF00>+{value}</color> {field.Name}");
            }
            else if (value < 0)
            {
                negative.AppendLine($"<color=#FF0000>{value}</color> {field.Name}");
            }
        }

        // Łączymy dodatnie i ujemne wyniki
        return positive.ToString() + negative.ToString();
    }

}
