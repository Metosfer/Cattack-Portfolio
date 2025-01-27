using System.Collections;
using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    public float speed = 2f;
    public float attackRange = 1.5f;
    public float followRange = 5f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    public Transform barrier;
    public Transform player;
    private Rigidbody2D rb;

    public AudioSource[] skeletonAudio;

    // Animation Components
    private bool canMove = false;
    public float spawnWaitTime = 1.5f;
    private SkeletonAnimationController skeletonAnimatorSC;
    private Animator animator;
    public bool isWalking = true;
    public bool isDead = false;
    private bool isDeathAnimationPlayed = false;
    private bool isInAttackRange = false;

    // Health
    public int maxHealth = 10;
    int currentHealth;

    private void Start()
    {
        animator = GetComponent<Animator>();
        skeletonAnimatorSC = GetComponent<SkeletonAnimationController>();
        StartCoroutine(WaitAndGo());
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        // Önce barrier'ý bul, sonra player'ý
        barrier = GameObject.FindWithTag("Barrier")?.transform;
        player = GameObject.FindWithTag("Player")?.transform;

        // Eðer barrier yoksa log at
        if (barrier == null)
        {
            Debug.LogWarning("Barrier bulunamadý!");
        }
    }

    private void FixedUpdate()
    {
        if (!canMove || isDead) return;

        // Barrier kontrolü
        if (barrier != null && barrier.gameObject.activeInHierarchy)
        {
            float barrierDistance = Vector2.Distance(transform.position, barrier.position);

            if (barrierDistance <= attackRange)
            {
                StopAndAttack(true);
                AttackBarrier();
                return; // Barrier saldýrýsý varsa diðer kontrolleri yapma
            }
            else if (barrierDistance <= followRange)
            {
                MoveToTarget(barrier.position);
                return; // Barrier takibi varsa diðer kontrolleri yapma
            }
        }

        // Barrier yoksa veya menzilde deðilse player'a odaklan
        if (player != null)
        {
            float playerDistance = Vector2.Distance(transform.position, player.position);

            if (playerDistance <= attackRange)
            {
                StopAndAttack(false);
                AttackPlayer();
            }
            else
            {
                MoveToTarget(player.position);
            }
        }
    }

    private void StopAndAttack(bool isBarrier)
    {
        isInAttackRange = true;
        isWalking = false;
        skeletonAnimatorSC.WalkingAnimationHandler(false);
    }

    private void MoveToTarget(Vector2 targetPosition)
    {
        isInAttackRange = false;
        isWalking = true;
        skeletonAnimatorSC.WalkingAnimationHandler(true);
        MoveTowards(targetPosition);
        FlipTowards(targetPosition);
    }

    void AttackBarrier()
    {
        if (Time.time <= lastAttackTime + attackCooldown) return;

        if (barrier != null && barrier.gameObject.activeInHierarchy)
        {
            Vector2 direction = (barrier.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, LayerMask.GetMask("Default"));

            if (hit.collider != null && hit.collider.CompareTag("Barrier"))
            {
                skeletonAnimatorSC.AttackAnimationHandler();
                lastAttackTime = Time.time;

                Barrier barrierComponent = hit.collider.GetComponent<Barrier>();
                if (barrierComponent != null)
                {
                    barrierComponent.TakeDamage(10);
                    Debug.Log($"Barrier'a hasar verildi! Skeleton pozisyonu: {transform.position}, Barrier pozisyonu: {barrier.position}");
                }
            }
        }
    }

    // Diðer metodlar ayný kalacak...
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public void TakeDamage(int damage)
    {
        skeletonAudio[1].Play();
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDeathAnimationPlayed)
        {
            Die();
        }
    }

    void Die()
    {
        skeletonAudio[0].Play();
        if (!isDeathAnimationPlayed)
        {
            isDead = true;
            isDeathAnimationPlayed = true;
            canMove = false;
            animator.SetBool("isDead", true);
            Debug.Log("Skeleton Öldürüldü!!!!!");

            float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, deathAnimationLength);
        }
    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDeathAnimationPlayed)
        {
            Die();
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

    void AttackPlayer()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            skeletonAnimatorSC.AttackAnimationHandler();
            Debug.Log("Player'a saldýrýyor!");
            lastAttackTime = Time.time;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
            }
        }
    }
}