using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public Stats Stats;
    public Weapon Wepon;

    void Start()
    {
        Stats.HP = Stats.MaxHP;
        StartCoroutine(Regeneration());
    }

    public void TakeDamage(int amount)
    {
        ShowText(amount.ToString());
        Stats.HP -= amount;
        if (Stats.HP <= 0)
        {
            Debug.Log("Gracz zginął!");
        }
    }

    void ShowText(string tekst)
    {
        GameObject NewPop = Instantiate(PrefabMenager.prefabMenager.PopUp, transform.position, Quaternion.identity);
        NewPop.GetComponent<PopUp>().Set(tekst);
        Destroy(NewPop, 1f);
    }


    IEnumerator Regeneration()
    {
        while (true)
        {
            Debug.Log("Regeneruję...");
            if(Stats.MaxHP < Stats.HP)
            {
                Stats.HP++;
            }
            yield return new WaitForSeconds(10f/Stats.RegenerationHP); 
        }
    }
}
