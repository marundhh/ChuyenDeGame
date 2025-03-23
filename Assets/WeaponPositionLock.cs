using UnityEngine;

public class WeaponPositionLock : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        // Lưu vị trí ban đầu của vũ khí
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Giữ nguyên vị trí và hướng của vũ khí
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
