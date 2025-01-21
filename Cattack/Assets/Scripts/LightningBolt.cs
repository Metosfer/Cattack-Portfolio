using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public int damage = 10;
    public bool destroyOnHit = true;
    public GameObject hitEffect;
    public float speed;
    public GameObject targetObject; // Hedef obje

    private float maxLifetime = 5f;
    private float currentLifetime = 0f;

    private void Update()
    {
        if (targetObject == null) return;

        // MoveTowards ile hedefe doðru hareket
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetObject.transform.position,
            speed * Time.deltaTime
        );

        // Rotasyonu hedefe doðru ayarla
        Vector2 direction = (targetObject.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Yaþam süresi kontrolü
        currentLifetime += Time.deltaTime;
        if (currentLifetime >= maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == targetObject)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                HandleHit();
            }
        }
    }

    private void HandleHit()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        if (destroyOnHit)
        {
            Destroy(gameObject);
        }
    }
}