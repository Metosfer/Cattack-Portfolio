using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int health = 10;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Bariyer y�k�ld�!");
            Destroy(gameObject);
            // Burada bariyerin y�k�lmas�n� sa�layabilirsin.
        }
    }
}
