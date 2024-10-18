using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    public float speed = 2f; // �skeletin hareket h�z�
    public float attackRange = 1.5f; // Sald�r� menzili
    public float followRange = 5f; // Takip menzili
    public float attackCooldown = 1f; // Sald�r� bekleme s�resi
    private float lastAttackTime = 0f;

    public Transform barrier; // Bariyer hedefi
    public Transform player; // Oyuncu hedefi (kedi)

    private Rigidbody2D rb;
    private Animator animator; // E�er iskelet i�in animasyon varsa kullan�labilir

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform; // Oyuncuyu 'Player' tag'iyle buluyoruz
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.position);
        float barrierDistance = Vector2.Distance(transform.position, barrier.position);

        // �nce oyuncuyu takip menzilinde kontrol et
        if (playerDistance <= followRange)
        {
            // E�er oyuncu sald�r� menzilindeyse sald�r
            if (playerDistance <= attackRange)
            {
                AttackPlayer();
                
            }
            // E�er oyuncu sald�r� menzilinde de�ilse ona do�ru hareket et
            else
            {
                MoveTowards(player.position);
                Debug.Log("Player'a y�neldi!");
            }
        }
        // Oyuncu yoksa bariyere sald�r
        else if (barrierDistance <= attackRange)
        {
            AttackBarrier();
            
        }
        // Oyuncu takip menzilinde de�ilse bariyere do�ru hareket et
        else
        {
            Debug.Log("Bariyere do�ru y�neldi");
            MoveTowards(barrier.position);
        }
    }

    void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void AttackPlayer()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("Player'a sald�r�yor!");
            lastAttackTime = Time.time;
            // Oyuncuya hasar verecek fonksiyonu �a��rabilirsin
            player.GetComponent<PlayerHealth>().TakeDamage(10);
            Destroy(gameObject); // Sald�r� sonras� iskeleti yok et
        }
    }

    void AttackBarrier()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("Bariyere sald�r�yor!");
            lastAttackTime = Time.time;
            // Bariyere hasar verecek fonksiyonu �a��rabilirsin
            barrier.GetComponent<Barrier>().TakeDamage(10);
            Destroy(gameObject); // Sald�r� sonras� iskeleti yok et
        }
    }
}
