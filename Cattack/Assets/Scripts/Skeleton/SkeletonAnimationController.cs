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
        animator = GetComponent<Animator>(); // Animator bile�enini al
    }

    void Start()
    {
        // �ste�e ba�l� olarak ba�lang��ta yap�lacak i�lemler
    }

    // Update is called once per frame
    void Update()
    {
        // Her karede yap�lacak i�lemler
    }

    public void DeathAnimationHandler()
    {
        animator.SetBool("isDead", true);
    }

    public void WalkingAnimationHandler(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking); // Y�r�me animasyonunu ba�lat veya durdur
    }


    public void AttackAnimationHandler()
    {
        animator.SetTrigger("isAttacking");
    }

}
