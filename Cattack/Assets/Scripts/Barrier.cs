using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int health = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Bariyer yıkıldı!");
            Destroy(gameObject);
            // Burada bariyerin yıkılmasını sağlayabilirsin.
        }
    }
}
