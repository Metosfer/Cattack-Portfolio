using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class ItemManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableItem
    {
        public GameObject itemPrefab;
        public string itemName;
        [Range(0f, 1f)]
        public float spawnChance = 1f;
    }

    [Header("Item Spawner Settings")]
    [SerializeField] private SpawnableItem[] spawnableItems;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float maxItem = 5f;

    private List<GameObject> activeItems = new List<GameObject>();
    private Coroutine spawnCoroutine;
    private TimeManager timeManager;

    private void Start()
    {
        if (spawnableItems.Length == 0)
        {
            Debug.LogError("No spawnable items assigned!");
            return;
        }

        // TimeManager'a bağlan
        timeManager = TimeManager.Instance;
        if (timeManager == null)
        {
            Debug.LogError("TimeManager not found in scene!");
            return;
        }

        // TimeManager event'lerine abone ol
        timeManager.OnDayStart += OnDayStarted;
        timeManager.OnNightStart += OnNightStarted;
    }

    private void OnDestroy()
    {
        // Event aboneliklerini temizle
        if (timeManager != null)
        {
            timeManager.OnDayStart -= OnDayStarted;
            timeManager.OnNightStart -= OnNightStarted;
        }
    }

    private void OnDayStarted()
    {
        StartSpawning();
    }

    private void OnNightStarted()
    {
        StopSpawning();
    }

    private void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    private void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // Tüm aktif itemları temizle
        foreach (GameObject item in activeItems.ToArray())
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        activeItems.Clear();
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (activeItems.Count < maxItem)
            {
                SpawnItem();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnItem()
    {
        if (spawnPoints.Length < 2)
        {
            Debug.LogWarning("En az 2 spawn noktası gereklidir!");
            return;
        }

        // Rastgele iki ardışık spawn noktası seç
        int randomIndex = Random.Range(0, spawnPoints.Length - 1);
        Transform spawnPoint1 = spawnPoints[randomIndex];
        Transform spawnPoint2 = spawnPoints[randomIndex + 1];

        // İki nokta arasında rastgele bir pozisyon belirle
        float randomT = Random.Range(0f, 1f);
        Vector3 spawnPosition = Vector3.Lerp(spawnPoint1.position, spawnPoint2.position, randomT);

        // Z pozisyonunu sıfırla
        spawnPosition.z = 0;

        SpawnableItem itemToSpawn = SelectRandomItem();
        if (itemToSpawn == null || itemToSpawn.itemPrefab == null) return;

        GameObject newItem = Instantiate(itemToSpawn.itemPrefab, spawnPosition, Quaternion.identity);
        activeItems.Add(newItem);
    }

    private SpawnableItem SelectRandomItem()
    {
        float totalChance = 0f;
        foreach (SpawnableItem item in spawnableItems)
        {
            totalChance += item.spawnChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float currentChance = 0f;

        foreach (SpawnableItem item in spawnableItems)
        {
            currentChance += item.spawnChance;
            if (randomValue <= currentChance)
            {
                return item;
            }
        }

        return spawnableItems[0];
    }

    void Update()
    {
        // Update metodu gerekirse burada kullanılabilir
    }
}