using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 30;
    public int health = 30;
    public float deathAnimationDuration = 1.5f; // �l�m animasyonu s�resi

    [Header("Components")]
    private PlayerAnimationController animationController;
    public GameObject damagePanel;
    private bool isDead = false;

    private void Awake()
    {
        animationController = GetComponent<PlayerAnimationController>();
        health = maxHealth; // Ba�lang��ta can maksimum olsun
    }

    private void Update()
    {
        // E�er zaten �l� de�ilse ve can 0'a e�it veya k���kse
        if (!isDead && health <= 0)
        {
            StartCoroutine(DeathSequence());
        }
    }

    public void TakeDamage(int damage)
    {
        // E�er zaten �l�yse hasar alma
        if (isDead) return;

        health -= damage;
        DamagePanel(); // Hasar al�nd���nda ekranda k�rm�z� bir panel belirsin
        Debug.Log($"Hasar al�nd�! Kalan can: {health}"); // Debug i�in

        if (health <= 0)
        {
            health = 0; // Can eksi de�ere d��mesin
            StartCoroutine(DeathSequence());
            Debug.Log("Oyuncu �ld�!");
        }
    }

    private void DamagePanel()
    {
        StartCoroutine(FlashDamagePanel());
    }

    private IEnumerator FlashDamagePanel()
    {
        damagePanel.SetActive(true);
        yield return new WaitForSeconds(0.5f); // Panelin a��k kalma s�resi
        damagePanel.SetActive(false);
    }

    private IEnumerator DeathSequence()
    {
        if (isDead) yield break; // E�er zaten �l�yse fonksiyonu terk et

        isDead = true;

        // �l�m animasyonunu ba�lat
        animationController.SetDeath(0);
        Debug.Log("�l�m animasyonu ba�lat�ld�"); // Debug i�in

        // Oyunu yava�lat
        Time.timeScale = 0.5f;

        // Animasyonun bitmesini bekle
        yield return new WaitForSeconds(deathAnimationDuration * Time.timeScale);

        // Oyuncuyu yok et
        Destroy(gameObject);
        Debug.Log("Oyuncu objesi yok edildi"); // Debug i�in
    }

    // Can yenileme fonksiyonu (ihtiya� olursa)
    public void Heal(int amount)
    {
        if (isDead) return;

        health = Mathf.Min(health + amount, maxHealth);
    }

    // Mevcut can� d�nd�r
    public int GetCurrentHealth()
    {
        return health;
    }
}
