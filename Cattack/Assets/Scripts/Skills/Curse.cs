using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : MonoBehaviour
{
    public int curseDamage = 5;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Tag kontrolü
        if (collision.CompareTag("Skeleton"))
        {
            // Skeleton türüne göre hasar verme
            SkeletonAI skeletonAI = collision.GetComponent<SkeletonAI>();
            SkeletonMageAI skeletonMageAI = collision.GetComponent<SkeletonMageAI>();
            SkeletonKnightAI skeletonKnightAI = collision.GetComponent<SkeletonKnightAI>();

            if (skeletonAI != null)
            {
                skeletonAI.TakeDamage(curseDamage);
                Debug.Log("Skeleton hit by Curse!");
            }
            if (skeletonMageAI != null)
            {
                skeletonMageAI.TakeDamage(curseDamage);
                Debug.Log("Skeleton Mage hit by Curse!");
            }
            if (skeletonKnightAI != null)
            {
                skeletonKnightAI.TakeDamage(curseDamage);
                Debug.Log("Skeleton Knight hit by Curse!");
            }

            // Çarpmadan sonra bir efekt ya da objeyi yok etme

        }
    }
}
