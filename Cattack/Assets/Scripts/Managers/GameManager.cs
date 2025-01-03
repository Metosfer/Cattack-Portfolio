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
    [SerializeField] private int maxEnemies = 5;

    [Header("Debug")]
    [SerializeField] private bool showGizmos = true;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine spawnCoroutine;
    private TimeManager timeManager;

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

        // TimeManager'a ba�lan
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
        StopSpawning();
        //CardManager.Instance.isCardsOpened = false;
    }

    private void OnNightStarted()
    {
        StartSpawning();
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

        // T�m aktif d��manlar� temizle
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
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
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