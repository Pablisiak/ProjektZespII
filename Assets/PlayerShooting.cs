using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public Transform firePoint;
    public Player Player;

    [Header("Szybkostrzelność")]
    public float fireRate = 0.25f;
    private float nextFireTime = 0f;

    [Header("Auto-Target")]
    public float Range = 10f; // maksymalny zasięg strzału
    private GameObject currentTarget;

    void Update()
    {
        fireRate = Player.Weapon.FireRate / ((float)(Player.Stats.AttackSpeed + 100) / 100);

        FindNearestEnemy();

        if (currentTarget != null && Time.time >= nextFireTime)
        {
            float distance = Vector2.Distance(transform.position, currentTarget.transform.position);
            if (distance <= Range * ((float)(Player.Stats.Range + 100) / 100))
            {
                ShootAt(currentTarget.transform);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void FindNearestEnemy()
    {
        List<GameObject> enemies = WaveManager.waveManager.activeEnemies;

        if (enemies == null || enemies.Count == 0)
        {
            currentTarget = null;
            return;
        }

        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        if (nearest != null && minDist <= Range * ((float)(Player.Stats.Range + 100) / 100))
            currentTarget = nearest;
        else
            currentTarget = null;
    }

    void ShootAt(Transform target)
    {
        Vector2 direction = (target.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        // Przypisanie strzału do danego gracza, aby wiedzieć kto ma otrzymać siano za killa
        Bullet bulletComp = bullet.GetComponent<Bullet>();
        bulletComp.owner = Player;

        bullet.GetComponent<Bullet>().damage = Player.Weapon.DMG(Player.Stats);

        // Krytyk
        float Crite = 1;
        int Rng = Random.Range(0, 100);
        if (Rng < Player.Stats.CritChance)
        {
            Crite = Player.Weapon.CritDamage;
            bullet.GetComponent<Bullet>().crite = true;
            float dmg = (float)bullet.GetComponent<Bullet>().damage * Crite;
            bullet.GetComponent<Bullet>().damage = (int)dmg;
        }

        bullet.GetComponent<Bullet>().lifetime *= ((float)(Player.Stats.Range + 100) / 100);
        bullet.transform.right = direction;
    }
}
