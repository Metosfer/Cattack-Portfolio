// Barrier.cs
using UnityEngine;
using TMPro;

public class Barrier : MonoBehaviour
{
    private static Barrier instance;
    public GameObject br;

    public GameObject barrierRender;
    public int health = 100;

    // TextMeshProUGUI referansý
    public TextMeshProUGUI healthText;

    private void Update()
    {
        BossDay();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Bariyer hasar aldý. Kalan saðlýk: " + health);

        // Saðlýk metnini güncelle
        if (healthText != null)
        {
            healthText.text = "Barrier Health: " + health;
        }

        if (health <= 0)
        {
            br.SetActive(false);
            Debug.Log("Bariyer yýkýldý!");
            Destroy(gameObject);
            Destroy(barrierRender);
        }
    }
    void BossDay()
    {
        if(TimeManager.Instance.currentDay == TimeManager.Instance.bossDay)
        {
         Destroy(br);
            Destroy(barrierRender);
            Destroy(healthText);
        }

    }
    // Singleton Pattern implementation
    public static Barrier GetInstance()
    {
        if (instance == null)
        {
            // Find the first GameObject with the "Barrier" component in the scene.
            instance = FindObjectOfType<Barrier>();
            if (instance == null)
            {
                // Create a new GameObject and add the Barrier component to it if none is found.
                GameObject barrierObject = new GameObject("Barrier");
                instance = barrierObject.AddComponent<Barrier>();
            }
        }
        return instance;
    }
}
