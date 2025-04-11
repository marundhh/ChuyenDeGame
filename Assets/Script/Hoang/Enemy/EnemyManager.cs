using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [Header("Danh sách Enemy cần ẩn/hiện")]
    public List<GameObject> enemiesToActivate;

    private bool playerSpawned = false;

    void Start()
    {
        // Ẩn toàn bộ enemy lúc bắt đầu
        foreach (GameObject enemy in enemiesToActivate)
        {
            if (enemy != null)
                enemy.SetActive(false);
        }

        // Bắt đầu kiểm tra liên tục xem player đã spawn chưa
        StartCoroutine(CheckForPlayer());
    }

    IEnumerator CheckForPlayer()
    {
        while (!playerSpawned)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                playerSpawned = true;

                // Hiện tất cả enemy
                foreach (GameObject enemy in enemiesToActivate)
                {
                    if (enemy != null)
                        enemy.SetActive(true);
                }

                Debug.Log("Player đã spawn. Kích hoạt enemy.");
                break;
            }

            yield return new WaitForSeconds(0.5f); // Kiểm tra mỗi 0.5s
        }
    }
}
