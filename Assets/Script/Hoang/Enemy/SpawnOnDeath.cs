using UnityEngine;
using Fusion;

public class SpawnOnDeath : NetworkBehaviour
{
    public PlayerProperties playerProps;
    public GameObject spawnObjectPrefab;
    public float delayAfterDeath = 1f;

    private bool hasSpawned = false;

    public override void FixedUpdateNetwork()
    {
        if (!hasSpawned && playerProps != null && playerProps.isDead)
        {
            hasSpawned = true;
            StartCoroutine(SpawnAfterDisappear());
        }
    }

    private System.Collections.IEnumerator SpawnAfterDisappear()
    {
        // Đợi một chút cho player biến mất
        yield return new WaitForSeconds(delayAfterDeath);

        if (spawnObjectPrefab != null)
        {
            Instantiate(spawnObjectPrefab, transform.position, Quaternion.identity);
        }

        // Hoặc nếu dùng Fusion:
        // Runner.Spawn(spawnObjectPrefab, transform.position, Quaternion.identity);
    }
}
