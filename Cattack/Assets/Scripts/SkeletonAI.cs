using UnityEngine;

public class SkeletonAI : MonoBehaviour
{
    public float speed = 2f; // Ýskeletin hareket hýzý
    public float attackRange = 1.5f; // Saldýrý menzili
    public float followRange = 5f; // Takip menzili
    public float attackCooldown = 1f; // Saldýrý bekleme süresi
    private float lastAttackTime = 0f;

    public Transform barrier; // Bariyer hedefi
    public Transform player; // Oyuncu hedefi (kedi)

    private Rigidbody2D rb;
    private Animator animator; // Eðer iskelet için animasyon varsa kullanýlabilir

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

        // Önce oyuncuyu takip menzilinde kontrol et
        if (playerDistance <= followRange)
        {
            // Eðer oyuncu saldýrý menzilindeyse saldýr
            if (playerDistance <= attackRange)
            {
                AttackPlayer();
                
            }
            // Eðer oyuncu saldýrý menzilinde deðilse ona doðru hareket et
            else
            {
                MoveTowards(player.position);
                Debug.Log("Player'a yöneldi!");
            }
        }
        // Oyuncu yoksa bariyere saldýr
        else if (barrierDistance <= attackRange)
        {
            AttackBarrier();
            
        }
        // Oyuncu takip menzilinde deðilse bariyere doðru hareket et
        else
        {
            Debug.Log("Bariyere doðru yöneldi");
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
            Debug.Log("Player'a saldýrýyor!");
            lastAttackTime = Time.time;
            // Oyuncuya hasar verecek fonksiyonu çaðýrabilirsin
            player.GetComponent<PlayerHealth>().TakeDamage(10);
            Destroy(gameObject); // Saldýrý sonrasý iskeleti yok et
        }
    }

    void AttackBarrier()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("Bariyere saldýrýyor!");
            lastAttackTime = Time.time;
            // Bariyere hasar verecek fonksiyonu çaðýrabilirsin
            barrier.GetComponent<Barrier>().TakeDamage(10);
            Destroy(gameObject); // Saldýrý sonrasý iskeleti yok et
        }
    }
}
