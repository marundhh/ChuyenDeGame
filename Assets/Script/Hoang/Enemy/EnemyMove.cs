using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public GameObject thisEnemy;
    private NavMeshAgent nav;
    private Animator anim;
    private AnimatorStateInfo enemyInfo;
    private float x;
    private float z;
    private float velocitySpeed;
    private float distance;
    private bool isAttacking = false;

    public float attackRange = 2.0f;
    public float runRange = 12.0f;

    private GameObject[] players;
    private GameObject targetPlayer;

    private WaitForSeconds lookTime = new WaitForSeconds(2);

    private PlayerProperties enemyProperties;
    private bool isDeadHandled = false;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyProperties = GetComponent<PlayerProperties>();

        nav.avoidancePriority = Random.Range(5, 75);
    }

    void Update()
    {
        // Nếu enemy đã chết thì bỏ qua toàn bộ xử lý còn lại
        if (enemyProperties != null && enemyProperties.health <= 0)
        {
            if (!isDeadHandled)
            {
                HandleDeath();
            }
            return;
        }

        // Tìm danh sách tất cả Player (nếu chưa có hoặc số lượng thay đổi)
        players = GameObject.FindGameObjectsWithTag("Player");

        // Tìm player gần nhất
        targetPlayer = GetClosestPlayer();

        if (targetPlayer == null)
            return;

        x = nav.velocity.x;
        z = nav.velocity.z;
        velocitySpeed = x + z;

        anim.SetBool("running", velocitySpeed != 0);
        isAttacking = velocitySpeed == 0 ? false : isAttacking;

        enemyInfo = anim.GetCurrentAnimatorStateInfo(0);
        distance = Vector3.Distance(transform.position, targetPlayer.transform.position);

        if (distance < attackRange || distance > runRange)
        {
            nav.isStopped = true;

            if (distance < attackRange && enemyInfo.IsTag("nonattack"))
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    anim.SetTrigger("attack");
                    StartCoroutine(LookAtTarget());
                }
            }

            if (distance < attackRange && enemyInfo.IsTag("attack"))
            {
                isAttacking = false;
            }
        }
        else
        {
            nav.isStopped = false;
            nav.destination = targetPlayer.transform.position;
        }
    }

    GameObject GetClosestPlayer()
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = player;
            }
        }

        return closest;
    }

    IEnumerator LookAtTarget()
    {
        yield return lookTime;
        if (targetPlayer != null)
        {
            transform.LookAt(targetPlayer.transform);
        }
    }

    void HandleDeath()
    {
        isDeadHandled = true;
        nav.isStopped = true;
        anim.SetBool("running", false);
        anim.SetTrigger("Die");

        StartCoroutine(DestroyAfterDelay(10f));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
