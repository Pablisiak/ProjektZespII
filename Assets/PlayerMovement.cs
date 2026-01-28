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
        if (player != null && (player.IsDead || player.IsHurt))
        {
            moveInput = Vector2.zero;
            HandleAnimation();
            return;
        }

        moveInput = moveInput.normalized;

        HandleAnimation();
        HandleFlip();
    }


    void FixedUpdate()
    {
        if (player != null && (player.IsDead || player.IsHurt))
            return;

        float speed = 5 * ((float)(player.Stats.Speed + 100) / 100);
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
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
