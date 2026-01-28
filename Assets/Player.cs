using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    public static event Action<Player> AnyPlayerDied;
    public event Action<Player> Died;
    public Stats Stats;
    public Weapon Weapon;
    public int Money;
    public List<Item> Items;
    private Animator anim;
    public bool IsDead = false;
    public bool IsHurt = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        Stats.HP = Stats.MaxHP;
        StartCoroutine(Regeneration());
    }

    public void TakeDamage(int amount)
    {
        int Rng = UnityEngine.Random.Range(0,100);
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
            IsHurt = true;

            anim.SetTrigger("Hurt");

            if (Stats.HP <= 0)
            {   
                Debug.Log("Gracz zginął!");
                IsDead = true;
                anim.SetBool("Dead", true);
            }
        }

        if (Stats.HP <= 0 && !IsDead)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("UMAR");

        if (IsDead) return;

        IsDead = true;

        // Wyłącz sterowanie
        var movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        var shooting = GetComponent<PlayerShooting>();
        if (shooting != null)
            shooting.enabled = false;

        // Wyłącz kolizję
        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Wyłącz fizykę
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false;
        }

        Died?.Invoke(this);
        AnyPlayerDied?.Invoke(this);
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

    public void OnHurtAnimationEnd()
    {
        IsHurt = false;
    }
}
