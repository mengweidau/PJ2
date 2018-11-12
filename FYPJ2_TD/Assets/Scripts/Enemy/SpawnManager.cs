using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;

    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    };

    [System.Serializable]
    // Wave class to determine what type of enemy, amount of enemy, rate of spawning in that specific wave.
    // Current testing phase: Preset enemy/spawn/rate .
    // Later on, Todo: Random enemy/spawn/rate etc etc..
    public class Wave   
    {
        public GameObject enemyPrefab;
        public int spawnAmount;
        public float rateOfSpawn;
    }

    public Wave[] wave;
    private int nextWave = 0;
    public float timeBetweenWaves = 5.0f;
    public float waveCountdown;

    public SpawnState state = SpawnState.COUNTING;
    private float searchCountdown = 1.0f;

    // Use this for initialization
    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for the state to determine on whether to spawn enemies
        if (state == SpawnState.WAITING)
        {
            // Check for status of current wave enemy
            if (EnemyisAlive() == false)
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                Debug.Log("Spawning next wave");
                StartCoroutine(SpawnWave(wave[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        for (int i = 0; i < _wave.spawnAmount; i++)
        {
            SpawnEnemy(_wave.enemyPrefab);
            yield return new WaitForSeconds(1.0f / _wave.rateOfSpawn);
        }
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(GameObject _enemy)
    {
        Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
    }

    bool EnemyisAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 1.0f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        // If wave exceed amount, reset to 0 / next level
        if (nextWave + 1 > wave.Length - 1)
        {
            nextWave = 0;
        }
        else
        {
            nextWave++;
        }
    }
}
