using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using System;


public class Playerupdatesword : NetworkBehaviour
{
    public GameObject Weapon;

    public override void FixedUpdateNetwork()
    {
            if (Input.GetMouseButtonDown(0))
            {
                Weapon.GetComponent<BoxCollider>().enabled = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Weapon.GetComponent<BoxCollider>().enabled = false;
            }
        
    }
}
