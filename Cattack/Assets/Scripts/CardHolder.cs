using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public List<GameObject> ChoosenCards;
    public static CardHolder Instance { get; set; }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
    public void ShowAttackCardIndex()
    {
        Debug.Log(CardManager.Instance.attackCardIndex);
        Destroy(CardManager.Instance.spawned1);
        CardManager.Instance.panel.SetActive(false);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

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