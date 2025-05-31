using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject miniBossPrefab;

    private float spawnInterval = 3f;
    public float spawnDistanceFromCamera = 2f;
    private Camera mainCamera;
    [Header("MiniBoss Settings")]
    public int miniBossWaveInterval = 5; // setiap kelipatan 5
    public int miniBossCount = 1;
    private void Start()
    {
        mainCamera = Camera.main;
        // InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    // public void SpawnEnemy()
    // {
    //     Vector2 spawnPos = GetRandomPositionOutsideCamera();
    //     Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    // }
    public IEnumerator SpawnEnemy(int count, float interval, int waveNumber)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPos = GetRandomPositionOutsideCamera();
            if ((waveNumber + 1) % miniBossWaveInterval == 0 && i < miniBossCount )
            {
            Debug.Log("spawn" + i);
                GameObject miniBoss = Instantiate(miniBossPrefab, spawnPos, Quaternion.identity);
                miniBoss.GetComponent<Enemy>().ScaleStats(waveNumber);
                
            }
            else
            {
                GameObject enemyObj = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ScaleStats(waveNumber); // ← Terapkan scaling
                }
            }
                yield return new WaitForSeconds(interval);
            }
        }
    private Vector2 GetRandomPositionOutsideCamera()
    {
        Vector2 camPos = mainCamera.transform.position;
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Pilih sisi acak: 0 = kiri, 1 = kanan, 2 = atas, 3 = bawah
        int side = Random.Range(0, 4);
        Vector2 spawnPos = Vector2.zero;

        switch (side)
        {
            case 0: // kiri
                spawnPos = new Vector2(camPos.x - camWidth / 2 - spawnDistanceFromCamera, Random.Range(camPos.y - camHeight / 2, camPos.y + camHeight / 2));
                break;
            case 1: // kanan
                spawnPos = new Vector2(camPos.x + camWidth / 2 + spawnDistanceFromCamera, Random.Range(camPos.y - camHeight / 2, camPos.y + camHeight / 2));
                break;
            case 2: // atas
                spawnPos = new Vector2(Random.Range(camPos.x - camWidth / 2, camPos.x + camWidth / 2), camPos.y + camHeight / 2 + spawnDistanceFromCamera);
                break;
            case 3: // bawah
                spawnPos = new Vector2(Random.Range(camPos.x - camWidth / 2, camPos.x + camWidth / 2), camPos.y - camHeight / 2 - spawnDistanceFromCamera);
                break;
        }

        return spawnPos;
    }
}
