// SkeletonAI.cs
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
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        if (barrier == null)
        {
            barrier = GameObject.FindWithTag("Barrier").transform;
        }
    }

    void FixedUpdate()
    {
        if (barrier == null || player == null) return;

        float playerDistance = Vector2.Distance(transform.position, player.position);
        float barrierDistance = Vector2.Distance(transform.position, barrier.position);

        if (playerDistance <= followRange)
        {
            if (playerDistance <= attackRange)
            {
                AttackPlayer();
            }
            else
            {
                MoveTowards(player.position);
            }
        }
        else if (barrierDistance <= attackRange)
        {
            AttackBarrier();
        }
        else
        {
            MoveTowards(barrier.position);
        }
    }

    void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    public void OnColliderEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Barrier"))
        {
            Debug.Log("Ýskelet bariyere çarptý!");
            Destroy(this.gameObject);
        }
    }

    void AttackPlayer()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("Player'a saldýrýyor!");
            lastAttackTime = Time.time;
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
                Destroy(gameObject);
            }
        }
    }

    void AttackBarrier()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("Bariyere saldýrýyor!");
            lastAttackTime = Time.time;
            Barrier barrierComponent = barrier.GetComponent<Barrier>();
            if (barrierComponent != null)
            {
                barrierComponent.TakeDamage(10);
                Destroy(gameObject);
            }
        }
    }
}

