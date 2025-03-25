using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RaycastWeapon : NetworkBehaviour
{
    public bool isFiring = false;
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;

    Ray ray;
    RaycastHit hitInfo;

    public override void Spawned()
    {
        // Kiểm tra xem player này có quyền bắn hay không
        if (!Object.HasInputAuthority) enabled = false;
    }

    public void StartFiring()
    {
        if (!Object.HasInputAuthority) return; // Chỉ người chơi local mới có thể bắn

        isFiring = true;
        RPC_FireWeapon(raycastOrigin.position, raycastDestination.position);
    }

    public void StopFiring()
    {
        if (!Object.HasInputAuthority) return;
        isFiring = false;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_FireWeapon(Vector3 origin, Vector3 destination)
    {
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
        }

        ray.origin = origin;
        ray.direction = destination - origin;

        var tracer = Instantiate(tracerEffect, origin, Quaternion.identity);
        tracer.AddPosition(origin);

        if (Physics.Raycast(ray, out hitInfo))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            tracer.transform.position = hitInfo.point;
        }
    }
}
