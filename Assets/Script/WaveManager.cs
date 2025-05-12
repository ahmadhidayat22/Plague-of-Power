using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float spawnInterval;
    }

    public Wave[] waves;
    public float timeBetweenWaves = 5f;
    public EnemySpawner spawner;

    private int currentWave = 0;
    private bool isSpawning = false;
    private bool waitingNextWave = false;

    public Text waveText; // opsional: UI info wave

    private void Start()
    {
        StartCoroutine(HandleWave());
    }

    private void Update()
    {
        if (isSpawning || waitingNextWave)
            return;

        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && currentWave < waves.Length)
        {
            StartCoroutine(HandleWave());
        }
    }

    private IEnumerator HandleWave()
    {
        isSpawning = true;

        if (waveText != null)
        {
            waveText.text = "Wave " + (currentWave + 1);
        }
        Debug.Log("Wave : " + (currentWave + 1));

        yield return new WaitForSeconds(2f); // sedikit delay sebelum spawn

        // Mulai spawn
        yield return StartCoroutine(spawner.SpawnEnemy(waves[currentWave].enemyCount, waves[currentWave].spawnInterval));

        isSpawning = false;

        // Jika masih ada wave berikutnya, tunggu dulu sebelum next wave
        if (currentWave < waves.Length - 1)
        {
            waitingNextWave = true;
            yield return new WaitForSeconds(timeBetweenWaves);
            currentWave++;
            waitingNextWave = false;
        }
    }
}
