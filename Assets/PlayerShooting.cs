using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public Transform firePoint;

    public Player Player;

    [Header("Szybkostrzelność")]
    public float fireRate = 0.25f; 
    private float nextFireTime = 0f;

    void Update()
    {
        fireRate = Player.Weapon.FireRate / ((float)(Player.Stats.AttackSpeed + 100) / 100);
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        

        bullet.GetComponent<Bullet>().damage = Player.Weapon.DMG(Player.Stats);

        float Crite = 1;
        int Rng = Random.Range(0,100);
        if(Rng < Player.Stats.CritChance)
        {
            Crite = Player.Weapon.CritDamage;
            bullet.GetComponent<Bullet>().crite = true;
            float dmg = (float)bullet.GetComponent<Bullet>().damage * Crite;;
            bullet.GetComponent<Bullet>().damage = (int)dmg;
        }
        bullet.GetComponent<Bullet>().lifetime *=  ((float)(Player.Stats.Range + 100) / 100);

        bullet.transform.right = direction;
    }
}
