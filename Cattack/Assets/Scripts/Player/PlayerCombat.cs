using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;


public class PlayerCombat : MonoBehaviour
{
    CardHolder cardHolder;

    Dictionary<char, int> skillPlacement = new Dictionary<char, int>();
    
    
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int secilenKartPC;
    
    //------------------//
    //Skill Check//
    public bool hairBallActivated = false;
    public bool meowActivated = false;


    private PlayerAnimationController playerAnim;
    public int attackDamage = 5;


    public void Awake()
    {
        
        //cardHolder = FindAnyObjectByType<CardHolder>();
        

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

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (skillPlacement.TryGetValue('Q', out int qSkill))
            {
                SkillManager(qSkill);
            }
            else
            {
                Debug.LogError("Key 'Q' is not found in the dictionary.");
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (skillPlacement.TryGetValue('W', out int wSkill))
            {
                SkillManager(wSkill);
            }
            else
            {
                Debug.LogError("Key 'W' is not found in the dictionary.");
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (skillPlacement.TryGetValue('E', out int eSkill))
            {
                SkillManager(eSkill);
            }
            else
            {
                Debug.LogError("Key 'E' is not found in the dictionary.");
            }
        }

    }

    public void SkillImplement()
    {
        try
        {
            cardHolder = FindObjectOfType<CardHolder>();
            secilenKartPC = cardHolder.secilenKart;

            Debug.Log(cardHolder.secilenKart);
            if (secilenKartPC != 9)
            {
                if (TimeManager.Instance.currentDay == 1)
                {
                    skillPlacement.Add('Q', secilenKartPC);
                }
                if (TimeManager.Instance.currentDay == 2)
                {
                    skillPlacement.Add('W', secilenKartPC);
                }
                if (TimeManager.Instance.currentDay == 3)
                {
                    skillPlacement.Add('E', secilenKartPC);
                }
                Debug.Log(skillPlacement['Q']);
            }
        }
        catch { }
    }

    public void SkillManager(int skillNum)
    {                    
        switch (skillNum)
        {               
            case 0:
                {
                    Hairball();
                }                                  
                break;

            case 1:               
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
            SkeletonKnightAI skeletonKnightAI = enemy.GetComponent<SkeletonKnightAI>();

            if (skeletonAI != null)
            {
                skeletonAI.TakeDamage(attackDamage);
            }

            if (skeletonMageAI != null)
            {
                skeletonMageAI.TakeDamage(attackDamage);
            }
            if(skeletonKnightAI != null)
            {
                skeletonKnightAI.TakeDamage(attackDamage);  
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