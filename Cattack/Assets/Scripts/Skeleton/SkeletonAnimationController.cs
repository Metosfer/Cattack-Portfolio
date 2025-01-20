using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationController : MonoBehaviour
{
    private SkeletonAI skeletonAI;
    private SkeletonMageAI skeletonMageAI;
    private SkeletonKnightAI skeletonKnightAI;
    private Animator animator;
    private bool isAttacking = false;

    private void Awake()
    {
        skeletonAI = GetComponent<SkeletonAI>();
        skeletonMageAI = GetComponent<SkeletonMageAI>();
        skeletonKnightAI = GetComponent<SkeletonKnightAI>();
        animator = GetComponent<Animator>();
    }

    public void DeathAnimationHandler()
    {
        animator.SetBool("isDead", true);
    }

    public void WalkingAnimationHandler(bool isWalking)
    {
        // Eðer saldýrý animasyonu oynatýlmýyorsa walk animasyonunu ayarla
        if (!isAttacking)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }

    public void AttackAnimationHandler()
    {
        StartCoroutine(AttackAnimationCoroutine());
    }

    private IEnumerator AttackAnimationCoroutine()
    {
        isAttacking = true;
        animator.SetBool("isWalking", false);
        animator.SetTrigger("isAttacking");

        // Saldýrý animasyonunun süresini al
        float attackAnimationLength = GetAttackAnimationLength();

        yield return new WaitForSeconds(attackAnimationLength);

        isAttacking = false;

        // Saldýrý bittikten sonra eðer Mage hala hareket ediyorsa walk animasyonunu tekrar baþlat
        if (skeletonMageAI != null && skeletonMageAI.isWalking)
        {
            animator.SetBool("isWalking", true);
        }
    }

    private float GetAttackAnimationLength()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name.Contains("Attack"))
            {
                return clip.length;
            }
        }

        return 1f; // Varsayýlan süre
    }
}