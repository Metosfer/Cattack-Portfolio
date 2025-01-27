using UnityEngine;

public class SpellProjectile : MonoBehaviour
{
    public int damage = 10;
    public bool destroyOnHit = true;
    public GameObject hitEffect;

    private void Start()
    {
        // Komponenetleri kontrol et
        Collider2D projectileCollider = GetComponent<Collider2D>();
        Rigidbody2D projectileRb = GetComponent<Rigidbody2D>();

        // Barrier kontrolü
        GameObject barrier = GameObject.FindWithTag("Barrier");
        if (barrier != null)
        {
            Barrier barrierComponent = barrier.GetComponent<Barrier>();
            Collider2D barrierCollider = barrier.GetComponent<Collider2D>();

            if (barrierCollider != null)
            {
                Debug.Log("Barrier Collider tipi: " + barrierCollider.GetType().Name);
                Debug.Log("Barrier Collider isTrigger: " + barrierCollider.isTrigger);
            }
        }
        else
        {
            Debug.Log("Barrier objesi bulunamadý!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerEnter2D tetiklendi. Çarpýlan obje: {collision.gameObject.name}, Tag: {collision.gameObject.tag}");

        if (collision.CompareTag("Player"))
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

        if (collision.gameObject.CompareTag("Player"))
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
