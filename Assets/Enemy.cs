using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int maxHealth = 100;
    public int damage = 1;
    public int money = 1;

    private int currentHealth;
    private Transform player;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Players.players.PlayersList[0].GetComponent<Player>().Money += money;
        Debug.Log(money);
        Destroy(gameObject);
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
        pop.SetColor(kolor); 
        Destroy(NewPop, 1f);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("ooooo");
            Player playerHealth = collision.gameObject.GetComponent<Player>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            int DMG = collision.gameObject.GetComponent<Bullet>().damage;
            if(collision.gameObject.GetComponent<Bullet>().crite)
                ShowText((DMG).ToString(), Color.yellow);
            else
                ShowText((DMG).ToString());
            Destroy(collision.gameObject);
            currentHealth -= DMG;
            if(currentHealth <= 0)
                Die();
        }
    }
}