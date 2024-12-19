using System.Collections;
using System.Collections.Generic;
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
    }

    public void Hairball()
    {
        if (CardManager.Instance.attackCardIndex == 0)
        {
           
                PlayerAnimationController.Instance.SetPlayerHairball();
                SkillAnimationController.Instance.SetHairballEffect();

        }

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Hairball();
        }

        if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    void Attack()
    {
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

        if(attackPoint == null)
        { 
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
