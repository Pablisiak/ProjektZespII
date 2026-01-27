using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 2f;
    public int maxHealth = 100;
    public int healthPerRound;
    public int damage = 1;
    public int money = 1;

    [Header("Rush ability")]
    public bool enemyRush = false;
    public float rushMultiplier = 8f;
    public float rushDuration = 1f;
    public float rushCooldown = 5f;

    private int currentHealth;
    private Transform player;

    private float baseSpeed;
    private float rushCooldownTimer;
    private float rushDurationTimer;
    private bool isRushing;
    private Vector2 rushDirection;
    private Animator anim;
    private bool isDead = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        maxHealth += healthPerRound * WaveManager.currentWaveIndex;
        currentHealth = maxHealth;

        baseSpeed = speed;
        rushCooldownTimer = rushCooldown;
    }

    void Update()
    {
        if (isRushing)
        {
            RushMove();
            return;
        }

        player = GetNearestPlayer();
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        transform.position += (Vector3)direction * speed * Time.deltaTime;

        HandleRushCooldown(direction);
    }


    void HandleRushCooldown(Vector2 currentDirection)
    {
        if (!enemyRush) return;

        rushCooldownTimer -= Time.deltaTime;

        if (rushCooldownTimer <= 0f)
        {
            StartRush(currentDirection);
        }
    }

    void StartRush(Vector2 direction)
    {
        isRushing = true;
        rushDirection = direction; 
        speed = baseSpeed * rushMultiplier;
        rushDurationTimer = rushDuration;
        rushCooldownTimer = rushCooldown;
    }

    void RushMove()
    {
        transform.position += (Vector3)rushDirection * speed * Time.deltaTime;

        rushDurationTimer -= Time.deltaTime;
        if (rushDurationTimer <= 0f)
        {
            EndRush();
        }
    }

    void EndRush()
    {
        isRushing = false;
        speed = baseSpeed;
    }

    Transform GetNearestPlayer()
    {
        if (Players.players == null || Players.players.PlayersList.Count == 0)
            return null;

        Transform nearest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject p in Players.players.PlayersList)
        {
            if (p == null) continue;
            if (!p.activeInHierarchy) continue;

            Player pl = p.GetComponent<Player>();
            if (pl == null) continue;

            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                nearest = p.transform;
            }
        }

        return nearest;
    }

    public void TakeDamage(int amount, Player playerWhoShot = null)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die(playerWhoShot);
        }
    }

    void Die(Player playerWhoShot)
    {
        if (playerWhoShot != null)
            playerWhoShot.Money += money;

    }

    void ShowText(string tekst)
    {
        GameObject pop = Instantiate(
            PrefabMenager.prefabMenager.PopUp,
            transform.position,
            Quaternion.identity
        );

        pop.GetComponent<PopUp>().Set(tekst);
        Destroy(pop, 1f);
    }

    void ShowText(string tekst, Color kolor)
    {
        GameObject pop = Instantiate(
            PrefabMenager.prefabMenager.PopUp,
            transform.position,
            Quaternion.identity
        );

        PopUp p = pop.GetComponent<PopUp>();
        p.Set(tekst);
        p.SetColor(kolor);
        Destroy(pop, 1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(isDead) return;

            Player playerHealth = collision.gameObject.GetComponent<Player>();
            if (playerHealth != null)
            {
                anim.ResetTrigger("Attack");
                anim.SetTrigger("Attack");
                playerHealth.TakeDamage(damage);
            }
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {

            if(isDead) return;
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                if (bullet.crite)
                    ShowText(bullet.damage.ToString(), Color.yellow);
                else
                    ShowText(bullet.damage.ToString());

                currentHealth -= bullet.damage;

                if(currentHealth > 0) {
                    anim.SetTrigger("Hurt");
                }
                else {
                    isDead = true;
                    anim.SetBool("Dead", true);

                    if (bullet.owner != null)
                        bullet.owner.Money += money;
                }
              Destroy(collision.gameObject);
            }
        }
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }
}
