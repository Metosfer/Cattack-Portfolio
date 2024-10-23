using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float cycleDuration = 60f; // Toplam d�ng� s�resi
    [SerializeField] private float dayDuration = 30f; // G�nd�z s�resi

    private float currentTime = 0f;
    private bool isNight = false;

    // Singleton pattern
    public static TimeManager Instance { get; private set; }

    // Events for time changes
    public event Action OnDayStart;
    public event Action OnNightStart;
    public event Action<float> OnTimeChanged; // Zamandaki her de�i�iklik i�in

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Zaman� g�ncelle
        currentTime += Time.deltaTime;

        // D�ng� s�resini a�t�ysa s�f�rla
        if (currentTime >= cycleDuration)
        {
            currentTime = 0f;
        }

        // Zaman de�i�im event'ini tetikle
        OnTimeChanged?.Invoke(currentTime);

        // Gece/G�nd�z kontrol�
        bool shouldBeNight = currentTime >= dayDuration;

        // Durum de�i�imi kontrol�
        if (shouldBeNight != isNight)
        {
            isNight = shouldBeNight;
            if (isNight)
            {
                OnNightStart?.Invoke();
                Debug.Log($"Night started at {currentTime:F1}s");
            }
            else
            {
                OnDayStart?.Invoke();
                Debug.Log($"Day started at {currentTime:F1}s");
            }
        }
    }

    // Public getter methods
    public bool IsNight() => isNight;
    public float GetCurrentTime() => currentTime;
    public float GetCycleDuration() => cycleDuration;
    public float GetDayDuration() => dayDuration;
    public float GetNightDuration() => cycleDuration - dayDuration;
    public float GetTimeUntilChange() => isNight ? cycleDuration - currentTime : dayDuration - currentTime;

    // Editor'da de�erleri de�i�tirmek i�in metodlar
    public void SetCycleDuration(float duration)
    {
        cycleDuration = Mathf.Max(duration, dayDuration);
    }

    public void SetDayDuration(float duration)
    {
        dayDuration = Mathf.Min(duration, cycleDuration);
    }

#if UNITY_EDITOR
    // Debug bilgisi
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 100));
        GUILayout.Label($"Current Time: {currentTime:F1}s");
        GUILayout.Label($"Current Period: {(isNight ? "Night" : "Day")}");
        GUILayout.Label($"Time until change: {GetTimeUntilChange():F1}s");
        GUILayout.EndArea();
    }
#endif
}