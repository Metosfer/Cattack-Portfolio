using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    private PlayerAnimationController playerAnim;
    public int attackDamage = 5;

    void Start()
    {
        playerAnim = GetComponent<PlayerAnimationController>();
        if (playerAnim == null)
        {
            Debug.LogError("PlayerAnimationController bileþeni bulunamadý!");
        }

        if (attackPoint == null)
        {
            Debug.LogError("Attack Point atanmamýþ!");
        }
    }

    public void Hairball()
    {
        if (CardManager.Instance != null && CardManager.Instance.attackCardIndex == 1)
        {
            PlayerAnimationController.Instance.SetPlayerHairball();
            SkillAnimationController.Instance.SetHairballEffect();
        }
    }
    public void Meow()
    {
        if (CardManager.Instance != null && CardManager.Instance.attackCardIndex == 2)
        {
            SkillAnimationController.Instance.SetMeowEffect();
            PlayerAnimationController.Instance.SetPlayerMeow();

        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Hairball();

        }

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Meow();
        }
    }



    void Attack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("Attack Point atanmamýþ!");
            return;
        }

        playerAnim.SetClaw();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Her düþman türü için güvenli kontrol
            SkeletonAI skeletonAI = enemy.GetComponent<SkeletonAI>();
            SkeletonMageAI skeletonMageAI = enemy.GetComponent<SkeletonMageAI>();

            if (skeletonAI != null)
            {
                skeletonAI.TakeDamage(attackDamage);
            }

            if (skeletonMageAI != null)
            {
                skeletonMageAI.TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}