using UnityEngine;
using TMPro;

public class Barrier : MonoBehaviour
{
    public GameObject br;
    public GameObject barrierRender;
    public int health = 200;
    public TextMeshProUGUI healthText;
    public BoxCollider2D barrierCollider;

    // Yeni de�i�kenler
    public bool isInvulnerable = false;
    public float damageFlashDuration = 0.1f;
    public float minDamageCooldown = 0.2f;
    private float lastDamageTime;
    private SpriteRenderer spriteRenderer;

    // Hasar alma paneli i�in yeni de�i�ken
    public GameObject damagePanel;
    public float damagePanelDuration = 0.5f; // Panelin a��k kalma s�resi

    private void Awake()
    {
        // Collider ayarlar�
        barrierCollider = GetComponent<BoxCollider2D>();
        if (barrierCollider == null)
        {
            barrierCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        barrierCollider.isTrigger = false;

        // Sprite Renderer referans�
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Tag ve Layer ayarlar�
        gameObject.tag = "Barrier";
        gameObject.layer = LayerMask.NameToLayer("Barrier"); // Yeni layer'� kullan

        lastDamageTime = 0f;
    }

    private void Update()
    {
        BossDay();
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            // Barrier'�n ismini de g�sterelim
            healthText.text = $"{gameObject.name} Health: {health}";
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || Time.time - lastDamageTime < minDamageCooldown)
        {
            Debug.Log($"{gameObject.name} bariyeri invulnerable veya cooldown i�inde. Hasar uygulanmad�.");
            return;
        }

        health -= damage;
        lastDamageTime = Time.time;

        Debug.Log($"{gameObject.name} bariyeri {damage} hasar ald�. Kalan sa�l�k: {health}");

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRoutine());
        }

        // Hasar alma panelini etkinle�tir
        if (damagePanel != null)
        {
            damagePanel.SetActive(true);
            StartCoroutine(DisableDamagePanelAfterDelay());
        }

        if (health <= 0)
        {
            DestroyBarrier();
        }
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(damageFlashDuration);
            spriteRenderer.color = originalColor;
        }
    }

    private System.Collections.IEnumerator DisableDamagePanelAfterDelay()
    {
        yield return new WaitForSeconds(damagePanelDuration);
        if (damagePanel != null)
        {
            damagePanel.SetActive(false);
        }
    }

    private void DestroyBarrier()
    {
        if (br != null) br.SetActive(false);
        Debug.Log($"{gameObject.name} bariyeri y�k�ld�!");

        if (barrierCollider != null)
        {
            barrierCollider.enabled = false;
        }

        if (barrierRender != null) Destroy(barrierRender);
        if (healthText != null) Destroy(healthText.gameObject);
        Destroy(gameObject);
    }

    private void BossDay()
    {
        if (TimeManager.Instance != null && TimeManager.Instance.currentDay == TimeManager.Instance.bossDay)
        {
            if (br != null) br.SetActive(false);
            if (barrierRender != null) Destroy(barrierRender);
            if (healthText != null) Destroy(healthText.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Skeleton"))
        {
            SkeletonAI skeleton = collision.gameObject.GetComponent<SkeletonAI>();
            if (skeleton != null)
            {
                Debug.Log($"{gameObject.name} bariyeri, Skeleton ile �arp���yor. Skeleton pozisyonu: {skeleton.transform.position}");
            }
        }
    }
}
