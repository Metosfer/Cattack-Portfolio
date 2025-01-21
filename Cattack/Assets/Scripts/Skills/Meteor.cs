using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public int meteorDamage = 20;

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
                skeletonAI.TakeDamage(meteorDamage);
                Debug.Log("Skeleton hit by Meteor!");
            }
            if (skeletonMageAI != null)
            {
                skeletonMageAI.TakeDamage(meteorDamage);
                Debug.Log("Skeleton Mage hit by Meteor!");
            }
            if (skeletonKnightAI != null)
            {
                skeletonKnightAI.TakeDamage(meteorDamage);
                Debug.Log("Skeleton Knight hit by Meteor!");
            }
            if (witchAI != null)
            {
                witchAI.TakeDamage(meteorDamage);
                Debug.Log("Witch hit by Meteor!");
            }
            // Çarpmadan sonra bir efekt ya da objeyi yok etme

        }
    }
}
