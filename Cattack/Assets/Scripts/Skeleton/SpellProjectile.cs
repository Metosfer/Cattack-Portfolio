using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    public int damage = 10;
    public bool destroyOnHit = true;
    public GameObject hitEffect;
    private Collider2D projectileCollider;
    private Rigidbody2D projectileRb;

    private void Start()
    {
        // Komponentleri al ve kaydet
        projectileCollider = GetComponent<Collider2D>();
        projectileRb = GetComponent<Rigidbody2D>();

        // Eðer Rigidbody2D yoksa ekle
        if (projectileRb == null)
        {
            projectileRb = gameObject.AddComponent<Rigidbody2D>();
            projectileRb.gravityScale = 0;
        }

        // Collider ayarlarý
        if (projectileCollider != null)
        {
            projectileCollider.isTrigger = true; // Mermi trigger olarak çalýþsýn
        }

        // Debug için barrier kontrolü
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        foreach (GameObject barrier in barriers)
        {
            Barrier barrierComponent = barrier.GetComponent<Barrier>();
            Collider2D barrierCollider = barrier.GetComponent<Collider2D>();
            if (barrierCollider != null)
            {
                Debug.Log($"Barrier {barrier.name} - Collider tipi: {barrierCollider.GetType().Name}, isTrigger: {barrierCollider.isTrigger}");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerEnter2D tetiklendi. Çarpýlan obje: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

        if (collision.CompareTag("Barrier"))
        {
            Barrier barrier = collision.GetComponent<Barrier>();
            if (barrier != null)
            {
                Debug.Log($"Mermi bariyere çarptý! Pozisyon: {transform.position}");
                barrier.TakeDamage(damage);
                HandleHit();
            }
        }
        else if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                HandleHit();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionEnter2D tetiklendi. Çarpýlan obje: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

        if (collision.gameObject.CompareTag("Barrier"))
        {
            Barrier barrier = collision.gameObject.GetComponent<Barrier>();
            if (barrier != null)
            {
                Debug.Log($"Mermi bariyere çarptý! Pozisyon: {transform.position}");
                barrier.TakeDamage(damage);
                HandleHit();
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                HandleHit();
            }
        }
    }

    private void HandleHit()
    {
        Debug.Log("HandleHit çaðrýldý");
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        if (destroyOnHit)
        {
            Debug.Log("Mermi yok ediliyor");
            Destroy(gameObject);
        }
    }
}