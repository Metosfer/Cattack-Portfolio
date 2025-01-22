using UnityEngine;

public class WitchAI : MonoBehaviour
{
    public float attackRange = 7f;
    public float attackCooldown = 3f;
    public GameObject lightningBoltPrefab;
    public float spellSpeed = 7f;
    public Transform spellSpawnPoint;
    public int attackStartDay = 5;
    public GameObject targetObject; // Player objesi için referans

    private float lastAttackTime = 0f;
    private Animator animator;
    private Animator skillAnimator;
    private bool isDead = false;
    public int maxHealth = 20;
    private int currentHealth;

    private WitchAnimationController witchAnimations;
    

    public bool IsFacingRight { get; private set; } = true; // Cadýnýn yönünü belirten özellik

    void Start()
    {
        witchAnimations = GetComponent<WitchAnimationController>();
        skillAnimator = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;

        if (targetObject == null)
        {
            Debug.LogError("Target Object atanmamýþ! Lütfen inspector'dan target object'i atayýn.");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Destroy(gameObject, 0.5f);
    }

    private void Update()
    {
        if (isDead || TimeManager.Instance.GetCurrentDay() < attackStartDay || targetObject == null) return;

        float targetDistance = Vector2.Distance(transform.position, targetObject.transform.position);
        if (targetDistance <= attackRange)
        {
            CastLightningBolt();
        }

        // Cadýnýn player'a doðru bakmasýný saðla
        Vector3 direction = targetObject.transform.position - transform.position;
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Sað tarafa bak
            IsFacingRight = true;
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Sol tarafa bak
            IsFacingRight = false;
        }
    }

    void CastLightningBolt()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            skillAnimator.SetTrigger("witchAttack1");
            witchAnimations.SetPlayerAttack1();

            Instantiate(lightningBoltPrefab, spellSpawnPoint.position, Quaternion.identity);
        }
    }
}
