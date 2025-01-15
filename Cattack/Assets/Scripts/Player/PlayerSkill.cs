using System.Collections;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    HollowPurple hollowPurple;
    Animator animator;

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

    [Header("Hollow Skill Settings")]
    public float hollowCooldown = 3f;
    private bool isHollowOnCooldown = false;
    private bool isHollowActive = false;
    public float hollowDuration = 4f;

    [Header("Meteor Skill Settings")]
    public float meteorCooldown = 3f;
    private bool isMeteorOnCooldown = false;
    private bool isMeteorActive = false;
    public float meteorDuration = 4f;
    


    [Header("Skill States")]
    public bool hairBallActivated = false;
    public bool meowActivated = false;
    public bool hollowActivated = false;
    public bool meteorActivated = false;

    private PlayerAnimationController playerAnim;
    private CatSkillFX catSkillFX;
    private CardData qSkill;
    private CardData wSkill;
    private CardData eSkill;

    private void Start()
    {
        hollowPurple = FindAnyObjectByType<HollowPurple>();
        catSkillFX = FindAnyObjectByType<CatSkillFX>();
        playerAnim = GetComponent<PlayerAnimationController>();
        baseAttackDamage = attackDamage;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        CheckInputs();
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
            case "Hollow":
                if (hollowActivated && !isHollowOnCooldown)
                {
                    CastHollow();
                }
                break;
            case "Meteor":
                if (meteorActivated && !isMeteorOnCooldown)
                {
                    CastMeteor();
                }
                break;

            default:
                Debug.Log($"Using skill: {skill.cardName}");
                break;
        }
    }

    //----------Hairball Skill----------    
    public void CastHairball()
    {
        PlayerAnimationController.Instance.SetPlayerHairball();
        catSkillFX.PlayHairballAnimation();
        StartCoroutine(HairballCooldownRoutine());
    }

    private IEnumerator HairballCooldownRoutine()
    {
        isHairballOnCooldown = true;
        yield return new WaitForSeconds(hairballCooldown);
        isHairballOnCooldown = false;
    }

    //-----------------Hollow Skill-----------------
    public void CastHollow()
    {
        PlayerAnimationController.Instance.SetPlayerHollow();
        catSkillFX.PlayHollowAnimation();
        StartCoroutine(HollowCooldownRoutine());
    }

    private IEnumerator HollowCooldownRoutine()
    {
        isHollowOnCooldown = true;
        yield return new WaitForSeconds(hollowCooldown);
        isHollowOnCooldown = false;
    }
    //-----------------Meteor Skill-----------------
    public void CastMeteor()
    {
        PlayerAnimationController.Instance.SetPlayerMeteor();
        catSkillFX.PlayMeteorAnimation();
        StartCoroutine(MeteorCooldownRoutine());
    }

    private IEnumerator MeteorCooldownRoutine()
    {
        isMeteorOnCooldown = true;
        yield return new WaitForSeconds(meteorCooldown);
        isMeteorOnCooldown = false;
    }
    //----------Meow Skill----------
    private void ActivateMeowSkill()
    {
        if (isMeowOnCooldown || isMeowActive) return;

        PlayerAnimationController.Instance.SetPlayerMeow();
        StartCoroutine(MeowEffectRoutine());
    }

    private IEnumerator MeowEffectRoutine()
    {
        isMeowActive = true;
        attackDamage = baseAttackDamage + (int)meowDamageBoost;
        Debug.Log($"Meow activated! Attack damage increased to {attackDamage}");

        yield return new WaitForSeconds(meowDuration);

        attackDamage = baseAttackDamage;
        isMeowActive = false;

        isMeowOnCooldown = true;
        Debug.Log("Meow effect ended, starting cooldown");

        yield return new WaitForSeconds(meowCooldown);

        isMeowOnCooldown = false;
        Debug.Log("Meow cooldown finished");
    }

    //----------Assign Skills----------
    private void AssignSkillToSlot(CardData card, ref CardData skillSlot)
    {
        skillSlot = card;
        UpdateSkillStates(card);
    }

    public void AssignQSkill(CardData card)
    {
        AssignSkillToSlot(card, ref qSkill);
        Debug.Log($"Q Skill assigned: {card.cardName}");
    }

    public void AssignWSkill(CardData card)
    {
        AssignSkillToSlot(card, ref wSkill);
        Debug.Log($"W Skill assigned: {card.cardName}");
    }

    public void AssignESkill(CardData card)
    {
        AssignSkillToSlot(card, ref eSkill);
        Debug.Log($"E Skill assigned: {card.cardName}");
    }

    //----------Update Skill States----------
    private void UpdateSkillStates(CardData card)
    {
        if (card == null) return;

        switch (card.cardName)
        {
            case "Hairball":
                hairBallActivated = true;
                break;
            case "Meow":
                meowActivated = true;
                break;
            case "Hollow":
                hollowActivated = true;
                break;
                case "Meteor":
                meteorActivated = true;
                break;
        }
    }

    //----------Attack----------
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