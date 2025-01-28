using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableEnemy
    {
        public GameObject enemyPrefab;
        public string enemyName;
        [Range(0f, 1f)]
        public float spawnChance = 1f;
    }

    [Header("Spawn Settings")]
    [SerializeField] private SpawnableEnemy[] spawnableEnemies;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int baseMaxEnemies = 5; // Temel maksimum düþman sayýsý
    [SerializeField] private Dictionary<int, int> dayToMaxEnemies = new Dictionary<int, int>(); // Günlere göre maksimum düþman sayýsý

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine spawnCoroutine;
    private TimeManager timeManager;
    private int maxEnemies; // Güncellenmiþ maksimum düþman sayýsý
    private float currentSpawnInterval; // Güncellenmiþ spawn aralýðý

    public static GameManager Instance { get; set; }
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
        if (spawnableEnemies.Length == 0)
        {
            Debug.LogError("No spawnable enemies assigned!");
            return;
        }

        // TimeManager'a baðlan
        timeManager = TimeManager.Instance;
        if (timeManager == null)
        {
            Debug.LogError("TimeManager not found in scene!");
            return;
        }

        // TimeManager event'lerine abone ol
        timeManager.OnDayStart += OnDayStarted;
        timeManager.OnNightStart += OnNightStarted;
        timeManager.OnDayChanged += OnDayChanged; // Gün deðiþim event'ine abone ol

        // Baþlangýçta maxEnemies ve spawnInterval deðerini ayarla
        maxEnemies = baseMaxEnemies;
        currentSpawnInterval = spawnInterval;
    }

    private void OnDestroy()
    {
        // Event aboneliklerini temizle
        if (timeManager != null)
        {
            timeManager.OnDayStart -= OnDayStarted;
            timeManager.OnNightStart -= OnNightStarted;
            timeManager.OnDayChanged -= OnDayChanged; // Gün deðiþim event'inden çýk
        }
    }

    private void OnDayStarted()
    {
        StopSpawning();
        // Kart seçimi yapýlabilir
        CardManager.Instance.canSelectCard = true;
    }

    private void OnNightStarted()
    {
        StartSpawning();
        // Kart seçimi yapýlamaz
        CardManager.Instance.canSelectCard = false;
    }

    private void OnDayChanged(int currentDay)
    {
        // Gün deðiþtiðinde maksimum düþman sayýsýný ve spawn aralýðýný güncelle
        if (dayToMaxEnemies.ContainsKey(currentDay))
        {
            maxEnemies = dayToMaxEnemies[currentDay];
        }
        else
        {
            maxEnemies = baseMaxEnemies + (currentDay - 1) * 5; // Varsayýlan olarak her gün 5 tane daha fazla düþman
        }

        // Spawn aralýðýný her gün biraz daha azaltarak düþman spawn hýzýný artýr
        currentSpawnInterval = Mathf.Max(1f, spawnInterval - (currentDay - 1) * 0.1f);
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

        // Tüm aktif düþmanlarý temizle
        foreach (GameObject enemy in activeEnemies.ToArray())
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (activeEnemies.Count < maxEnemies && timeManager.canSpawnSkeletons)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomSpawnPoint];

        SpawnableEnemy enemyToSpawn = SelectRandomEnemy();
        if (enemyToSpawn == null || enemyToSpawn.enemyPrefab == null) return;

        GameObject newEnemy = Instantiate(enemyToSpawn.enemyPrefab,
                                        selectedSpawnPoint.position,
                                        Quaternion.identity);

        Vector3 position = newEnemy.transform.position;
        position.z = 0;
        newEnemy.transform.position = position;

        activeEnemies.Add(newEnemy);
    }

    private SpawnableEnemy SelectRandomEnemy()
    {
        float totalChance = 0f;
        foreach (SpawnableEnemy enemy in spawnableEnemies)
        {
            totalChance += enemy.spawnChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float currentChance = 0f;

        foreach (SpawnableEnemy enemy in spawnableEnemies)
        {
            currentChance += enemy.spawnChance;
            if (randomValue <= currentChance)
            {
                return enemy;
            }
        }

        return spawnableEnemies[0];
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos || spawnPoints == null) return;

        Gizmos.color = Color.red;
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != null)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
            }
        }
    }
}
