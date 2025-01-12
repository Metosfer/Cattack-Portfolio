using System.Collections;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 5;

    [Header("Meow Skill Settings")]
    public float meowDamageBoost = 5f;
    public float meowDuration = 5f;
    public float meowCooldown = 5f;
    private bool isMeowOnCooldown = false;
    private bool isMeowActive = false;
    private int baseAttackDamage;

    [Header("Hairball Skill Settings")]
    
    public float hairballCooldown = 3f;
    private bool isHairballOnCooldown = false;

    [Header("Skill States")]
    public bool hairBallActivated = false;
    public bool meowActivated = false;

    private PlayerAnimationController playerAnim;
    private CardData qSkill;
    private CardData wSkill;
    private CardData eSkill;

    private void Start()
    {
        playerAnim = GetComponent<PlayerAnimationController>();
        baseAttackDamage = attackDamage;
    }

    private void Update()
    {
        CheckInputs();
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void CheckInputs()
    {
        if (qSkill != null && Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill(qSkill);
        }
        if (wSkill != null && Input.GetKeyDown(KeyCode.W))
        {
            UseSkill(wSkill);
        }
        if (eSkill != null && Input.GetKeyDown(KeyCode.E))
        {
            UseSkill(eSkill);
        }
    }

    private void UseSkill(CardData skill)
    {
        if (skill == null) return;

        switch (skill.cardName)
        {
            case "Hairball":
                if (hairBallActivated && !isHairballOnCooldown)
                {
                    CastHairball();
                }
                break;

            case "Meow":
                if (meowActivated && !isMeowOnCooldown)
                {
                    ActivateMeowSkill();
                }
                break;

            default:
                Debug.Log($"Using skill: {skill.cardName}");
                break;
        }
    }

    private void CastHairball()
    {


        PlayerAnimationController.Instance.SetPlayerHairball();



        StartCoroutine(HairballCooldownRoutine());
    }

    private IEnumerator HairballCooldownRoutine()
    {
        isHairballOnCooldown = true;
        yield return new WaitForSeconds(hairballCooldown);
        isHairballOnCooldown = false;
    }

    private void ActivateMeowSkill()
    {
        if (isMeowOnCooldown || isMeowActive) return;

        PlayerAnimationController.Instance.SetPlayerMeow();
        StartCoroutine(MeowEffectRoutine());
    }

    private IEnumerator MeowEffectRoutine()
    {
        // Activate meow effect
        isMeowActive = true;
        attackDamage = baseAttackDamage + (int)meowDamageBoost;
        Debug.Log($"Meow activated! Attack damage increased to {attackDamage}");

        // Wait for duration
        yield return new WaitForSeconds(meowDuration);

        // Reset damage
        attackDamage = baseAttackDamage;
        isMeowActive = false;

        // Start cooldown
        isMeowOnCooldown = true;
        Debug.Log("Meow effect ended, starting cooldown");

        yield return new WaitForSeconds(meowCooldown);

        // Reset cooldown
        isMeowOnCooldown = false;
        Debug.Log("Meow cooldown finished");
    }

    public void AssignQSkill(CardData card)
    {
        qSkill = card;
        UpdateSkillStates(card);
        Debug.Log($"Q Skill assigned: {card.cardName}");
    }

    public void AssignWSkill(CardData card)
    {
        wSkill = card;
        UpdateSkillStates(card);
        Debug.Log($"W Skill assigned: {card.cardName}");
    }

    public void AssignESkill(CardData card)
    {
        eSkill = card;
        UpdateSkillStates(card);
        Debug.Log($"E Skill assigned: {card.cardName}");
    }

    private void UpdateSkillStates(CardData card)
    {
        if (card == null) return;

        switch (card.cardName)
        {
            case "Hairball":
                hairBallActivated = true;
                meowActivated = false;
                break;
            case "Meow":
                meowActivated = true;
                hairBallActivated = false;
                break;
            default:
                hairBallActivated = false;
                meowActivated = false;
                break;
        }
    }

    public void Attack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("Attack Point is not assigned!");
            return;
        }

        playerAnim.SetClaw();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Check for each enemy type
            SkeletonAI skeletonAI = enemy.GetComponent<SkeletonAI>();
            SkeletonMageAI skeletonMageAI = enemy.GetComponent<SkeletonMageAI>();
            SkeletonKnightAI skeletonKnightAI = enemy.GetComponent<SkeletonKnightAI>();

            if (skeletonAI != null)
            {
                skeletonAI.TakeDamage(attackDamage);
            }
            if (skeletonMageAI != null)
            {
                skeletonMageAI.TakeDamage(attackDamage);
            }
            if (skeletonKnightAI != null)
            {
                skeletonKnightAI.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
