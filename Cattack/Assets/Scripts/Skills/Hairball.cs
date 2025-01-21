using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hairball : MonoBehaviour
{
    public int hairballDamage = 10;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Tag kontrolü
        if (collision.CompareTag("Skeleton") || collision.CompareTag("Witch"))
        {
            // Skeleton türüne göre hasar verme
            SkeletonAI skeletonAI = collision.GetComponent<SkeletonAI>();
            SkeletonMageAI skeletonMageAI = collision.GetComponent<SkeletonMageAI>();
            SkeletonKnightAI skeletonKnightAI = collision.GetComponent<SkeletonKnightAI>();
            WitchAI witchAI = collision.GetComponent<WitchAI>();

            if (skeletonAI != null)
            {
                skeletonAI.TakeDamage(hairballDamage);
                Debug.Log("Skeleton hit by Hairball!");
            }
            if (skeletonMageAI != null)
            {
                skeletonMageAI.TakeDamage(hairballDamage);
                Debug.Log("Skeleton Mage hit by Hairball!");
            }
            if (skeletonKnightAI != null)
            {
                skeletonKnightAI.TakeDamage(hairballDamage);
                Debug.Log("Skeleton Knight hit by Hairball!");
            }
            if (witchAI != null)
            {
                witchAI.TakeDamage(hairballDamage);
                Debug.Log("Witch hit by Hairball!");
            }

            // Çarpmadan sonra bir efekt ya da objeyi yok etme

        }

    }
}