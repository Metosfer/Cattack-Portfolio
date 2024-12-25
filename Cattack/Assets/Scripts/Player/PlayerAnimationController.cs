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
            DontDestroyOnLoad(gameObject); // Bu sat�r eklendi
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
                Debug.LogError("Animator bile�eni bulunamad�!");
            }
            else
            {
                Debug.LogWarning("Animator Start'ta atand�, performans i�in Awake'te atanmas� �nerilir.");
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

    public void SetDeath(int isHealth)
    {
        if (isDead) return; // Zaten �l�yse tekrar tetikleme

        isDead = true;
        animator.SetInteger("isHealth", isHealth);

        Debug.Log($"�l�m animasyonu tetiklendi. isHealth de�eri: {animator.GetInteger("isHealth")}");

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
        Debug.LogError($"Animator parameter '{parameterName}' bulunamad�!");
        return false;
    }
}
