using System.Collections;
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
    public int magazineSize = 30; // Số đạn tối đa
    public float reloadTime = 2f; // Thời gian chờ hồi đạn
    public float fireRate = 0.2f; // Độ trễ giữa các phát bắn

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading = false;

    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip emptyMagSound; // Âm thanh khi hết đạn

    Ray ray;
    RaycastHit hitInfo;

    public override void Spawned()
    {
        if (!Object.HasInputAuthority) enabled = false;
        currentAmmo = magazineSize; // Đạn đầy khi bắt đầu
    }

    public void StartFiring()
    {
        if (!Object.HasInputAuthority || isReloading) return;
        if (Time.time < nextFireTime) return; // Giới hạn tốc độ bắn

        if (currentAmmo > 0)
        {
            isFiring = true;
            currentAmmo--; // Giảm đạn
            nextFireTime = Time.time + fireRate;

            RPC_FireWeapon(raycastOrigin.position, raycastDestination.position);
        }
        else
        {
            if (!isReloading)
            {
                PlayEmptyMagSound(); // Chơi âm thanh hết đạn
                StartCoroutine(ReloadAmmo()); // Hết đạn thì tự động nạp lại
            }
        }
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

        if (audioSource && fireSound)
        {
            audioSource.PlayOneShot(fireSound);
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

    private IEnumerator ReloadAmmo()
    {
        if (isReloading) yield break;

        isReloading = true;
        if (audioSource && reloadSound)
        {
            audioSource.PlayOneShot(reloadSound);
        }
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        isReloading = false;
    }

    private void PlayEmptyMagSound()
    {
        if (audioSource && emptyMagSound)
        {
            audioSource.PlayOneShot(emptyMagSound);
        }
    }

    public int GetAmmoCount()
    {
        return currentAmmo;
    }
}
