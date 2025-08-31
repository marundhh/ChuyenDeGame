using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class EnemySpawner : NetworkBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public int maxEnemyCount = 5;
    public float respawnDelay = 3f;

    private List<GameObject> enemies;

    private bool playerSpawned = false;
    private bool isSpawning = false;

    void Awake()
    {
        enemies = new List<GameObject>();
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            StartCoroutine(CheckForPlayerAndStartSpawning());
        }
    }

    IEnumerator CheckForPlayerAndStartSpawning()
    {
        while (!playerSpawned)
        {
            var players = FindObjectsOfType<PlayerProperties>();
            foreach (var player in players)
            {
                if (player != null && player.health > 0)
                {
                    playerSpawned = true;

                    // Spawn lần đầu
                    for (int i = 0; i < maxEnemyCount; i++)
                    {
                        SpawnEnemy();
                    }
                    break;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        if (!Object || !Object.HasStateAuthority || !playerSpawned || enemies == null)
            return;

        // Loại bỏ enemy đã bị tiêu diệt
        enemies.RemoveAll(e => e == null);

        // Nếu thiếu enemy thì spawn thêm
        if (enemies.Count < maxEnemyCount && !isSpawning)
        {
            StartCoroutine(RespawnEnemyAfterDelay(respawnDelay));
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null || Runner == null)
        {
            Debug.LogWarning("EnemyPrefab hoặc Runner chưa được thiết lập.");
            return;
        }

        NetworkObject enemyNetworkObj = enemyPrefab.GetComponent<NetworkObject>();
        if (enemyNetworkObj == null)
        {
            Debug.LogError("Enemy prefab thiếu NetworkObject component!");
            return;
        }

        NetworkObject newEnemy = Runner.Spawn(
            enemyNetworkObj,
            transform.position,
            Quaternion.identity,
            inputAuthority: null
        );

        if (newEnemy != null)
        {
            enemies.Add(newEnemy.gameObject);
        }
    }

    IEnumerator RespawnEnemyAfterDelay(float delay)
    {
        isSpawning = true;
        yield return new WaitForSeconds(delay);
        SpawnEnemy();
        isSpawning = false;
    }
}
