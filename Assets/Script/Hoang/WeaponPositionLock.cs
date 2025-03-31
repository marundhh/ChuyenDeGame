using UnityEngine;
using System.Collections.Generic;
using Fusion;

public class WeaponPositionLock : NetworkBehaviour
{
    [SerializeField] List<Transform> aimTargets; // Các Transform cần di chuyển
    [SerializeField] float aimSmoothSpeed = 20f;
    [SerializeField] LayerMask aimMask;
    [SerializeField] float maxAimDistance = 100f;

    private Camera playerCamera;

    public override void Spawned()
    {
        // Đảm bảo chỉ có camera của player local được sử dụng
        if (Object.HasInputAuthority)
        {
            playerCamera = Camera.main; // Hoặc gán camera đúng của player này
        }
    }

    void Update()
    {
        if (!Object.HasInputAuthority || playerCamera == null) return; // Chỉ chạy trên máy của người chơi này

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = playerCamera.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimMask))
        {
            foreach (Transform aimTarget in aimTargets)
            {
                aimTarget.position = Vector3.Lerp(aimTarget.position, hit.point, aimSmoothSpeed * Time.deltaTime);
            }
        }
    }
}
