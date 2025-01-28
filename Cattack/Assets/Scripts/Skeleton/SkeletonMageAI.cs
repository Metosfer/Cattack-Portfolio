using System.Collections;
using UnityEngine;

public class SkeletonMageAI : MonoBehaviour
{
    public float speed = 2f;
    public float attackRange = 5f;  // Büyü menzili daha uzun
    public float followRange = 8f;  // Takip menzili daha uzun
    public float attackCooldown = 2f;
    public GameObject spellPrefab;  // Büyü objesi (küp)
    public float spellSpeed = 5f;   // Büyünün hareket hýzý
    public Transform bulletPoint;
    public float bulletDestroyTime;

    private float lastAttackTime = 0f;
    public Transform barrier;
    public Transform player;
    private Rigidbody2D rb;
    public AudioSource[] mageAudio;

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

    // Layer masks for detection
    private LayerMask barrierLayer;
    private LayerMask playerLayer;
    private Transform currentTarget;

    void Start()
    {
        animator = GetComponent<Animator>();
        skeletonAnimatorSC = GetComponent<SkeletonAnimationController>();
        StartCoroutine(WaitAndGo());
        bulletDestroyTime = Time.deltaTime;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        barrierLayer = LayerMask.GetMask("Barrier");
        playerLayer = LayerMask.GetMask("Player");

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

    public void TakeDamage(int damage)
    {
        mageAudio[1].Play();
        currentHealth -= damage;

        if (currentHealth <= 0 && !isDeathAnimationPlayed)
        {
            Die();
        }
    }

    void Die()
    {
        mageAudio[0].Play();
        if (isDeathAnimationPlayed) return;
        isDeathAnimationPlayed = true;

        isDead = true;
        canMove = false;

        if (animator != null)
        {
            animator.SetBool("isDead", true);
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Death"))
            {
                float deathAnimationLength = stateInfo.length;
                Destroy(gameObject, deathAnimationLength);
            }
            else
            {
                Destroy(gameObject, 1.5f); // Varsayýlan süre
            }
        }
        else
        {
            Destroy(gameObject, 1.5f);
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return; // Ölü durumdaysa hareket veya baþka iþlemleri durdur

        if (!canMove) return;

        UpdateCurrentTarget();

        if (currentTarget != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);

            if (distanceToTarget <= attackRange)
            {
                isInAttackRange = true;
                isWalking = false;
                skeletonAnimatorSC.WalkingAnimationHandler(false);

                if (distanceToTarget < attackRange * 0.8f)
                {
                    MoveAway(currentTarget.position);
                }
                else
                {
                    CastSpellAt(currentTarget.position);
                }
            }
            else
            {
                isInAttackRange = false;
                isWalking = true;
                skeletonAnimatorSC.WalkingAnimationHandler(true);
                MoveTowards(currentTarget.position);
            }

            FlipTowards(currentTarget.position);
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
            isWalking = false; // Saldýrý sýrasýnda yürüme durumunu kapat
            skeletonAnimatorSC.AttackAnimationHandler();

            GameObject spell = Instantiate(spellPrefab, bulletPoint.position, Quaternion.identity);
            Rigidbody2D spellRb = spell.GetComponent<Rigidbody2D>();

            Vector2 direction = (target - (Vector2)transform.position).normalized;
            spellRb.velocity = direction * spellSpeed;

            Destroy(spell, 10f);

            // Saldýrý animasyonu bittikten sonra yürüme durumunu kontrol et
            StartCoroutine(ResetWalkingAfterAttack());
        }
    }

    private IEnumerator ResetWalkingAfterAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            isWalking = true;
            skeletonAnimatorSC.WalkingAnimationHandler(true);
        }
    }
}
