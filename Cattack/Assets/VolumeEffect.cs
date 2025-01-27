using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeEffect : MonoBehaviour
{
    public Volume globalVolume;
    private LensDistortion lensDistortion;

    public float minIntensity = -0.5f;
    public float maxIntensity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (globalVolume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            // LensDistortion component is found
        }
        else
        {
            Debug.LogError("LensDistortion component not found in the global volume profile.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Example: Change intensity based on some condition
        if (TimeManager.Instance.currentDay == TimeManager.Instance.bossDay)
        {
            float pingPongValue = Mathf.PingPong(Time.time, Mathf.Abs(minIntensity - maxIntensity));
            lensDistortion.intensity.value = Mathf.Lerp(minIntensity, maxIntensity, pingPongValue);
        }
        else
        {
            lensDistortion.intensity.value = 0.0f; // Reset intensity
        }
    }
}
