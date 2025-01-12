using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationController : MonoBehaviour
{
    private SkeletonAI skeletonAI;
    private SkeletonMageAI skeletonMageAI;
    private SkeletonKnightAI skeletonKnightAI;

    private Animator animator;

    private void Awake()
    {
        skeletonAI = GetComponent<SkeletonAI>();
        skeletonMageAI = GetComponent<SkeletonMageAI>();
        skeletonKnightAI = GetComponent<SkeletonKnightAI>();
        animator = GetComponent<Animator>(); // Animator bileþenini al
    }

    void Start()
    {
        // Ýsteðe baðlý olarak baþlangýçta yapýlacak iþlemler
    }

    // Update is called once per frame
    void Update()
    {
        // Her karede yapýlacak iþlemler
    }

    public void DeathAnimationHandler()
    {
        animator.SetBool("isDead", true);
    }

    public void WalkingAnimationHandler(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking); // Yürüme animasyonunu baþlat veya durdur
    }


    public void AttackAnimationHandler()
    {
        animator.SetTrigger("isAttacking");
    }

}
