using UnityEngine;
using System;
using TMPro;
using UnityEngine.SceneManagement; // Sahne y�netimi i�in gerekli

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float cycleDuration = 60f; // Toplam d�ng� s�resi
    [SerializeField] private float dayDuration = 30f; // G�nd�z s�resi

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI dayText; // G�n say�s�n� g�sterecek TMP
    [SerializeField] private TextMeshProUGUI timeText; // Saati g�sterecek TMP

    private float currentTime = 0f;
    public int currentDay = 1; // Mevcut g�n
    public bool isNight = false;

    // Singleton pattern
    public static TimeManager Instance { get; set; }

    // Events for time changes
    public event Action OnDayStart;
    public event Action OnNightStart;
    public event Action<float> OnTimeChanged;
    public event Action<int> OnDayChanged; // Yeni g�n event'i

    private string activeSceneName;

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
        activeSceneName = SceneManager.GetActiveScene().name; // Aktif sahneyi al
        UpdateUITexts(); // UI'� ba�lang��ta g�ncelle

        // E�er BossScene sahnesindeysek, sadece gece modu aktif olsun
        if (activeSceneName == "BossScene")
        {
            isNight = true;
            OnNightStart?.Invoke(); // Gece ba�lama event'i tetiklenir
        }
    }

    private void Update()
    {
        if (activeSceneName == "BossScene")
        {
            // BossScene'de s�rekli gece modunda kal�r
            UpdateUITexts();
            return;
        }

        // Normal d�ng�
        currentTime += Time.deltaTime;

        // D�ng� tamamland���nda
        if (currentTime >= cycleDuration)
        {
            currentTime = 0f;
            currentDay++; // Yeni g�ne ge�
            OnDayChanged?.Invoke(currentDay); // G�n de�i�im event'ini tetikle
        }

        OnTimeChanged?.Invoke(currentTime);

        // Gece/G�nd�z kontrol�
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
            if (activeSceneName == "BossScene")
            {
                dayText.text = "Boss Gece Modu Aktif";
            }
            else
            {
                dayText.text = $"G�n: {currentDay}";
            }
        }

        if (timeText != null)
        {
            // Saati 24 saat format�nda g�ster
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
