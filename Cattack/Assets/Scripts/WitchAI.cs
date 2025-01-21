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

    void Start()
    {
        skillAnimator = GetComponentInChildren<Animator>();
        animator = GetComponent<Animator>();
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
        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        if (isDead || TimeManager.Instance.GetCurrentDay() < attackStartDay || targetObject == null) return;

        float targetDistance = Vector2.Distance(transform.position, targetObject.transform.position);
        if (targetDistance <= attackRange)
        {
            CastLightningBolt();
        }
    }

    void CastLightningBolt()
    {
        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            skillAnimator.SetTrigger("witchAttack1");
            animator.SetTrigger("witchAttack1");

            GameObject spell = Instantiate(lightningBoltPrefab, spellSpawnPoint.position, Quaternion.identity);
            LightningBolt lightningBolt = spell.GetComponent<LightningBolt>();
            if (lightningBolt != null)
            {
                lightningBolt.targetObject = targetObject;
                lightningBolt.speed = spellSpeed;
            }
        }
    }
}