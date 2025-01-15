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
            DontDestroyOnLoad(gameObject); // Bu satýr eklendi
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator bileþeni bulunamadý!");
            }
            else
            {
                Debug.LogWarning("Animator Start'ta atandý, performans için Awake'te atanmasý önerilir.");
            }
        }
    }

    private void SetAnimatorBool(string paramName, bool value)
    {
        if (isDead) return;
        if (IsAnimationParameterValid(paramName))
        {
            animator.SetBool(paramName, value);
        }
    }

    private void SetAnimatorTrigger(string paramName)
    {
        if (isDead) return;
        if (IsAnimationParameterValid(paramName))
        {
            animator.SetTrigger(paramName);
        }
    }

    public void SetRunning(bool isRunning) => SetAnimatorBool("isRunning", isRunning);
    public void TriggerJump() => SetAnimatorTrigger("isJumping");
    public void SetGrounded(bool isGrounded) => SetAnimatorBool("catGrounded", isGrounded);
    public void SetFalling(bool isFalling) => SetAnimatorBool("isFalling", isFalling);
    public void SetDashing(bool isDashing) => SetAnimatorBool("isDashing", isDashing);
    public void SetClaw() => SetAnimatorTrigger("isClawing");
    public void SetPlayerHairball() => SetAnimatorTrigger("catHairball");
    public void SetPlayerMeow() => SetAnimatorTrigger("catMeow");
    public void SetPlayerHollow() => SetAnimatorTrigger("catHollow");
    public void SetPlayerMeteor() => SetAnimatorTrigger("catMeteor");

    public void SetDeath(int isHealth)
    {
        if (isDead) return; // Zaten ölüyse tekrar tetikleme

        isDead = true;
        animator.SetInteger("isHealth", isHealth);

        Debug.Log($"Ölüm animasyonu tetiklendi. isHealth deðeri: {animator.GetInteger("isHealth")}");

        animator.SetBool("isRunning", false);
        animator.SetBool("isDashing", false);
        animator.SetBool("isFalling", false);
        animator.ResetTrigger("isJumping");
        animator.ResetTrigger("isClawing");
    }

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