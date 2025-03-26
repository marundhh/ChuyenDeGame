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
    public int magazineSize = 30;
    public float reloadTime = 2f;
    public float fireRate = 0.2f;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading = false;

    public AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip emptyMagSound;

    // 🎯 Biến độ giật (Recoil)
    private Transform cameraTransform;
    public float recoilAmount = 2f;
    public float recoilSpeed = 5f;
    private Vector3 originalCameraRotation;

    Ray ray;
    RaycastHit hitInfo;

    public override void Spawned()
    {
        if (!Object.HasInputAuthority)
        {
            enabled = false;
            return;
        }

        currentAmmo = magazineSize;

        // 🔍 **Tìm Camera Player (Chạy Coroutine để đảm bảo tìm thấy)**
        StartCoroutine(FindPlayerCamera());
    }

    private IEnumerator FindPlayerCamera()
    {
        while (cameraTransform == null)
        {
            yield return new WaitForSeconds(0.1f);

            Camera foundCamera = GetComponentInChildren<Camera>();
            if (foundCamera == null)
            {
                foundCamera = Camera.main; // Nếu chưa tìm thấy, dùng Camera chính
            }

            if (foundCamera != null)
            {
                cameraTransform = foundCamera.transform;
                originalCameraRotation = cameraTransform.localEulerAngles;
            }
        }
    }

    public void StartFiring()
    {
        if (!Object.HasInputAuthority || isReloading || Time.time < nextFireTime) return;

        if (currentAmmo > 0)
        {
            isFiring = true;
            currentAmmo--;
            nextFireTime = Time.time + fireRate;

            RPC_FireWeapon(raycastOrigin.position, raycastDestination.position);

            // 🔊 Phát âm thanh bắn
            if (audioSource && fireSound) audioSource.PlayOneShot(fireSound);

            // 🔥 Thêm hiệu ứng giật (Recoil)
            ApplyRecoil();
        }
        else
        {
            // 🔊 Phát âm thanh hết đạn
            if (audioSource && emptyMagSound) audioSource.PlayOneShot(emptyMagSound);

            StartCoroutine(Reload());
        }
    }

    private void ApplyRecoil()
    {
        if (cameraTransform != null)
        {
            Vector3 recoilRotation = new Vector3(-recoilAmount, Random.Range(-recoilAmount / 2, recoilAmount / 2), 0);
            cameraTransform.localEulerAngles += recoilRotation;
            StartCoroutine(ResetRecoil());
        }
    }

    private IEnumerator ResetRecoil()
    {
        yield return new WaitForSeconds(0.1f);
        if (cameraTransform != null)
        {
            cameraTransform.localEulerAngles = Vector3.Lerp(cameraTransform.localEulerAngles, originalCameraRotation, Time.deltaTime * recoilSpeed);
        }
    }

    public void StopFiring()
    {
        if (!Object.HasInputAuthority) return;
        isFiring = false;
    }

    private IEnumerator Reload()
    {
        if (isReloading) yield break;
        isReloading = true;

        // 🔊 Phát âm thanh nạp đạn
        if (audioSource && reloadSound) audioSource.PlayOneShot(reloadSound);

        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        isReloading = false;
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

    // ✅ **Thêm phương thức GetAmmoCount() để AmmoUI lấy số đạn**
    public int GetAmmoCount()
    {
        return currentAmmo; // Trả về số đạn còn lại
    }
}
