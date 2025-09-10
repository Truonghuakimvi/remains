using System.Collections;
using System.Threading;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive = 0;

    public int totalEnemy;

    public static int enemyDefeat;

    public Wave[] waves;

    public float timeBetweenWave = 15f;
    public float countdown = 1f;

    private int waveIndex = 0;
    public GameManager manager;

    private float timeout;

    void Start()
    {
        EnemiesAlive = 0;

        foreach (Wave wave in waves)
        {
            totalEnemy += wave.count;
        }

        enemyDefeat = 0;

        timeout = 0;
    }

    void Update()
    {
        if (GameManager.gameEnded)
        {
            enabled = false;
        }

        if (timeout > 0)
        {
            timeout -= Time.deltaTime;
        }

        if (EnemiesAlive > 0 && timeout > 0)
        {
            return;
        }

        if (enemyDefeat == totalEnemy && PlayerStats.Lives > 0)
        {
            manager.WinGame();
        }

        if (countdown <= 0f && waveIndex < waves.Length)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWave;

            timeout = waves[waveIndex].timeout;

            return;
        }
        countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];

        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy, wave.spawnpoint, wave.waypoint);
            yield return new WaitForSeconds(wave.rate);
        }

        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy, Transform spawnpoint, Waypoint waypoint)
    {
        GameObject Enemy = Instantiate(enemy, spawnpoint.position, Quaternion.identity);
        Enemy.GetComponent<EnemyMovement>().waypoint = waypoint;
    }
}
