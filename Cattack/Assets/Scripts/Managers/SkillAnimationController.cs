using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimationController : MonoBehaviour
{
    public Animator animator;
    public static SkillAnimationController Instance { get; set; }
    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (Instance == null)
            Instance = this;

        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Start()
    {

    }
    //Hairball effect
    public void SetHairballEffect()
    {
        animator.SetTrigger("hairballThrow");
    }
    // Update is called once per frame
    void Update()
    {

    }

}
