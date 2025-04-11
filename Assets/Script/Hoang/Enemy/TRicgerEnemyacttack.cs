using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRicgerEnemyacttack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject weapon;


    public void EnableMovement(bool enable)
    {
        /*if (enable == false)
        {
            tcp.lockMovement = true;
            tcp.lockMovement = true;
        }
        elsess
        { 
            tcp.lockMovement = false;
            tcp.lockMovement = false;
        }*/
    }
    public void OnEnableWeaponCollider(int isEnable)
    {
        if (weapon != null)
        {
            var col = weapon.GetComponent<Collider>();
            if (col != null)
            {
                if (isEnable == 1)
                    col.enabled = true;
                else
                    col.enabled = false;
            }
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
