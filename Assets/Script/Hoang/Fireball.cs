using UnityEngine;
using Fusion;


public class Fireball : NetworkBehaviour
{
 
    public float lifetime = 5f;
    public int damage = 20;


    private void Start()
    {
        if (Object.HasStateAuthority)
        {
            Destroy(gameObject, lifetime);
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return;


        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy hit! Gây sát thương: " + damage);
            Runner.Despawn(Object);
        }
    }
}
