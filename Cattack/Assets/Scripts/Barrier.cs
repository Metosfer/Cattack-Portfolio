// Barrier.cs
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Bariyer hasar ald�. Kalan sa�l�k: " + health);
        if (health <= 0)
        {
            Debug.Log("Bariyer y�k�ld�!");
            Destroy(gameObject);
        }
    }


}