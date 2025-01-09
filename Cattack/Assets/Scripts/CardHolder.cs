using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardHolder : MonoBehaviour
{

    public int secilenKart = 9; //Listedeki null eleman yerine bo� index olarak kullan�l�yor.


    public KeyCode key = KeyCode.None;

    

    public void S�f�rKart()
    {
        secilenKart = 0;
        

        CardManager.Instance.panel.SetActive(false);

        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

        Debug.Log(secilenKart);
    }
    public void BirKart()
    {
        secilenKart = 1;


        CardManager.Instance.panel.SetActive(false);

        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

        Debug.Log(secilenKart);
    }
    public void IkiKart()
    {
        secilenKart = 2;


        CardManager.Instance.panel.SetActive(false);

        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

        Debug.Log(secilenKart);
    }
    public void UcKart()
    {
        secilenKart = 3;


        CardManager.Instance.panel.SetActive(false);

        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

        Debug.Log(secilenKart);
    }
    public void DortKart()
    {
        secilenKart = 4;


        CardManager.Instance.panel.SetActive(false);

        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

        Debug.Log(secilenKart);
    }
    public void BesKart()
    {
        secilenKart = 5;


        CardManager.Instance.panel.SetActive(false);

        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

        Debug.Log(secilenKart);
    }
    public void AltiKart()
    {
        secilenKart = 6;


        CardManager.Instance.panel.SetActive(false);

        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

        Debug.Log(secilenKart);
    }
    public void YediKart()
    {
        secilenKart = 7;


        CardManager.Instance.panel.SetActive(false);

        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

        Debug.Log(secilenKart);
    }
    public void SekizKart()
    {
        secilenKart = 8;


        CardManager.Instance.panel.SetActive(false);

        Destroy(CardManager.Instance.spawned1);
        Destroy(CardManager.Instance.spawned2);
        Destroy(CardManager.Instance.spawned3);

        Debug.Log(secilenKart);
    }

}