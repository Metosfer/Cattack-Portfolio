using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;

    private Animator animator;

    //Dash------
    
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 24f;
    private float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    [SerializeField]private TrailRenderer trailRenderer;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (isDashing)
        {
            return;

        }
        Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            animator.SetBool("isDashing", true);
            StartCoroutine(Dash());
            

        }
    }


    void FixedUpdate()
    {
        if (isDashing)
        {
         return;
        
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Basic Movement
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // Face Direction
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }


    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
        {
            isGrounded = true;
        }
    }
    private void Jump()
    {
        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
        

    }
    private IEnumerator Dash()
    {
      
        
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        animator.SetBool("isDashing", false);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        
    }
}