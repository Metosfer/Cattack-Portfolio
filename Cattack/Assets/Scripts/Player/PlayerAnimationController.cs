using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public static PlayerAnimationController Instance { get; private set; }
    private Animator animator;
    public bool isAttacking = false;
    private bool isDead = false; // Ölüm durumu kontrolü için

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
        // Animator bileþeninin varlýðýný kontrol et
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            Debug.LogWarning("Animator Start'ta atandý, performans için Awake'te atanmasý önerilir.");
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
        if (isDead) return; // Zaten ölüyse tekrar tetikleme

        isDead = true;
        animator.SetInteger("isHealth", isHealth);

        // Debug için ölüm animasyonu parametresini kontrol et
        Debug.Log($"Ölüm animasyonu tetiklendi. isHealth deðeri: {animator.GetInteger("isHealth")}");

        // Diðer animasyonlarý resetle
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
    

    // Animator parametrelerini kontrol etmek için yardýmcý metod
    public bool IsAnimationParameterValid(string parameterName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameterName)
                return true;
        }
        Debug.LogError($"Animator parameter '{parameterName}' bulunamadý!");
        return false;
    }
}