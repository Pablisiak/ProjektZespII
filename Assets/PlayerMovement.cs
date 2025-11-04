using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ustawienia ruchu")]            
    private Rigidbody2D rb;                  
    private Vector2 moveInput;      

    public Player player;         

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput = moveInput.normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * (5 * ((float)(player.Stats.Speed + 100) / 100)) * Time.fixedDeltaTime);
    }
}
