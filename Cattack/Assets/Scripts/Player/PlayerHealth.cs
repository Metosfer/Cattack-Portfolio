using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 30;
    public int health = 30;
    public float deathAnimationDuration = 1.5f; // Ölüm animasyonu süresi

    [Header("Components")]
    private PlayerAnimationController animationController;
    public GameObject damagePanel;
    private bool isDead = false;

    private void Awake()
    {
        animationController = GetComponent<PlayerAnimationController>();
        health = maxHealth; // Baþlangýçta can maksimum olsun
    }

    private void Update()
    {
        // Eðer zaten ölü deðilse ve can 0'a eþit veya küçükse
        if (!isDead && health <= 0)
        {
            StartCoroutine(DeathSequence());
        }
    }

    public void TakeDamage(int damage)
    {
        // Eðer zaten ölüyse hasar alma
        if (isDead) return;

        health -= damage;
        DamagePanel(); // Hasar alýndýðýnda ekranda kýrmýzý bir panel belirsin
        Debug.Log($"Hasar alýndý! Kalan can: {health}"); // Debug için

        if (health <= 0)
        {
            health = 0; // Can eksi deðere düþmesin
            StartCoroutine(DeathSequence());
            Debug.Log("Oyuncu öldü!");
        }
    }

    private void DamagePanel()
    {
        StartCoroutine(FlashDamagePanel());
    }

    private IEnumerator FlashDamagePanel()
    {
        damagePanel.SetActive(true);
        yield return new WaitForSeconds(0.5f); // Panelin açýk kalma süresi
        damagePanel.SetActive(false);
    }

    private IEnumerator DeathSequence()
    {
        if (isDead) yield break; // Eðer zaten ölüyse fonksiyonu terk et

        isDead = true;

        // Ölüm animasyonunu baþlat
        animationController.SetDeath(0);
        Debug.Log("Ölüm animasyonu baþlatýldý"); // Debug için

        // Oyunu yavaþlat
        Time.timeScale = 0.5f;

        // Animasyonun bitmesini bekle
        yield return new WaitForSeconds(deathAnimationDuration * Time.timeScale);

        // Oyuncuyu yok et
        Destroy(gameObject);
        Debug.Log("Oyuncu objesi yok edildi"); // Debug için
    }

    // Can yenileme fonksiyonu (ihtiyaç olursa)
    public void Heal(int amount)
    {
        if (isDead) return;

        health = Mathf.Min(health + amount, maxHealth);
    }

    // Mevcut caný döndür
    public int GetCurrentHealth()
    {
        return health;
    }
}
