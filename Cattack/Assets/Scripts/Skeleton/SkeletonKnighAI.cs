using System.Collections;
using UnityEngine;

public class SkeletonKnightAI : MonoBehaviour
{
    public float speed = 2f;
    public float attackRange = 1.5f;
    public float followRange = 5f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    public Transform barrier;
    public Transform player;
    private Rigidbody2D rb;

    public AudioSource[] knightAudio;
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

    void Start()
    {
        animator = GetComponent<Animator>();
        skeletonAnimatorSC = GetComponent<SkeletonAnimationController>();
        StartCoroutine(WaitAndGo()); // Correctly start the coroutine here
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        if (barrier == null)
        {
            barrier = GameObject.FindWithTag("Barrier").transform;
        }
    }

    // Implement TakeDamage from IDamageable
    public void TakeDamage(int damage)
    {
        knightAudio[1].Play();
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDeathAnimationPlayed)
        {
            Die();
        }
    }

    void Die()
    {
        knightAudio[0].Play();  
        if (!isDeathAnimationPlayed)
        {
            isDead = true;
            isDeathAnimationPlayed = true;
            canMove = false;
            animator.SetBool("isDead", true);
            Debug.Log("Skeleton Öldürüldü!!!!!");

            // Ölüm animasyonunun uzunluðunu al
            float deathAnimationLength = animator.GetCurrentAnimatorStateInfo(0).length;

            // Ölüm animasyonu bittikten sonra destroy et
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
                AttackPlayer();
            }
            else
            {
                isInAttackRange = false;
                isWalking = true;
                skeletonAnimatorSC.WalkingAnimationHandler(true);
                MoveTowards(player.position);
                FlipTowards(player.position);
            }
        }
        else if (barrierDistance <= attackRange)
        {
            isInAttackRange = true;
            isWalking = false;
            speed = 0;
            skeletonAnimatorSC.WalkingAnimationHandler(false);
            AttackBarrier();
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
        yield return new WaitForSeconds(spawnWaitTime); // Wait for the specified spawnWaitTime
        canMove = true; // Once the wait time is over, enable movement
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
            // Rastgele bir saldýrý animasyonu seç
            knightAudio[2].Play();
            string attackTrigger = Random.Range(0, 2) == 0 ? "isAttacking" : "isAttacking2";
            animator.SetTrigger(attackTrigger);

            Debug.Log("Player'a saldýrýyor!");
            lastAttackTime = Time.time;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
            }
        }
    }

    void AttackBarrier()
    {
        if (barrier != null && Time.time > lastAttackTime + attackCooldown)
        {
            // Rastgele bir saldýrý animasyonu seç
            string attackTrigger = Random.Range(0, 2) == 0 ? "isAttacking" : "isAttacking2";
            animator.SetTrigger(attackTrigger);

            Debug.Log("Bariyere saldýrýyor!");
            lastAttackTime = Time.time;
            Barrier barrierComponent = barrier.GetComponent<Barrier>();
            if (barrierComponent != null)
            {
                barrierComponent.TakeDamage(10);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Çarpýþma algýlandý: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Barrier"))
        {
            speed = 0f;
            Debug.Log("Barrier ile çarpýþma algýlandý.");
            Barrier barrierComponent = collision.gameObject.GetComponent<Barrier>();
            if (barrierComponent != null)
            {
                barrierComponent.TakeDamage(10);
            }
        }
    }
}
