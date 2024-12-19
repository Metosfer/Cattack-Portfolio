using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public static CardHolder Instance { get; set; }
    private void Awake()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowAttackCardIndex()
    {
        Debug.Log(CardManager.Instance.attackCardIndex);
        Destroy(CardManager.Instance.spawned1);
    }
    public void ShowDefenseCardIndex()
    {
        Debug.Log(CardManager.Instance.defenseCardIndex);
        Destroy(CardManager.Instance.spawned2);
    }
    public void ShowPassiveCardIndex()
    {
        Debug.Log(CardManager.Instance.passiveCardIndex);
        Destroy(CardManager.Instance.spawned3);
    }
}
