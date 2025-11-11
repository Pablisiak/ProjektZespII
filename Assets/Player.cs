using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    public Stats Stats;
    public Weapon Weapon;
    public int Money;
    public List<Item> Items;

    void Start()
    {
        Stats.HP = Stats.MaxHP;
        StartCoroutine(Regeneration());
    }

    public void TakeDamage(int amount)
    {
        int Rng = Random.Range(0,100);
        if(Rng < Stats.Doge)
        {
            ShowText("Dodge", Color.white);
        }
        else
        {
            float dmg = amount;
            for(int i = 0; i < Stats.Armor; i++)
            {
                dmg *= 0.99f;
            }
            dmg = (int)dmg;
            ShowText(dmg.ToString(), Color.red);
            Stats.HP -= (int)dmg;
            if (Stats.HP <= 0)
            {
                Debug.Log("Gracz zginął!");
            }
        }
    }

    void ShowText(string tekst)
    {
        GameObject NewPop = Instantiate(PrefabMenager.prefabMenager.PopUp, transform.position, Quaternion.identity);
        NewPop.GetComponent<PopUp>().Set(tekst);
        Destroy(NewPop, 1f);
    }

    void ShowText(string tekst, Color kolor)
    {
        GameObject NewPop = Instantiate(PrefabMenager.prefabMenager.PopUp, transform.position, Quaternion.identity);
        PopUp pop = NewPop.GetComponent<PopUp>();
        pop.Set(tekst);
        pop.SetColor(kolor); // nowa linia
        Destroy(NewPop, 1f);
    }



    IEnumerator Regeneration()
    {
        while (true)
        {
            if(Stats.MaxHP > Stats.HP && Stats.RegenerationHP > 0)
            {
                ShowText("1", Color.green); 
                Stats.HP++;
            }

            float wait =  Stats.RegenerationHP > 0 ? Stats.RegenerationHP : 10;

            yield return new WaitForSeconds(10f/wait); 
        }
    }
}
