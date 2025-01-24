using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WitchAI : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 7f;
    public float attackCooldown = 3f;
    public float spellSpeed = 7f;

    [Header("Skill Ranges")]
    [SerializeField] private SkillRange[] skillRanges;

    [Header("References")]
    public GameObject lightningBoltPrefab;
    public Transform spellSpawnPoint;
    public GameObject targetObject;

    [Header("Stats")]
    public int maxHealth = 20;
    private float lastAttackTime = 0f;
    private bool isDead = false;
    private int currentHealth;
    private WitchAnimationController witchAnimations;
    private bool isAttacking = false;
    private bool isFacingRight = true;
    private Dictionary<string, float> skillCooldowns = new Dictionary<string, float>();

    private SkillRange currentSkill;

    public bool IsFacingRight => isFacingRight;

    [System.Serializable]
    public class SkillRange
    {
        public string skillName;
        public float minDistance;
        public float maxDistance;
        public float cooldown;
        public string animationTrigger;
        public bool requiresFXAnimation;
        public bool requiresPrefab;
        public int damageAmount;
        public bool isKnockback;
        public float damageDelay;
    }

    private void Start()
    {
        InitializeComponents();
        ValidateSetup();
        InitializeSkillCooldowns();
    }

    private void InitializeComponents()
    {
        witchAnimations = GetComponent<WitchAnimationController>();
        currentHealth = maxHealth;
    }

    private void InitializeSkillCooldowns()
    {
        // Her skill için cooldown dictionary'sini baþlat
        foreach (var skill in skillRanges)
        {
            skillCooldowns[skill.animationTrigger] = 0f;
        }
    }

    private bool IsSkillOnCooldown(string skillTrigger)
    {
        return Time.time < skillCooldowns[skillTrigger];
    }

    private void SetSkillCooldown(string skillTrigger, float cooldown)
    {
        skillCooldowns[skillTrigger] = Time.time + cooldown;
    }

    private void ValidateSetup()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object atanmamýþ! Lütfen inspector'dan target object'i atayýn.");
        }

        if (skillRanges == null || skillRanges.Length == 0)
        {
            Debug.LogError("Skill ranges tanýmlanmamýþ! Lütfen inspector'dan skill range'leri ayarlayýn.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentSkill != null && other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(currentSkill.damageAmount);

                if (currentSkill.isKnockback)
                {
                    ApplyKnockback(other.gameObject);
                }
            }
        }
    }

    private void Update()
    {
        if (ShouldSkipUpdate()) return;

        UpdateFacing();
        CheckAndExecuteAttack();
    }

    private bool ShouldSkipUpdate()
    {
        return isDead ||
               TimeManager.Instance.GetCurrentDay() < TimeManager.Instance.GetBossDay() ||
               targetObject == null ||
               isAttacking;
    }

    private void UpdateFacing()
    {
        Vector3 direction = targetObject.transform.position - transform.position;
        isFacingRight = direction.x > 0;

        transform.rotation = Quaternion.Euler(0, isFacingRight ? 0 : 180, 0);
    }

    private void CheckAndExecuteAttack()
    {
        float targetDistance = Vector2.Distance(transform.position, targetObject.transform.position);
        if (targetDistance <= attackRange)
        {
            ExecuteAppropriateSkill(targetDistance);
        }
    }

    private void ExecuteAppropriateSkill(float targetDistance)
    {
        foreach (var skillRange in skillRanges)
        {
            if (IsWithinRange(targetDistance, skillRange.minDistance, skillRange.maxDistance) &&
                !IsSkillOnCooldown(skillRange.animationTrigger))
            {
                StartCoroutine(ExecuteSkill(skillRange));
                break;
            }
        }
    }

    private bool IsWithinRange(float distance, float min, float max)
    {
        return distance > min && distance <= max;
    }

    private IEnumerator ExecuteSkill(SkillRange skill)
    {
        if (isAttacking || IsSkillOnCooldown(skill.animationTrigger)) yield break;

        isAttacking = true;
        currentSkill = skill;
        SetSkillCooldown(skill.animationTrigger, skill.cooldown);

        // Animasyon kontrolü
        switch (skill.animationTrigger)
        {
            case "witchAttack1":
                witchAnimations.SetPlayerAttack1();
                if (skill.requiresPrefab)
                {
                    Instantiate(lightningBoltPrefab, spellSpawnPoint.position, Quaternion.identity);
                }
                break;
            case "witchAttack2":
                witchAnimations.SetPlayerAttack2();
                break;
            case "witchAttack3":
                witchAnimations.SetPlayerAttack3();
                break;
            case "witchAttack4":
                witchAnimations.SetPlayerAttack4();
                break;
        }

        // Hasar verme gecikmesi
        yield return new WaitForSeconds(skill.damageDelay);

        // Hasar verme iþlemi
        if (currentSkill != null && targetObject != null)
        {
            PlayerHealth playerHealth = targetObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(currentSkill.damageAmount);

                if (currentSkill.isKnockback)
                {
                    ApplyKnockback(targetObject);
                }
            }
        }

        // Animasyon süresini bekle
        yield return new WaitForSeconds(skill.cooldown - skill.damageDelay);

        currentSkill = null;
        witchAnimations.SetPlayerSkillFinished();
        isAttacking = false;
    }

    private void ApplyKnockback(GameObject target)
    {
        Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
        PlayerAnimationController playerAnimations = target.GetComponent<PlayerAnimationController>();

        if (targetRb != null)
        {
            Vector2 knockbackDirection = (target.transform.position - transform.position).normalized;
            targetRb.AddForce(knockbackDirection * 10f, ForceMode2D.Impulse);

            if (playerAnimations != null)
            {
                playerAnimations.SetKnockbackState(true);
            }

            StartCoroutine(ResetPlayerAnimationAfterKnockback(target, playerAnimations));
        }
    }

    private IEnumerator ResetPlayerAnimationAfterKnockback(GameObject target, PlayerAnimationController playerAnimations)
    {
        yield return new WaitForSeconds(0.5f); // Knockback süresi

        if (playerAnimations != null)
        {
            playerAnimations.SetKnockbackState(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // AttackStartDay kontrolü
        if (TimeManager.Instance.GetCurrentDay() < TimeManager.Instance.GetBossDay()) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        if (witchAnimations != null && witchAnimations.witchAnimator != null)
        {
            witchAnimations.witchAnimator.SetTrigger("Die");
        }

        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
