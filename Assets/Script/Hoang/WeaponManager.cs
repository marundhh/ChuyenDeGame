using UnityEngine;
using Fusion;

public class WeaponManager : NetworkBehaviour
{
    public float lifetime = 3f;
    private float timer;
    void Update()
    {
        // Tự hủy sau một thời gian sống
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            if (Object != null && Object.HasStateAuthority)
            {
                Runner.Despawn(Object); // Fusion-style despawn
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("vao OntriggerEnter");
        if (other.CompareTag("Enemy"))
        {
            PlayerProperties opponentCombat = other.GetComponent<PlayerProperties>();
            if (opponentCombat != null)
            {
                opponentCombat.RpcTakeDamage(10);
                Debug.Log("Sword hit opponent! Damage dealt: " + 10);
            }
        }
    }
}
