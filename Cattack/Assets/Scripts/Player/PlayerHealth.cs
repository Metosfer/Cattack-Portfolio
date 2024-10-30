using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 30;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Oyuncu �ld�!");
            Dead();
            // Oyuncunun �lme mekanizmas�n� burada ekleyebilirsin.
        }
    }

    private void Dead()
    {
        Destroy(gameObject);
        Time.timeScale = 0f;
    }
}
