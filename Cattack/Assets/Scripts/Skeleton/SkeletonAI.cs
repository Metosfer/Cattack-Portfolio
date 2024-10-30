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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        if (barrier == null)
        {
            barrier = GameObject.FindWithTag("Barrier").transform;
        }
    }

    private void FixedUpdate()
    {
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
            speed = 0;
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
                // Destroy(gameObject) kaldýrýldý - artýk player'a saldýrýnca ölmeyecek
            }
        }
    }

    void AttackBarrier()
    {
        if (barrier != null && Time.time > lastAttackTime + attackCooldown)
        {
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
                Debug.Log("Barrier bileþeni bulundu ve hasar veriliyor.");
                barrierComponent.TakeDamage(10);
            }
        }
    }

   
}