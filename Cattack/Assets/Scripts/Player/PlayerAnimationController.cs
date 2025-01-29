using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    public static PlayerAnimationController Instance { get; private set; }
    private Animator animator;
    public bool isAttacking = false;
    private bool isDead = false;
    private Coroutine currentAnimationCoroutine;
    private bool isPlayingHairball = false;
    private bool isKnockback = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            animator = GetComponent<Animator>();
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Animator parametrelerini kontrol et ve log'la
        Debug.Log("Mevcut Animator parametreleri:");
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            Debug.Log($"Parametre adı: {param.name}, Tipi: {param.type}");
        }
    }

    private void Update()
    {
        // Hairball animasyonu oynatılmıyorsa ve knockback durumu yoksa hareket kontrolü yap
        if (!isPlayingHairball && !isKnockback)
        {
            CheckMovement();
        }
    }

    private void CheckMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;

        // Animator'da "Run" parametresi varsa onu kullan, yoksa "isRunning" kullan
        if (IsAnimationParameterValid("Run"))
        {
            animator.SetBool("Run", isMoving);
        }
        else if (IsAnimationParameterValid("isRunning"))
        {
            animator.SetBool("isRunning", isMoving);
        }
    }

    private void SetAnimatorTrigger(string paramName)
    {
        if (isDead) return;
        if (IsAnimationParameterValid(paramName))
        {
            // Önceki animation coroutine'i varsa durdur
            if (currentAnimationCoroutine != null)
            {
                StopCoroutine(currentAnimationCoroutine);
            }

            // Yeni coroutine başlat
            currentAnimationCoroutine = StartCoroutine(PlayAnimationWithReset(paramName));
        }
    }

    private IEnumerator PlayAnimationWithReset(string paramName)
    {
        // Hairball animasyonu için özel kontrol
        bool isHairball = paramName == "catHairball";
        if (isHairball)
        {
            isPlayingHairball = true;
        }

        // Önce diğer skill triggerlarını resetle
        ResetAllSkillTriggers();

        // Yeni animasyonu tetikle
        animator.SetTrigger(paramName);

        // Animator'daki state bilgisini al
        yield return new WaitForSeconds(0.1f); // State geçişinin gerçekleşmesi için kısa bir bekleme
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Animasyonun bitmesini bekle
        yield return new WaitForSeconds(stateInfo.length);

        // Hairball animasyonu bittiyse flag'i resetle
        if (isHairball)
        {
            isPlayingHairball = false;
        }

        // Animasyon bittiğinde idle state'e dön
        ReturnToIdleState();
    }

    private void ResetAllSkillTriggers()
    {
        animator.ResetTrigger("catHairball");
        animator.ResetTrigger("catMeow");
        animator.ResetTrigger("catHollow");
        animator.ResetTrigger("catMeteor");
        animator.ResetTrigger("catCurse");
        animator.ResetTrigger("catHiss");
        animator.ResetTrigger("isClawing");
    }

    public void ReturnToIdleState()
    {
        ResetAllSkillTriggers();
        CheckMovement(); // Hareket durumunu kontrol et
    }

    private void SetAnimatorBool(string paramName, bool value)
    {
        if (isDead) return;
        if (IsAnimationParameterValid(paramName))
        {
            animator.SetBool(paramName, value);
        }
        else
        {
            Debug.LogWarning($"'{paramName}' parametresi bulunamadı. Alternatif parametre aranıyor...");
            // Alternatif parametre isimlerini kontrol et
            if (paramName == "isRunning" && IsAnimationParameterValid("Run"))
            {
                animator.SetBool("Run", value);
            }
        }
    }

    public bool IsAnimationParameterValid(string parameterName)
    {
        if (animator == null) return false;

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameterName)
                return true;
        }
        return false;
    }

    // Public metodlar
    public void SetRunning(bool isRunning)
    {
        if (!isPlayingHairball && !isKnockback) // Hairball animasyonu oynatılmıyorsa ve knockback durumu yoksa koşma durumunu güncelle
        {
            SetAnimatorBool("Run", isRunning); // Önce "Run" parametresini dene
            if (!IsAnimationParameterValid("Run"))
            {
                SetAnimatorBool("isRunning", isRunning); // "Run" yoksa "isRunning" parametresini dene
            }
        }
    }

    // Diğer public metodlar aynı kalacak
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
        if (isDead) return;

        isDead = true;
        animator.SetInteger("isHealth", isHealth);

        Debug.Log($"Ölüm animasyonu tetiklendi. isHealth değeri: {animator.GetInteger("isHealth")}");

        ResetAllSkillTriggers();
        SetRunning(false);
        SetDashing(false);
        SetFalling(false);
    }

    public void SetKnockbackState(bool state)
    {
        isKnockback = state;
        animator.SetBool("IsKnockback", isKnockback);
    }
}
