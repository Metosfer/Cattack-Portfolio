using System.Collections;
using UnityEngine;

public class CatSkillFX : MonoBehaviour
{
    private Animator animator;
    private Quaternion baslangicRotasyonu;
    private Vector3 baslangicPozisyonu;
    private bool skillAktif = false;

    private HollowPurple hollowPurple;

    private void Start()
    {
        hollowPurple = FindAnyObjectByType<HollowPurple>();

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("CatSkillFX: Animator component not found!");
        }
    }

    public void PlayMeteorAnimation()
    {

        if (animator != null)
        {
            // Skill başladığında pozisyon ve rotasyonu kaydet
            skillAktif = true;
            //baslangicRotasyonu = transform.rotation;
            //baslangicPozisyonu = transform.localPosition;
            animator.SetTrigger("playMeteor");
            animator.SetBool("skillFinished", true);
        }
    }
    public void PlayHairballAnimation()
    {
        if (animator != null)
        {
            // Skill başladığında pozisyon ve rotasyonu kaydet
            skillAktif = true;
            //baslangicRotasyonu = transform.rotation;
            //baslangicPozisyonu = transform.localPosition;

            animator.SetTrigger("playHairball");
            animator.SetBool("skillFinished", true);
        }
    }
    public void PlayHollowAnimation()
    {
        if (animator != null)
        {
            // Skill başladığında pozisyon ve rotasyonu kaydet
            skillAktif = true;
            //baslangicRotasyonu = transform.rotation;
            //baslangicPozisyonu = transform.localPosition;
            animator.SetTrigger("playHollow");
            animator.SetBool("skillFinished", true);
        }
    }
    public void PlayMeowAnimation()
    {
        if (animator != null)
        {
            // Skill başladığında pozisyon ve rotasyonu kaydet
            skillAktif = true;
            //baslangicRotasyonu = transform.rotation;
            //baslangicPozisyonu = transform.localPosition;

            animator.SetTrigger("playMeow");
        }
    }

    private void Update()
    {
        if (skillAktif)
        {
            // Skill efektinin rotasyonunu ve pozisyonunu sabit tut
            transform.rotation = baslangicRotasyonu;
            transform.localPosition = baslangicPozisyonu;
        }
    }

    // Animasyon bittiğinde çağrılacak fonksiyon (Animation Event olarak kullanabilirsiniz)
    public void SkillBitir()
    {
        skillAktif = false;
    }
}
