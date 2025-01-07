using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;


public class PlayerCombat : MonoBehaviour
{
    CardHolder cardHolder;

    
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    
    //------------------//
    //Skill Check//
    public bool hairBallActivated = false;
    public bool meowActivated = false;


    private PlayerAnimationController playerAnim;
    public int attackDamage = 5;


    public void Awake()
    {
        
        cardHolder = FindAnyObjectByType<CardHolder>();

        Debug.Log("Awake Kullanıldı");
    }
   public void Start()
    {
        Debug.Log("Start Kullanıldı");

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
    public void Update()
    {
        //seçilen yetenek numaralarının tutulması


        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }


    }

    public void SkillManager(int? skillNum)
    {                    
        switch (skillNum)
        {               
            case 10:
                {
                    Hairball();
                }                                  
                break;

            case 11:               
                {
                    Meow();
                }
                break;


            default:
                hairBallActivated = false;
                meowActivated = false;
            break;
        }
     }

    
   
    public void Hairball()
    {
        if (CardManager.Instance != null && hairBallActivated == true)
        {
            PlayerAnimationController.Instance.SetPlayerHairball();
            SkillAnimationController.Instance.SetHairballEffect();
        }
    }
    public void Meow()
    {
        if (CardManager.Instance != null && meowActivated == true)
        {
            SkillAnimationController.Instance.SetMeowEffect();
            PlayerAnimationController.Instance.SetPlayerMeow();

        }
    }





   public void Attack()
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
    

    public void OnDrawGizmos()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}