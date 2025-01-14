using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowPurple : MonoBehaviour
{
    public int hollowDamage = 15;

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
                skeletonAI.TakeDamage(hollowDamage);
                Debug.Log("Skeleton hit by Hollow Purple!");
            }
            if (skeletonMageAI != null)
            {
                skeletonMageAI.TakeDamage(hollowDamage);
                Debug.Log("Skeleton Mage hit by Hollow Purple!");
            }
            if (skeletonKnightAI != null)
            {
                skeletonKnightAI.TakeDamage(hollowDamage);
                Debug.Log("Skeleton Knight hit by Hollow Purple!");
            }

            // Çarpmadan sonra bir efekt ya da objeyi yok etme
            Destroy(gameObject); // Eðer Hollow Purple yok edilecekse
        }
    }
}
