using UnityEngine;

public class StartSceneCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target; // Điểm trung tâm map (gắn vào trung tâm map của bạn)
    public float rotationSpeed = 1f; // Tốc độ xoay mặc định
    public float distanceFromTarget = 10f; // Khoảng cách từ camera đến target
    public float heightOffset = 5f; // Độ cao camera so với target

    [Header("Player Control")]
    public float mouseSensitivity = 2f; // Tốc độ điều khiển bằng chuột
    private float currentRotationAngle = 0f; // Góc xoay hiện tại

    void Update()
    {
        // Tự động xoay camera quanh target
        currentRotationAngle += rotationSpeed * Time.deltaTime;

        // Điều khiển bằng chuột (giữ chuột phải để xoay)
        if (Input.GetMouseButton(1)) // Chuột phải
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            currentRotationAngle += mouseX; // Thay đổi góc xoay dựa trên chuột
        }

        // Tính toán vị trí camera
        Vector3 orbitDirection = new Vector3(
            Mathf.Sin(currentRotationAngle),
            0,
            Mathf.Cos(currentRotationAngle)
        ).normalized;

        Vector3 cameraPosition = target.position + (orbitDirection * distanceFromTarget);
        cameraPosition.y += heightOffset; // Thêm độ cao

        // Áp dụng vị trí và hướng nhìn
        transform.position = cameraPosition;
        transform.LookAt(target.position); // Camera luôn hướng về trung tâm
    }
}