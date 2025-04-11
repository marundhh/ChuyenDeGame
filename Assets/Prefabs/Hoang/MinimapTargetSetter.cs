using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MinimapTargetSetter : NetworkBehaviour
{
    public override void Spawned()
    {
        if (Object.HasInputAuthority) // ch? ch?y cho player local
        {
            GameObject minimapCam = GameObject.Find("MinimapCamera");
            if (minimapCam != null)
            {
                MinimapFollow follow = minimapCam.GetComponent<MinimapFollow>();
                follow.target = this.transform;
            }
        }
    }
}