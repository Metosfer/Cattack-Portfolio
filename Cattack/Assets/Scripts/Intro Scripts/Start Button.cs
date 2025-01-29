using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public Transform Cutscene;
    public Transform Canvas;
    public void StartGame()
    {
        GameObject xd = Instantiate(Cutscene).gameObject;
        VideoWait videoWait = xd.GetComponent<VideoWait>();
        videoWait.Setup(gameObject.GetComponent<StartButton>());
        Canvas.gameObject.SetActive(false);
    }

    public void RunGame()
    {
        SceneManager.LoadScene(1);
    }

    public void RunGameTrigger(float time)
    {
        Debug.Log(time);
        Invoke(nameof(RunGame),time);
        
    }

}
