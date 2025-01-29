using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoWait : MonoBehaviour
{
    public StartButton startButton;
    public VideoPlayer videoPlayer;

    public void Setup(StartButton startButtons)
    {
        startButton = startButtons;
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.targetCamera = Camera.main;
        startButton.RunGameTrigger((float)videoPlayer.length);
    }
}
