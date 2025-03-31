using UnityEngine;
using UnityEngine.Animations.Rigging;
using Fusion;

public class CharacterAiming : NetworkBehaviour
{
    public float turnSpeed = 15f;
    public float aimDuration = 0.3f;

    public Rig aimLayer;
    private Camera mainCamera;
    private RaycastWeapon weapon;

    [Networked] private bool isAiming { get; set; } // Đồng bộ trạng thái ngắm

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            mainCamera = Camera.main; // Chỉ tìm camera của người chơi local
            weapon = GetComponentInChildren<RaycastWeapon>();

            if (weapon == null)
            {
                Debug.LogError("Weapon không được tìm thấy!");
            }
        }
    }

    void FixedUpdate()
    {
        if (!Object.HasInputAuthority) return; // Chỉ cập nhật góc quay trên máy của người chơi này

        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (!Object.HasInputAuthority) return; // Chỉ xử lý input trên máy của người chơi này

        // Nhấn E để bật/tắt chế độ ngắm
        if (Input.GetKeyDown(KeyCode.E))
        {
            isAiming = !isAiming;
            RPC_SetAiming(isAiming); // Gửi trạng thái ngắm tới các client khác
        }

        // Điều chỉnh hiệu ứng ngắm
        aimLayer.weight = Mathf.Clamp01(aimLayer.weight + (isAiming ? Time.deltaTime / aimDuration : -Time.deltaTime / aimDuration));

        // Kiểm tra nếu có vũ khí mới bắn
        if (weapon != null)
        {
            if (isAiming && Input.GetMouseButton(0)) // Giữ chuột trái để bắn
            {
                Debug.Log("Bắt đầu bắn...");
                weapon.StartFiring();
            }
            if (Input.GetMouseButtonUp(0)) // Nhả chuột trái để ngừng bắn
            {
                Debug.Log("Dừng bắn...");
                weapon.StopFiring();
            }
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_SetAiming(bool aiming)
    {
        isAiming = aiming; // Cập nhật trạng thái ngắm trên tất cả client
    }
}
