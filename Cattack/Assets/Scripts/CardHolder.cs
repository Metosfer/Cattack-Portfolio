using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardHolder : MonoBehaviour
{

    public int? secilenKart = null;
    public int? secilenKartQ = null;
    public int? secilenKartW = null;
    public int? secilenKartE = null;

    public KeyCode key = KeyCode.None;

    public void ShowAttackCardIndex()
    {
        if (TimeManager.Instance.currentDay == 1)
        {
            secilenKartQ = secilenKart;
        }
        else if (TimeManager.Instance.currentDay == 2)
        {
            secilenKartW = secilenKart;
        }
        else if (TimeManager.Instance.currentDay == 3)
        {
            secilenKartE = secilenKart;
        }

        secilenKart = Convert.ToInt32("1" + CardManager.Instance.attackCardIndex);
        Debug.Log(secilenKart);

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

        secilenKart = Convert.ToInt32("2" + CardManager.Instance.defenseCardIndex);

        if (TimeManager.Instance.currentDay == 1)
        {
            secilenKartQ = secilenKart;
        }
        else if (TimeManager.Instance.currentDay == 2)
        {
            secilenKartW = secilenKart;
        }
        else if (TimeManager.Instance.currentDay == 3)
        {
            secilenKartE = secilenKart;
        }

    }
    public void ShowPassiveCardIndex()
    {
        Debug.Log(CardManager.Instance.passiveCardIndex);
        Destroy(CardManager.Instance.spawned3);
        CardManager.Instance.panel.SetActive(false);
        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);

        secilenKart = Convert.ToInt32("3" + CardManager.Instance.passiveCardIndex);

        if (TimeManager.Instance.currentDay == 1)
        {
            secilenKartQ = secilenKart;
        }
        else if (TimeManager.Instance.currentDay == 2)
        {
            secilenKartW = secilenKart;
        }
        else if (TimeManager.Instance.currentDay == 3)
        {
            secilenKartE = secilenKart;
        }

    }
}