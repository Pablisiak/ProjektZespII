using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 25;
    public float lifetime = 1f;
    public bool crite;

    void Start()
    {
        // Zniszcz pocisk po określonym czasie
        Destroy(gameObject, lifetime);

        // Pobierz pierwszego gracza z listy i zignoruj kolizję
        if (Players.players != null && Players.players.PlayersList.Count > 0)
        {
            GameObject playerObj = Players.players.PlayersList[0];

            if (playerObj != null)
            {
                Collider2D bulletCol = GetComponent<Collider2D>();
                Collider2D playerCol = playerObj.GetComponent<Collider2D>();

                if (bulletCol != null && playerCol != null)
                {
                    Physics2D.IgnoreCollision(bulletCol, playerCol);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Trafienie przeciwnika
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
