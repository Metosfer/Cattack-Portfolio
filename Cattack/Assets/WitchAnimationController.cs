using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchAnimationController : MonoBehaviour
{
    public Animator witchAnimator;
    private WitchSkillFXAnimationController witchSkillFX;

    // Start is called before the first frame update
    void Start()
    {
        witchSkillFX = FindAnyObjectByType<WitchSkillFXAnimationController>();
        witchAnimator = GetComponent<Animator>();
    }

    public void SetPlayerAttack1()
    {
        witchAnimator.SetTrigger("witchAttack1");
        witchSkillFX.TriggerWitchAttack1();
    }

    public void SetPlayerAttack2()
    {
        witchAnimator.SetTrigger("witchAttack2");
        witchSkillFX.TriggerWitchAttack2();
    }

    public void SetPlayerAttack3()
    {
        witchAnimator.SetTrigger("witchAttack3");
        
    }

    public void SetPlayerAttack4()
    {
        witchAnimator.SetTrigger("witchAttack4");
        
    }

    public void SetPlayerSkillFinished()
    {
        witchAnimator.SetTrigger("isAttacking");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

