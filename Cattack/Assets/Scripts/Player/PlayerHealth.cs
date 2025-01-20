using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int health = 100;
    private float currentHealth;
    public float deathAnimationDuration = 1.5f;

    [Header("Components")]
    private PlayerAnimationController animationController;
    public GameObject damagePanel;
    private bool isDead = false;
    private UnityEngine.UI.Slider healthSlider;
    private PlayerSkills playerSkills;

    private void Start()
    {
        playerSkills = GetComponent<PlayerSkills>();
    }

    private void Awake()
    {
        healthSlider = FindAnyObjectByType<UnityEngine.UI.Slider>();
        animationController = GetComponent<PlayerAnimationController>();
        health = maxHealth;
    }

    private void Update()
    {
        if (playerSkills.isHissActive)
        {
            ActivateHissSkill();
        }

        if (!isDead && health <= 0)
        {
            StartCoroutine(DeathSequence());
        }
    }

    public void ActivateHissSkill()
    {
        // Hiss skill etkisi: Caný sadece hissHealthBoost kadar artýr
        int boostAmount = playerSkills.hissHealthBoost;
        // Can deðerini güncelle (maxHealth sýnýrýna dikkat ederek)
        health = Mathf.Min(health + boostAmount, maxHealth);
        // Slider'ý canla senkronize et
        healthSlider.value = health;
        // Hiss skill etkisinin tekrar çalýþmasýný önlemek için bayraðý kapat
        playerSkills.isHissActive = false;
        // Hiss cooldown iþlemini baþlat
        StartCoroutine(HealthCooldown());
    }

    IEnumerator HealthCooldown()
    {
        yield return new WaitForSeconds(playerSkills.hissCooldown);
    }

    public void TakeDamage(int damage)
    {
        playerSkills.audioSource[4].Play(); 
        if (isDead) return;

        healthSlider.value -= damage;
        health -= damage;
        DamagePanel();
        Debug.Log($"Hasar alýndý! Kalan can: {health}");

        if (health <= 0)
        {
            health = 0;
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
        yield return new WaitForSeconds(0.5f);
        damagePanel.SetActive(false);
    }

    private IEnumerator DeathSequence()
    {
        if (isDead) yield break;

        isDead = true;
        animationController.SetDeath(0);
        Debug.Log("Ölüm animasyonu baþlatýldý");

        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(deathAnimationDuration * Time.timeScale);

        Destroy(gameObject);
        Debug.Log("Oyuncu objesi yok edildi");
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        health = Mathf.Min(health + amount, maxHealth);
    }

    public int GetCurrentHealth()
    {
        return health;
    }
}