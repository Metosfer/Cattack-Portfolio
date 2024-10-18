// Barrier.cs
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Bariyer hasar aldý. Kalan saðlýk: " + health);
        if (health <= 0)
        {
            Debug.Log("Bariyer yýkýldý!");
            Destroy(gameObject);
        }
    }


}