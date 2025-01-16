using System.Collections;
using UnityEngine;

public class CatSkillFX : MonoBehaviour
{
    private Animator animator;
    private Vector3 baslangicPozisyonu;
    private float baslangicYonu;
    private bool skillAktif = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("CatSkillFX: Animator component not found!");
        }
    }

    public void PlayMeteorAnimation()
    {
        TriggerSkill("playMeteor");
    }

    public void PlayHairballAnimation()
    {
        TriggerSkill("playHairball");
    }

    public void PlayHollowAnimation()
    {
        TriggerSkill("playHollow");
    }

    public void PlayMeowAnimation()
    {
        TriggerSkill("playMeow");
    }

    public void PlayCurseAnimation()
    {
        TriggerSkill("playCurse");
    }

    private void TriggerSkill(string triggerName)
    {
        if (animator != null)
        {
            // Skill baþladýðýnda pozisyon ve yönü kaydet
            skillAktif = true;
            baslangicPozisyonu = transform.position;
            baslangicYonu = transform.localScale.x; // Karakterin yön bilgisini sakla

            // Skill animasyonunu tetikle
            animator.SetTrigger(triggerName);
        }
    }

    private void Update()
    {
        if (skillAktif)
        {
            // Skill efektini baþlangýç pozisyonunda tut
            transform.position = baslangicPozisyonu;

            // Skill efektinin yönünü sabit tut
            transform.localScale = new Vector3(baslangicYonu, transform.localScale.y, transform.localScale.z);
        }
    }

    // Animasyon bittiðinde çaðrýlacak fonksiyon (Animation Event olarak kullanabilirsiniz)
    public void SkillBitir()
    {
        skillAktif = false;
    }
}
