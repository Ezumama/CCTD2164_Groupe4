using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static event System.Action OnGameVictory;
    public static WaveManager instance;

    // --- Structures (inchangées) ---
    [System.Serializable] public class EnemyBatch { public GameObject enemyPrefab; public int count = 5; public float spawnInterval = 1f; }
    [System.Serializable] public class PathGroup { public int pathManagerIndex; public EnemyBatch[] enemies; }
    [System.Serializable] public class Wave { public PathGroup[] pathGroups; }

    public Wave[] waves;
    public Transform[] airSpawnPoints;
    public float timeBetweenWaves = 2f;

    [Header("Références")]
    public PathManager pathManager;
    public TextMeshProUGUI waveDisplay;

    [Header("Debug (Lecture seule)")]
    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private int aliveEnemies = 0;
    private bool isSpawning = false;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }

    void Start()
    {
        if (pathManager == null) 
        { 
            //Debug.LogError("PathManager manquant !"); 
            return; 
        }
        StartCoroutine(StartWavesRoutine());
    }

    public void RegisterEnemy(string enemyName)
    {
        aliveEnemies++;
        //Debug.Log($"[INSCRIPTION] {enemyName} ajouté. Total : {aliveEnemies}");
    }

    public void UnregisterEnemy(string enemyName)
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
        //Debug.Log($"[DÉPART] {enemyName} est mort. Restants : {aliveEnemies}");

        //if (!isSpawning && aliveEnemies <= 0)
        //{
        //    Debug.Log("<color=yellow>Compteur à zéro, prêt pour la suite.</color>");
        //}
    }

    IEnumerator StartWavesRoutine()
    {
        while (currentWaveIndex < waves.Length)
        {
            UpdateWaveDisplay(currentWaveIndex + 1);
            //Debug.Log($"<color=cyan>--- Lancement Vague {currentWaveIndex + 1} ---</color>");

            isSpawning = true;
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));

            // Sécurité : On attend un peu pour être sûr que les derniers ennemis 
            // sont bien instanciés et enregistrés
            yield return new WaitForSeconds(0.5f);
            isSpawning = false;

            // ATTENTE CRUCIALE
            // On attend que le spawn soit fini ET qu'il n'y ait plus d'ennemis
            while (isSpawning || aliveEnemies > 0)
            {
                yield return new WaitForSeconds(0.2f);
            }

            //Debug.Log($"<color=green>Vague {currentWaveIndex + 1} terminée !</color>");
            currentWaveIndex++;

            if (currentWaveIndex < waves.Length)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        //Debug.Log("Félicitations, toutes les vagues sont terminées !");
        CheckForVictory();
    }

    private void UpdateWaveDisplay(int waveNumber)
    {
        if (waveDisplay != null) waveDisplay.text = waveNumber.ToString();
    }

    IEnumerator SpawnWave(Wave wave)
    {
        List<Coroutine> pathSpawners = new List<Coroutine>();
        foreach (var group in wave.pathGroups)
        {
            pathSpawners.Add(StartCoroutine(SpawnPathGroup(group)));
        }
        foreach (var spawner in pathSpawners) yield return spawner;
    }

    IEnumerator SpawnPathGroup(PathGroup pathGroup)
    {
        int pIndex = pathGroup.pathManagerIndex;
        if (pIndex < 0 || pIndex >= pathManager.spawnPointsData.Length) yield break;

        var spData = pathManager.spawnPointsData[pIndex];

        foreach (var batch in pathGroup.enemies)
        {
            for (int i = 0; i < batch.count; i++)
            {
                if (batch.enemyPrefab == null) continue;

                GameObject obj = InstantiateEnemy(batch.enemyPrefab, pIndex, spData);
                if (obj != null) RegisterEnemy(gameObject.name);

                yield return new WaitForSeconds(batch.spawnInterval);
            }
        }
    }

    private GameObject InstantiateEnemy(GameObject prefab, int pathIndex, PathManager.SpawnPointData spData)
    {
        bool isFlying = prefab.GetComponent<EnemyAir>() != null;
        if (isFlying)
        {
            Transform spawn = (airSpawnPoints != null && pathIndex < airSpawnPoints.Length) ? airSpawnPoints[pathIndex] : airSpawnPoints[0];
            return Instantiate(prefab, spawn.position, spawn.rotation);
        }
        else
        {
            if (spData.waypoints.Length == 0) return null;
            Transform node = spData.waypoints[Random.Range(0, spData.waypoints.Length)];
            GameObject obj = Instantiate(prefab, spData.spawnPointTransform.position, spData.spawnPointTransform.rotation);
            obj.GetComponent<EnemyNav>()?.SetPath(node.Cast<Transform>().ToArray());
            return obj;
        }
    }

    private void CheckForVictory()
    {
        if (currentWaveIndex >= waves.Length && aliveEnemies <= 0)
        {
            OnGameVictory?.Invoke();
            this.enabled = false;
        }
    }
}




