using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //public enum SkillSlot
    //    {

    //    Q,W,E

    //    }
    //private Dictionary<SkillSlot, int> skillMappings = new Dictionary<SkillSlot, int>();



    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    
    //------------------//
    //Skill Check//
    public bool hairBallActivated = false;
    public bool meowActivated = false;


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
    //public void AssignSkill(SkillSlot slot, int cardIndex)
    //{
    //    if (!skillMappings.ContainsKey(slot))
    //    {
    //        skillMappings[slot] = cardIndex;
    //        Debug.Log($"{slot} slotuna {cardIndex} indeksi atandı.");
    //    }
    //    else
    //    {
    //        Debug.LogError($"{slot} slotuna zaten bir skill atanmış.");
    //    }
    //}
    public void CardQ()
    {
        
        
        if (TimeManager.Instance.currentDay == 1 )
        {
            
        switch (CardManager.Instance.attackCardIndex)
        {
                
            case 1:
                hairBallActivated = true;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Hairball();
                }

                break;

            case 2:
                meowActivated = true;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Meow();
                }
                
                break;
            case 3:
                Debug.Log("3.Kart seçildi");
                break;

            default:
                hairBallActivated = false;
                meowActivated = false;


                break;
        }
        }

    }
    public void CardW()
    {
        if (TimeManager.Instance.currentDay == 2)
        {
            switch (CardManager.Instance.attackCardIndex)
            {
                case 1:
                    hairBallActivated = true;
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        Hairball();
                    }

                    break;

                case 2:
                    meowActivated = true;
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        Meow();
                    }

                    break;
                case 3:
                    Debug.Log("3.Kart seçildi");
                    break;

                default:
                    hairBallActivated = false;
                    meowActivated = false;


                    break;
            }
        }

    }
    public void CardE()
    {
        if (TimeManager.Instance.currentDay == 3)
        {
            switch (CardManager.Instance.attackCardIndex)
            {
                case 1:
                    hairBallActivated = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Hairball();
                    }

                    break;

                case 2:
                    meowActivated = true;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Meow();
                    }

                    break;
                case 3:
                    Debug.Log("3.Kart seçildi");
                    break;

                default:
                    hairBallActivated = false;
                    meowActivated = false;


                    break;
            }
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

    void Update()
    {
        CardQ();
        CardW();
        CardE();

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
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