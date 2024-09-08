using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum EnemyType
{
    Plane,
    Blimp
}

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public EnemyWave[] Waves { get => waves; }
    public int CurrentWave { get; private set; } = -1;
    public List<Enemy> SpawnedEnemies { get => spawnedEnemies; }

    public bool GameComplete { get; private set; }

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private EnemyWave[] waves;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private float nextWaveEnemyPercentage = 0.25f;
    [Space]
    [SerializeField] private int maxHorizontalSpawnRadius = 500;
    [SerializeField] private int maxVerticalSpawnRadius = 200;
    [SerializeField] private int verticalSpawnOffset = 750;
    [SerializeField] private int minSpawnDistToPlayer = 100;
    [Space]
    [SerializeField] private GameObject waveMenu;
    [Space]
    [SerializeField] private bool debug = false;

    private List<Enemy> spawnedEnemies = new List<Enemy>();
    private int spawnedEnemyCount = 0;

    void Awake()
    {
        Instance = this;
    }

    public void StartNewWave()
    {
        CurrentWave++;

        if (CurrentWave >= waves.Length)
        {
            GameComplete = true;
            return;
        }

        EnemyWave wave = waves[CurrentWave];
        List<EnemyType> waveEnemies = wave.CreateWave();

        for (int i = 0; i < waveEnemies.Count; i++)
        {
            EnemyType type = waveEnemies[i];
            GameObject prefab = enemyPrefabs[(int)type];
            Vector3 spawnPos = new Vector3(Random.Range(-maxHorizontalSpawnRadius, maxHorizontalSpawnRadius), Random.Range(-maxVerticalSpawnRadius + verticalSpawnOffset, maxVerticalSpawnRadius + verticalSpawnOffset), Random.Range(-maxHorizontalSpawnRadius, maxHorizontalSpawnRadius));
            Vector3 spawnRot = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.Euler(spawnRot), enemyParent);
            if (!enemy.GetComponent<Enemy>().ShouldPitch)
            {
                enemy.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            }

            if ((spawnPos - Player.Instance.transform.position).magnitude < minSpawnDistToPlayer)
            {
                Destroy(enemy);
                i--;
                continue;
            }

            spawnedEnemies.Add(enemy.GetComponent<Enemy>());
        }

        spawnedEnemyCount = spawnedEnemies.Count;
        StartCoroutine(WaveMenuDisplay(wave));
    }

    private IEnumerator WaveMenuDisplay(EnemyWave wave)
    {
        waveMenu.GetComponentInChildren<TMP_Text>().text = "Wave " + (CurrentWave + 1) + ": " + wave.WaveName;
        waveMenu.GetComponentInChildren<FadeUI>().Display(true);
        yield return new WaitForSeconds(5f);
        waveMenu.GetComponentInChildren<FadeUI>().Display(false);
    }

    void Start()
    {
        if (debug)
        {
            StartNewWave();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debug && Input.GetKeyDown(KeyCode.N))
        {
            StartNewWave();
        }

        if (((float)spawnedEnemies.Count / (float)spawnedEnemyCount) < nextWaveEnemyPercentage)
        {
            StartNewWave();
        }
    }
}
