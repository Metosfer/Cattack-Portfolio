using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerCombat;

public class CardHolder : MonoBehaviour
{
    private Dictionary<SkillSlot, int> skillMappings = new Dictionary<SkillSlot, int>();

    public List<GameObject> ChoosenCards;
    public static CardHolder Instance { get; set; }

    public enum SkillSlot
    {

        Q, W, E

    }
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void AssignSkill(SkillSlot slot, int cardIndex)
    {
        if (!skillMappings.ContainsKey(slot))
        {
            skillMappings[slot] = cardIndex;
            Debug.Log($"{slot} slotuna {cardIndex} indeksi atandý.");
        }
        else
        {
            Debug.LogError($"{slot} slotuna zaten bir skill atanmýþ.");
        }
    }
    public void ShowAttackCardIndex()
    {
        Debug.Log(CardManager.Instance.attackCardIndex);
        Destroy(CardManager.Instance.spawned1);
        CardManager.Instance.panel.SetActive(false);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

        if(TimeManager.Instance.currentDay == 1)
        {
            AssignSkill(SkillSlot.Q, CardManager.Instance.attackCardIndex);

        }

    }
    public void ShowDefenseCardIndex()
    {
        Debug.Log(CardManager.Instance.defenseCardIndex);
        Destroy(CardManager.Instance.spawned2);
        CardManager.Instance.panel.SetActive(false);
        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned3);
    }
    public void ShowPassiveCardIndex()
    {
        Debug.Log(CardManager.Instance.passiveCardIndex);
        Destroy(CardManager.Instance.spawned3);
        CardManager.Instance.panel.SetActive(false);
        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);
    }
}