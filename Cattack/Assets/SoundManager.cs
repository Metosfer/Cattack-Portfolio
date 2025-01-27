using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Boss Sound")]
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (TimeManager.Instance.currentDay == TimeManager.Instance.bossDay)
        { 
         audioSource.Play();


        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
