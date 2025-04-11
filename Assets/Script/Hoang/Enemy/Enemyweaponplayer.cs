using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

public class Enemyweaponplayer : MonoBehaviour
{
 
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
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
