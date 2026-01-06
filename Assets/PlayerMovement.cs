using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    private Vector2 moveInput;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    public Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        moveInput = moveInput.normalized;

        HandleAnimation();
        HandleFlip();
    }

    private void HandleFlip()
    {
        if (moveInput.x > 0) // Porusza się w prawo
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0) // Porusza się w lewo
        {
            spriteRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        float speed = 5 * ((float)(player.Stats.Speed + 100) / 100);
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }

    private void HandleAnimation()
    {
        float moveMagnitude = moveInput.magnitude;
        anim.SetFloat("Speed", moveMagnitude);
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
}
