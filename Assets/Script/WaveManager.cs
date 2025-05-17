using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

    public GameObject WavePanel;       
    
    private TMP_Text waveText;
     
    private void Start()
    {
        if (WavePanel != null)
        {
            waveText = WavePanel.GetComponentInChildren<TMP_Text>();
            WavePanel.SetActive(false); // sembunyikan panel saat awal
        }

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

        // Tampilkan panel dan teks wave
        if (WavePanel != null && waveText != null)
        {
            WavePanel.SetActive(true);
            waveText.text = "Wave " + (currentWave + 1);
        }

        Debug.Log("Wave : " + (currentWave + 1));

        yield return new WaitForSeconds(2f); // durasi tampilnya panel wave

        if (WavePanel != null)
        {
            WavePanel.SetActive(false); // sembunyikan panel lagi
        }

        // Mulai spawn musuh
        yield return StartCoroutine(spawner.SpawnEnemy(waves[currentWave].enemyCount, waves[currentWave].spawnInterval));

        isSpawning = false;

        // Jika masih ada wave berikutnya, tunggu jeda antar wave
        if (currentWave < waves.Length - 1)
        {
            waitingNextWave = true;
            yield return new WaitForSeconds(timeBetweenWaves);
            currentWave++;
            waitingNextWave = false;
        }
    }
}
