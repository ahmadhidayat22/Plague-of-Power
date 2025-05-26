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
    public WeaponUpgradeUI weaponUpgradeUI;
    public GameObject WavePanel;       
    public GameObject GuidesPanel;       
    
    private TMP_Text waveText;
    private bool isShowGudiesPanel;
     
    private void Start()
    {
        isShowGudiesPanel = false;
        if (WavePanel != null)
        {
            waveText = WavePanel.GetComponentInChildren<TMP_Text>();
            WavePanel.SetActive(false);
            GuidesPanel.SetActive(false);
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
        if (!isShowGudiesPanel)
        {

            GuidesPanel.SetActive(true);
            yield return new WaitForSeconds(5f);
            GuidesPanel.GetComponent<Animator>().SetTrigger("Close");
            yield return new WaitForSeconds(2f);
            GuidesPanel.SetActive(false);
            isShowGudiesPanel = true;
        }

        // Tampilkan panel dan teks wave
        if (WavePanel != null && waveText != null)
        {
            
            WavePanel.SetActive(true);
            waveText.text = "Wave " + (currentWave + 1);
        }
        
        
       
        // Debug.Log("Wave : " + (currentWave + 1));

        yield return new WaitForSeconds(2f); // durasi tampilnya panel wave
        if (currentWave % 2 == 0 && currentWave != 0)
        {
            weaponUpgradeUI.OpenUpgradeUI();

        }
        if (WavePanel != null)
        {
            WavePanel.SetActive(false);
        }

        // Mulai spawn musuh
        yield return StartCoroutine(spawner.SpawnEnemy(waves[currentWave].enemyCount, waves[currentWave].spawnInterval, currentWave));

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
