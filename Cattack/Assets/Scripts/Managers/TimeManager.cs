using UnityEngine;
using System;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float cycleDuration = 60f; // Toplam döngü süresi
    [SerializeField] private float dayDuration = 30f; // Gündüz süresi

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dayText; // Gün sayýsýný gösterecek TMP
    [SerializeField] private TextMeshProUGUI timeText; // Saati gösterecek TMP

    private float currentTime = 0f;
    private int currentDay = 1; // Mevcut gün
    public bool isNight = false;

    // Singleton pattern
    public static TimeManager Instance { get; set; }

    // Events for time changes
    public event Action OnDayStart;
    public event Action OnNightStart;
    public event Action<float> OnTimeChanged;
    public event Action<int> OnDayChanged; // Yeni gün event'i

    private void Awake()
    {
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

    private void Start()
    {
        UpdateUITexts(); // UI'ý baþlangýçta güncelle
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        // Döngü tamamlandýðýnda
        if (currentTime >= cycleDuration)
        {
            currentTime = 0f;
            currentDay++; // Yeni güne geç
            OnDayChanged?.Invoke(currentDay); // Gün deðiþim event'ini tetikle
        }

        OnTimeChanged?.Invoke(currentTime);

        // Gece/Gündüz kontrolü
        bool shouldBeNight = currentTime >= dayDuration;

        if (shouldBeNight != isNight)
        {
            isNight = shouldBeNight;
            if (isNight)
            {
                CardManager.Instance.nightCheck = true;
                OnNightStart?.Invoke();
            }
            else
            {
                CardManager.Instance.nightCheck = false;
                OnDayStart?.Invoke();
            }
        }

        UpdateUITexts();
    }

    private void UpdateUITexts()
    {
        if (dayText != null)
        {
            dayText.text = $"Gün: {currentDay}";
        }

        if (timeText != null)
        {
            // Saati 24 saat formatýnda göster
            float timeOfDay = (currentTime / cycleDuration) * 24f;
            int hours = Mathf.FloorToInt(timeOfDay);
            int minutes = Mathf.FloorToInt((timeOfDay - hours) * 60);
            timeText.text = $"Saat: {hours:00}:{minutes:00}";
        }
    }

    // Public getter methods
    public bool IsNight() => isNight;
    public float GetCurrentTime() => currentTime;
    public float GetCycleDuration() => cycleDuration;
    public float GetDayDuration() => dayDuration;
    public float GetNightDuration() => cycleDuration - dayDuration;
    public float GetTimeUntilChange() => isNight ? cycleDuration - currentTime : dayDuration - currentTime;
    public int GetCurrentDay() => currentDay; // Yeni getter

    // Setter methods
    public void SetCycleDuration(float duration)
    {
        cycleDuration = Mathf.Max(duration, dayDuration);
    }

    public void SetDayDuration(float duration)
    {
        dayDuration = Mathf.Min(duration, cycleDuration);
    }

    public void SetDay(int day) // Yeni setter
    {
        currentDay = Mathf.Max(1, day);
        OnDayChanged?.Invoke(currentDay);
        UpdateUITexts();
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 200, 100));
        GUILayout.Label($"Current Time: {currentTime:F1}s");
        GUILayout.Label($"Current Day: {currentDay}");
        GUILayout.Label($"Current Period: {(isNight ? "Night" : "Day")}");
        GUILayout.Label($"Time until change: {GetTimeUntilChange():F1}s");
        GUILayout.EndArea();
    }
#endif
}