using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public int damage = 10;
    public bool destroyOnHit = true;
    public GameObject hitEffect;
    public float speed;

    private float maxLifetime = 5f;
    private float currentLifetime = 0f;
    private Vector2 moveDirection;

    private WitchSkillFXAnimationController witchSkillFX;

    private void Start()
    {
        witchSkillFX = FindAnyObjectByType<WitchSkillFXAnimationController>();
        // Cadýnýn yönünü al
        WitchAI witchAI = FindObjectOfType<WitchAI>();
        if (witchAI != null)
        {
            moveDirection = witchAI.IsFacingRight ? Vector2.right : Vector2.left;
        }
    }

    private void Update()
    {
        // Hareket et
        transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
        witchSkillFX.TriggerWitchAttack1();
        // Yaþam süresi kontrolü
        currentLifetime += Time.deltaTime;
        if (currentLifetime >= maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
