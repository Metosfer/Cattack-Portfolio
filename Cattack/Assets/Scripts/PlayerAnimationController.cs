using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Running Animation Control
    public void SetRunning(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    // Jump Trigger
    public void TriggerJump()
    {
        animator.SetTrigger("isJumping");
    }

    // Grounded State Control
    public void SetGrounded(bool isGrounded)
    {
        animator.SetBool("catGrounded", isGrounded);
    }

    // Falling State Control
    public void SetFalling(bool isFalling)
    {
        animator.SetBool("isFalling", isFalling);
    }

    // Dash Animation Control
    public void SetDashing(bool isDashing)
    {
        animator.SetBool("isDashing", isDashing);
    }

    // Claw Animation Control

    public void SetClaw()
    {
        animator.SetTrigger("isClawing");
    }
}