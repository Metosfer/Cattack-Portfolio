using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchSkillFXAnimationController : MonoBehaviour
{
    private Animator witchFXAnimation;

    // Start is called before the first frame update
    void Start()
    {
        witchFXAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerWitchAttack1()
    {
        if (witchFXAnimation != null)
        {
            witchFXAnimation.SetTrigger("witchAttack1");
        }
    }

    public void TriggerWitchAttack2()
    {
        if (witchFXAnimation != null)
        {
            witchFXAnimation.SetTrigger("witchAttack2");
        }
    }

    
}

