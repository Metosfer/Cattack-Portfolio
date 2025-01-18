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
    private void SetAnimatorTrigger(string paramName)
    {
        if (isDead) return;
        if (IsAnimationParameterValid(paramName))
        {
            // Önce diğer skill triggerlarını resetle
            ResetAllSkillTriggers();
            animator.SetTrigger(paramName);
        }
    }
    private void ResetAllSkillTriggers()
    {
        animator.ResetTrigger("catHairball");
        animator.ResetTrigger("catMeow");
        animator.ResetTrigger("catHollow");
        animator.ResetTrigger("catMeteor");
        animator.ResetTrigger("catCurse");
        animator.ResetTrigger("catHiss");
    }
    public void ReturnToIdleState()
    {
        // Tüm trigger'ları resetle
        animator.ResetTrigger("catHairball");
        animator.ResetTrigger("catMeow");
        animator.ResetTrigger("catHollow");
        animator.ResetTrigger("catMeteor");
        animator.ResetTrigger("catCurse");
        animator.ResetTrigger("isClawing");
        animator.ResetTrigger("catHiss");

        // Karakter hareket ediyorsa running, etmiyorsa idle state'e dön
        bool isMoving = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f;
        SetRunning(isMoving);
    }
    private void SetAnimatorBool(string paramName, bool value)
    {
        if (isDead) return;
        if (IsAnimationParameterValid(paramName))
        {
            animator.SetBool(paramName, value);
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
    public void SetPlayerCurse() => SetAnimatorTrigger("catCurse");
    public void SetPlayerHiss() => SetAnimatorTrigger("catHiss");

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
        animator.ResetTrigger("catHairball");
        animator.ResetTrigger("catMeow");
        animator.ResetTrigger("catHiss");
        animator.ResetTrigger("catHollow");
        animator.ResetTrigger("catMeteor");
        animator.ResetTrigger("catCurse");
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