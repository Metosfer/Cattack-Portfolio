using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("Raycast Fall Detection")]
    public float raycastDistance = 1f;
    public LayerMask groundLayer;
    public float fallThreshold = 0.5f;
    public Color raycastColor = Color.red;

    [Header("Dash Settings")]
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    [Header("Component References")]
    private Rigidbody2D rb;
    private PlayerAnimationController animationController;
    [SerializeField] private TrailRenderer trailRenderer;

    [Header("Movement States")]
    private bool isGrounded;
    private bool canDash = true;
    private bool isDashing;
    private bool isFalling;
    public bool playerTouched = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponent<PlayerAnimationController>();
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        CheckFallState();
        HandleJump();
        HandleDash();
        HandleClaw();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        HandleMovement();
    }

    private void CheckFallState()
    {
        // Raycast aþaðý doðru
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            raycastDistance,
            groundLayer
        );

        // Düþme durumunu kontrol et
        if (hit.collider == null && rb.velocity.y < -fallThreshold)
        {
            if (!isFalling)
            {
                isFalling = true;
                animationController.SetFalling(true);
            }
        }
        else if (hit.collider != null)
        {
            if (isFalling)
            {
                isFalling = false;
                animationController.SetFalling(false);
            }
        }
    }

    // Gizmos çizimi için Editor'da görselleþtirme
    void OnDrawGizmosSelected()
    {
        // Raycast çizgisini çiz
        Gizmos.color = raycastColor;
        Vector3 direction = Vector3.down * raycastDistance;
        Gizmos.DrawRay(transform.position, direction);

        // Raycast çarpýþma noktasýný göster
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            raycastDistance,
            groundLayer
        );

        if (hit.collider != null)
        {
            // Çarpýþma noktasýný küçük bir küre ile iþaretle
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(hit.point, 0.1f);
        }
    }

    private void HandleClaw()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animationController.SetClaw();
        }
    }
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // Hareket yönünü belirleme
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }

        // Animasyon kontrolü
        animationController.SetRunning(Mathf.Abs(horizontalInput) > 0.1f);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            animationController.TriggerJump();
            animationController.SetGrounded(false);
        }
    }

    private void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        animationController.SetDashing(true);

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);

        trailRenderer.emitting = false;
        rb.gravityScale = originalGravity;

        animationController.SetDashing(false);
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Witch"))
        {
            Debug.Log("Player Cadýyla Temas Etti!!");
            playerTouched = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = true;
            isFalling = false;
            animationController.SetGrounded(true);
            animationController.SetFalling(false);
        }
        else
        {
            isGrounded = false;
            animationController.SetGrounded(false);
        }
    }
}
