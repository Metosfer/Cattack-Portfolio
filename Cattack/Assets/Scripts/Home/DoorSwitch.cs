using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class DoorSwitch : MonoBehaviour
{
    public float maxOrthoSize = 11f;
    public float minOrthoSize = 9.5f;
    public float transationTime = 2f;

    [SerializeField] CinemachineVirtualCamera playerCam;

    private bool isTransitioning = false;
    private float targetOrthoSize;

    void Start()
    {

    }

    public void Update()
    {
        if (isTransitioning)
        {
            playerCam.m_Lens.OrthographicSize = Mathf.Lerp(playerCam.m_Lens.OrthographicSize, targetOrthoSize, Time.deltaTime / transationTime);
            if (Mathf.Abs(playerCam.m_Lens.OrthographicSize - targetOrthoSize) < 0.01f)
            {
                playerCam.m_Lens.OrthographicSize = targetOrthoSize;
                isTransitioning = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TimeManager.Instance.isNight == false && collision.CompareTag("Player"))
        {
            targetOrthoSize = minOrthoSize;
            isTransitioning = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetOrthoSize = maxOrthoSize;
            isTransitioning = true;
        }
    }
}
