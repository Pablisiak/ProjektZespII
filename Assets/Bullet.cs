using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 25;
    public float lifetime = 1f;
    public bool crite;
    public Player owner;

    void Start()
    {
        // Zniszcz pocisk po określonym czasie
        Destroy(gameObject, lifetime);

        // Pobierz WSZYSTKICH graczy z listy i zignoruj kolizję
        if (Players.players != null && Players.players.PlayersList.Count > 0)
        {
            Collider2D bulletCol = GetComponent<Collider2D>();
            if (bulletCol == null) return;

            foreach (GameObject playerObj in Players.players.PlayersList)
            {
                if (playerObj != null)
                {
                    Collider2D playerCol = playerObj.GetComponent<Collider2D>();
                    if (playerCol != null)
                    {
                        Physics2D.IgnoreCollision(bulletCol, playerCol);
                    }
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
            enemy.TakeDamage(damage, owner);
            Destroy(gameObject);
        }
    }
}
