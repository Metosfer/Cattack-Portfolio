// Barrier.cs
using UnityEngine;
using TMPro;

public class Barrier : MonoBehaviour
{
    private static Barrier instance;
    public GameObject br;
    public GameObject barrierRender;
    public int health = 100;
    public TextMeshProUGUI healthText;
    public BoxCollider2D barrierCollider; // Yeni eklendi

    private void Awake()
    {
        instance = this;
        barrierCollider = GetComponent<BoxCollider2D>();
        if (barrierCollider == null)
        {
            barrierCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        // Barrier'ýn tag'ini ayarla
        gameObject.tag = "Barrier";
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
            healthText.text = "Barrier Health: " + health;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Bariyer hasar aldý. Kalan saðlýk: " + health);

        if (health <= 0)
        {
            DestroyBarrier();
        }
    }

    private void DestroyBarrier()
    {
        if (br != null) br.SetActive(false);
        Debug.Log("Bariyer yýkýldý!");
        if (barrierRender != null) Destroy(barrierRender);
        if (healthText != null) Destroy(healthText.gameObject);
        Destroy(gameObject);
    }

    void BossDay()
    {
        if (TimeManager.Instance.currentDay == TimeManager.Instance.bossDay)
        {
            if (br != null) br.SetActive(false);
            if (barrierRender != null) Destroy(barrierRender);
            if (healthText != null) Destroy(healthText.gameObject);
            Destroy(gameObject);
        }
    }

    public static Barrier GetInstance()
    {
        return instance;
    }
}
