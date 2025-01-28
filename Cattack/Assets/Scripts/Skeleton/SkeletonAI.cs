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

    // Health
    public int maxHealth = 10;
    int currentHealth;

    // Layer masks for detection
    private LayerMask barrierLayer;
    private LayerMask playerLayer;
    private Transform currentTarget;

    private void Start()
    {
        animator = GetComponent<Animator>();
        skeletonAnimatorSC = GetComponent<SkeletonAnimationController>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        barrierLayer = LayerMask.GetMask("Barrier");
        playerLayer = LayerMask.GetMask("Player");

        StartCoroutine(WaitAndGo());
        FindTargets();
    }

    private void FindTargets()
    {
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        float nearestBarrierDistance = float.MaxValue;
        Transform nearestBarrier = null;

        foreach (GameObject barrierObj in barriers)
        {
            if (barrierObj.activeInHierarchy)
            {
                float distance = Vector2.Distance(transform.position, barrierObj.transform.position);
                if (distance < nearestBarrierDistance)
                {
                    nearestBarrierDistance = distance;
                    nearestBarrier = barrierObj.transform;
                }
            }
        }
        barrier = nearestBarrier;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        UpdateCurrentTarget();
    }

    private void UpdateCurrentTarget()
    {
        float barrierDistance = barrier != null ? Vector2.Distance(transform.position, barrier.position) : float.MaxValue;
        float playerDistance = player != null ? Vector2.Distance(transform.position, player.position) : float.MaxValue;

        if (player != null && playerDistance <= followRange)
        {
            currentTarget = player;
        }
        else if (barrier != null && barrier.gameObject.activeInHierarchy)
        {
            currentTarget = barrier;
        }
        else
        {
            currentTarget = null;
        }
    }

    private void FixedUpdate()
    {
        if (!canMove || isDead) return;

        UpdateCurrentTarget();

        if (currentTarget != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);

            if (distanceToTarget <= attackRange)
            {
                StopAndAttack();
                if (currentTarget == barrier)
                {
                    AttackBarrier();
                }
                else
                {
                    AttackPlayer();
                }
            }
            else
            {
                MoveToTarget(currentTarget.position);
            }
        }
    }

    private void StopAndAttack()
    {
        isWalking = false;
        skeletonAnimatorSC.WalkingAnimationHandler(false);
    }

    private void MoveToTarget(Vector2 targetPosition)
    {
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
            skeletonAnimatorSC.AttackAnimationHandler();
            lastAttackTime = Time.time;

            // Barrier'ýn direkt Collider'ýna hasar verme
            Barrier barrierComponent = barrier.GetComponent<Barrier>();
            if (barrierComponent != null)
            {
                barrierComponent.TakeDamage(10);
                Debug.Log($"{barrier.name} Barrier hasar aldý! Kalan saðlýk: {barrierComponent.health}");
            }
            else
            {
                Debug.LogWarning("Barrier bileþeni bulunamadý!");
            }
        }
        else
        {
            Debug.LogWarning("Barrier aktif deðil veya bulunamadý!");
        }
    }


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
        isDead = true;
        isDeathAnimationPlayed = true;
        canMove = false;
        animator.SetBool("isDead", true);

        float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, deathAnimationLength);
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
        if ((target.x < transform.position.x && transform.localScale.x > 0) ||
            (target.x > transform.position.x && transform.localScale.x < 0))
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
            lastAttackTime = Time.time;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRange);
    }
}
