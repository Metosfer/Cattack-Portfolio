using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

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

        private string activeSceneName;
        private Color bossSceneColor = new Color(0.235f, 0.074f, 0.471f); // 3C1378 renk kodunun Color formatý

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

        private void Start()
        {
            activeSceneName = SceneManager.GetActiveScene().name;

            // Eðer sahne BossScene ise, sabit rengi ayarla
            if (activeSceneName == "BossScene")
            {
                light.color = bossSceneColor;
            }
        }

        private void Update()
        {
            // Eðer sahne BossScene ise, renk deðiþimi yapma
            if (activeSceneName == "BossScene")
            {
                return;
            }

            // Diðer sahnelerde renk deðiþimini uygula
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
