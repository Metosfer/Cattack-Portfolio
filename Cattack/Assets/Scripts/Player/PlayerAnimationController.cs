using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public static PlayerAnimationController Instance { get; private set; }
    private Animator animator;
    public bool isAttacking = false;
    private bool isDead = false; // �l�m durumu kontrol� i�in

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            animator = GetComponent<Animator>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Animator bile�eninin varl���n� kontrol et
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            Debug.LogWarning("Animator Start'ta atand�, performans i�in Awake'te atanmas� �nerilir.");
        }
    }

    // Running Animation Control
    public void SetRunning(bool isRunning)
    {
        if (isDead) return;
        animator.SetBool("isRunning", isRunning);
    }

    // Jump Trigger
    public void TriggerJump()
    {
        if (isDead) return;
        animator.SetTrigger("isJumping");
    }

    // Grounded State Control
    public void SetGrounded(bool isGrounded)
    {
        if (isDead) return;
        animator.SetBool("catGrounded", isGrounded);
    }

    // Falling State Control
    public void SetFalling(bool isFalling)
    {
        if (isDead) return;
        animator.SetBool("isFalling", isFalling);
    }

    // Dash Animation Control
    public void SetDashing(bool isDashing)
    {
        if (isDead) return;
        animator.SetBool("isDashing", isDashing);
    }

    // Claw Animation Control
    public void SetClaw()
    {
        if (isDead) return;
        animator.SetTrigger("isClawing");
    }

    // Death Animation Control
    public void SetDeath(int isHealth)
    {
        if (isDead) return; // Zaten �l�yse tekrar tetikleme

        isDead = true;
        animator.SetInteger("isHealth", isHealth);

        // Debug i�in �l�m animasyonu parametresini kontrol et
        Debug.Log($"�l�m animasyonu tetiklendi. isHealth de�eri: {animator.GetInteger("isHealth")}");

        // Di�er animasyonlar� resetle
        animator.SetBool("isRunning", false);
        animator.SetBool("isDashing", false);
        animator.SetBool("isFalling", false);
        animator.ResetTrigger("isJumping");
        animator.ResetTrigger("isClawing");
    }
    
    ///////------------------SKILLS----------------/////////////
     //Hairball Control
     public void SetPlayerHairball()
    {
        animator.SetTrigger("catHairball");
    }
    

    // Animator parametrelerini kontrol etmek i�in yard�mc� metod
    public bool IsAnimationParameterValid(string parameterName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameterName)
                return true;
        }
        Debug.LogError($"Animator parameter '{parameterName}' bulunamad�!");
        return false;
    }
}