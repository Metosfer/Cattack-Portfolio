using System.Collections;
using UnityEngine;

public class SkeletonMageAI : MonoBehaviour
{
    public float speed = 2f;
    public float attackRange = 5f;  // B�y� menzili daha uzun
    public float followRange = 8f;  // Takip menzili daha uzun
    public float attackCooldown = 2f;
    public GameObject spellPrefab;  // B�y� objesi (k�p)
    public float spellSpeed = 5f;   // B�y�n�n hareket h�z�
    public Transform bulletPoint;
    public float bulletDestroyTime;

    private float lastAttackTime = 0f;
    public Transform barrier;
    public Transform player;
    private Rigidbody2D rb;

    // Animation Components
    private bool canMove = false;
    public float spawnWaitTime = 1.5f;
    private SkeletonAnimationController skeletonAnimatorSC;
    private Animator animator;
    public bool isWalking = true;
    public bool isDead = false;
    private bool isDeathAnimationPlayed = false;
    private bool isInAttackRange = false;

    //Health
    public int maxHealth = 10;
    int currentHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        skeletonAnimatorSC = GetComponent<SkeletonAnimationController>();
        StartCoroutine(WaitAndGo());
        bulletDestroyTime = Time.deltaTime;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        if (barrier == null)
        {
            barrier = GameObject.FindWithTag("Barrier").transform;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !isDeathAnimationPlayed)
        {
            Die();
        }
    }

    void Die()
    {
        if (!isDeathAnimationPlayed)
        {
            isDead = true;
            isDeathAnimationPlayed = true;
            canMove = false;
            animator.SetBool("isDead", true);
            Debug.Log("Skeleton Mage �ld�r�ld�!!!!!");

            float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, deathAnimationLength);
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        float playerDistance = Vector2.Distance(transform.position, player.position);
        float barrierDistance = Vector2.Distance(transform.position, barrier.position);

        if (playerDistance <= followRange)
        {
            if (playerDistance <= attackRange)
            {
                isInAttackRange = true;
                isWalking = false;
                skeletonAnimatorSC.WalkingAnimationHandler(false);

                if (playerDistance < attackRange * 0.8f)
                {
                    MoveAway(player.position);
                }
                else
                {
                    CastSpellAt(player.position);
                }
            }
            else
            {
                isInAttackRange = false;
                isWalking = true;
                skeletonAnimatorSC.WalkingAnimationHandler(true);
                MoveTowards(player.position);
            }

            FlipTowards(player.position);
        }
        else if (barrierDistance <= attackRange)
        {
            isInAttackRange = true;
            isWalking = false;
            skeletonAnimatorSC.WalkingAnimationHandler(false);
            CastSpellAt(barrier.position);
            FlipTowards(barrier.position);
        }
        else
        {
            isInAttackRange = false;
            isWalking = true;
            skeletonAnimatorSC.WalkingAnimationHandler(true);
            MoveTowards(barrier.position);
            FlipTowards(barrier.position);
        }
    }

    IEnumerator WaitAndGo()
    {
        yield return new WaitForSeconds(spawnWaitTime);
        canMove = true;
        skeletonAnimatorSC.WalkingAnimationHandler(isWalking);
    }

    void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void MoveAway(Vector2 target)
    {
        Vector2 direction = ((Vector2)transform.position - target).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void FlipTowards(Vector2 target)
    {
        if (target.x < transform.position.x && transform.localScale.x < 0 ||
            target.x > transform.position.x && transform.localScale.x > 0)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    void CastSpellAt(Vector2 target)
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            skeletonAnimatorSC.AttackAnimationHandler();

            GameObject spell = Instantiate(spellPrefab, bulletPoint.position, Quaternion.identity);
            Rigidbody2D spellRb = spell.GetComponent<Rigidbody2D>();

            Vector2 direction = (target - (Vector2)transform.position).normalized;
            spellRb.velocity = direction * spellSpeed;

            Destroy(spell, 10f);
        }
    }
}
