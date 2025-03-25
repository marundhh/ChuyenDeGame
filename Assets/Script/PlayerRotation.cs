using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15f;
    public float aimDuration = 0.3f;

    public Rig aimLayer;
    private Camera mainCamera;
    private RaycastWeapon weapon;
    private bool isAiming = false;

    void Start()
    {
        mainCamera = Camera.main;
        weapon = GetComponentInChildren<RaycastWeapon>();
    }

    void FixedUpdate()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        // Nhấn E để bật/tắt chế độ ngắm
        if (Input.GetKeyDown(KeyCode.E))
        {
            isAiming = !isAiming;
        }

        // Điều chỉnh hiệu ứng ngắm
        aimLayer.weight = Mathf.Clamp01(aimLayer.weight + (isAiming ? Time.deltaTime / aimDuration : -Time.deltaTime / aimDuration));

        // Kiểm tra nếu có vũ khí mới bắn
        if (weapon != null)
        {
            if (isAiming && Input.GetMouseButton(0)) // Giữ chuột trái để bắn
            {
                weapon.StartFiring();
            }
            if (Input.GetMouseButtonUp(0)) // Nhả chuột trái để ngừng bắn
            {
                weapon.StopFiring();
            }
        }
    }
}
