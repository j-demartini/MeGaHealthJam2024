using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class EnemyWave : MonoBehaviour
{
    public float DifficultyMultiplier { get => difficultyMultiplier; }
    public string WaveName { get => waveName; }

    [SerializeField] private string waveName;
    [SerializeField] private EnemyType[] enemyTypes;

    [Tooltip("The probability of each enemy type spawning, MUST ADD TO 1")]
    [Range(0, 1)]
    [SerializeField] private float[] spawnProbabilities;
    [SerializeField] private float difficultyMultiplier = 1f;

    [SerializeField] private int minEnemyCount;
    [SerializeField] private int maxEnemyCount;

    public List<EnemyType> CreateWave()
    {
        int spawnCount = Random.Range(minEnemyCount, maxEnemyCount);

        List<EnemyType> wave = new List<EnemyType>();

        for (int i = 0; i < spawnCount; i++)
        {
            float randomValue = Random.value;
            float cumulativeProbability = 0;
            for (int j = 0; j < enemyTypes.Length; j++)
            {
                cumulativeProbability += spawnProbabilities[j];
                if (randomValue <= cumulativeProbability)
                {
                    wave.Add(enemyTypes[j]);
                    break;
                }
            }
        }

        return wave;
    }
}
