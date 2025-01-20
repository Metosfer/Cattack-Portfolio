using System.Collections;
using UnityEngine;

public class CatSkillFX : MonoBehaviour
{
    private Animator animator;
    private Vector3 baslangicPozisyonu;
    private float baslangicYonu;
    private bool skillAktif = false;
    public AudioSource[] catAudio;

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
        catAudio[2].Play();
        TriggerSkill("playMeteor");
    }

    public void PlayHairballAnimation()
    {
        catAudio[0].Play();
        TriggerSkill("playHairball");
    }

    public void PlayHollowAnimation()
    {
        catAudio[1].Play();
        TriggerSkill("playHollow");
    }

    public void PlayMeowAnimation()
    {
        
        TriggerSkill("playMeow");
    }

    public void PlayCurseAnimation()
    {
        catAudio[3].Play();
        TriggerSkill("playCurse");
    }

    private void TriggerSkill(string triggerName)
    {
        if (animator != null)
        {
            // Skill ba�lad���nda pozisyon ve y�n� kaydet
            skillAktif = true;
            baslangicPozisyonu = transform.position;
            baslangicYonu = transform.localScale.x; // Karakterin y�n bilgisini sakla

            // Skill animasyonunu tetikle
            animator.SetTrigger(triggerName);
        }
    }

    private void Update()
    {
        if (skillAktif)
        {
            // Skill efektini ba�lang�� pozisyonunda tut
            transform.position = baslangicPozisyonu;

            // Skill efektinin y�n�n� sabit tut
            transform.localScale = new Vector3(baslangicYonu, transform.localScale.y, transform.localScale.z);
        }
    }

    // Animasyon bitti�inde �a�r�lacak fonksiyon (Animation Event olarak kullanabilirsiniz)
    public void SkillBitir()
    {
        skillAktif = false;
    }
}
