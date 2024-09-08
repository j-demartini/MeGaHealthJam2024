using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private EnemyWave[] waves;
    [SerializeField] private Transform enemyParent;
    [Space]
    [SerializeField] private int maxHorizontalSpawnRadius = 500;
    [SerializeField] private int maxVerticalSpawnRadius = 100;

    private List<Enemy> spawnedEnemies = new List<Enemy>();

    void Awake()
    {
        Instance = this;
    }

    public void StartNewWave()
    {
        CurrentWave++;
        EnemyWave wave = waves[CurrentWave];
        List<EnemyType> waveEnemies = wave.CreateWave();

        foreach (EnemyType type in waveEnemies)
        {
            GameObject prefab = enemyPrefabs[(int)type];
            Vector3 spawnPos = new Vector3(Random.Range(-maxHorizontalSpawnRadius, maxHorizontalSpawnRadius), Random.Range(-maxVerticalSpawnRadius, maxVerticalSpawnRadius), Random.Range(-maxHorizontalSpawnRadius, maxHorizontalSpawnRadius));
            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity, enemyParent);
            spawnedEnemies.Add(enemy.GetComponent<Enemy>());
        }
    }

    void Start()
    {
        StartNewWave();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
