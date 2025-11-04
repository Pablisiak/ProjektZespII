using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUp : MonoBehaviour
{
    public TMP_Text Health;

    public void Set(string tekst)
    {
        Health.text = tekst;
    }

    public void SetColor(Color kolor)
    {
        Health.color = kolor;
    }
}
