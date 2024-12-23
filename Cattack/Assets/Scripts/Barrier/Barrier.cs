// Barrier.cs
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private static Barrier instance;
    public GameObject br;

    public GameObject barrierRender;
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Bariyer hasar ald�. Kalan sa�l�k: " + health);
        if (health <= 0)
        {
            br.SetActive(false);
            Debug.Log("Bariyer y�k�ld�!");
            Destroy(gameObject);
            Destroy(barrierRender);
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