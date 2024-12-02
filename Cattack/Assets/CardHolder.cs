using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    // Start is called before the first frame update
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
        Destroy(CardManager.Instance.panel);
    }
    public void ShowDefenseCardIndex()
    {
        Debug.Log(CardManager.Instance.defenseCardIndex);
        Destroy(CardManager.Instance.panel);
    }
    public void ShowPassiveCardIndex()
    {
        Debug.Log(CardManager.Instance.passiveCardIndex);
        Destroy(CardManager.Instance.panel);
    }
}
