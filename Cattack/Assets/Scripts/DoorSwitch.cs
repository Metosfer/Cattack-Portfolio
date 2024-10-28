using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class DoorSwitch : MonoBehaviour
{
    
    [SerializeField] CinemachineVirtualCamera playerCam;


    void Start()
    {
        
    }

    
    void FixedUpdate()
    {


            if (playerCam.m_Lens.OrthographicSize == 5f && TimeManager.Instance.isNight == true)
        {
            playerCam.m_Lens.OrthographicSize = 9.56f;
        }


            if(playerCam.m_Lens.OrthographicSize == 5f && HomeBorderSwitch.Instance.isPlayerInside==false)
        {
            playerCam.m_Lens.OrthographicSize = 9.56f;
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TimeManager.Instance.isNight == false && collision.CompareTag("Player"))
        {

            playerCam.m_Lens.OrthographicSize = 5f;
        }

      }
    }

    



