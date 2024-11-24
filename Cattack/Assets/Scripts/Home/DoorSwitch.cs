using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class DoorSwitch : MonoBehaviour
{
    [SerializeField] float maxOrthoSize = 9.56f;
    [SerializeField] float minOrthoSize = 8.5f;
    [SerializeField] float transationTime = 2f;

    [SerializeField] CinemachineVirtualCamera playerCam;
    

    void Start()
    {
       
    }

    void Update()
    {
        if (playerCam.m_Lens.OrthographicSize >= minOrthoSize && TimeManager.Instance.isNight == true)
        {
            playerCam.m_Lens.OrthographicSize = Mathf.LerpAngle(minOrthoSize, maxOrthoSize, transationTime);
        }

        if (playerCam.m_Lens.OrthographicSize >= minOrthoSize && HomeBorderSwitch.Instance.isPlayerInside == false)
        {
            playerCam.m_Lens.OrthographicSize = Mathf.LerpAngle(minOrthoSize, maxOrthoSize, transationTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TimeManager.Instance.isNight == false && collision.CompareTag("Player"))
        {
            playerCam.m_Lens.OrthographicSize = Mathf.LerpAngle(maxOrthoSize, minOrthoSize, transationTime);
           
        }
    }
}