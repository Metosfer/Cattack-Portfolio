using UnityEngine;

public class SkeletonMageAI : MonoBehaviour
{
    public float speed = 2f;
    public float attackRange = 5f;  // B�y� menzili daha uzun
    public float followRange = 8f;  // Takip menzili daha uzun
    public float attackCooldown = 2f;
    public GameObject spellPrefab;  // B�y� objesi (k�p)
    public float spellSpeed = 5f;   // B�y�n�n hareket h�z�

    private float lastAttackTime = 0f;
    public Transform barrier;
    public Transform player;
    private Rigidbody2D rb;

    //Health
    public int maxHealth = 10;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        /////////////////
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

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Skeleton �ld�r�ld�!!!!!");
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        float playerDistance = Vector2.Distance(transform.position, player.position);
        float barrierDistance = Vector2.Distance(transform.position, barrier.position);

        // Optimal mesafeyi koru
        float optimalRange = attackRange * 0.8f;

        if (playerDistance <= followRange)
        {
            if (playerDistance <= attackRange)
            {
                if (playerDistance < optimalRange)
                {
                    // �ok yak�nsa uzakla�
                    MoveAway(player.position);
                }
                else
                {
                    // Menzildeyse sald�r
                    CastSpellAt(player.position);
                }
            }
            else
            {
                // Menzil d���ndaysa yakla�
                MoveTowards(player.position);
            }
        }
        else if (barrierDistance <= attackRange)
        {
            CastSpellAt(barrier.position);
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

    void MoveAway(Vector2 target)
    {
        Vector2 direction = ((Vector2)transform.position - target).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void CastSpellAt(Vector2 target)
    {

        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            // B�y� objesini olu�tur
            GameObject spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
            Rigidbody2D spellRb = spell.GetComponent<Rigidbody2D>();

            // Hedefe do�ru y�nlendir
            Vector2 direction = (target - (Vector2)transform.position).normalized;
            spellRb.velocity = direction * spellSpeed;

            // 5 saniye sonra yok et
            Destroy(spell, 5f);
        }
    }
}