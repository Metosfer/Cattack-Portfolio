using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace WorldTime
{
    [RequireComponent(typeof(Light2D))]
    public class WorldLight : MonoBehaviour
    {
        private static WorldLight instance; // Singleton instance variable

        public float duration = 5f;

        [SerializeField] private Gradient gradient;
        private Light2D light;
        public float startTime;



        private void Awake()
        {
            light = GetComponent<Light2D>();
            startTime = Time.time;

            // Ensure only one WorldLight instance exists
            if (instance != null && instance != this)
            {
                Destroy(gameObject); // Destroy duplicates
                return;
            }

            instance = this; // Set the current instance
        }


        private void Update()
        {
            // Calculate the time elapsed since the start
            var timeElapsed = Time.time - startTime;

            // Normalize the time to a percentage between 0 and 1
            float percentage = Mathf.Sin(Mathf.PI * 2 * timeElapsed / duration) * 0.5f + 0.5f;
            percentage = Mathf.Clamp01(percentage);

            // Set the light's color using the gradient
            light.color = gradient.Evaluate(percentage);
        }

        // Static method to access the instance
        public static WorldLight GetInstance()
        {
            return instance;
        }
    }

}