using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weaponmanagesword : MonoBehaviour
{
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
