using UnityEngine;

public class CameraDistanceToggle : MonoBehaviour
{
    public vThirdPersonCamera cameraController;
    public float alternativeDistance = 5f;
    private float originalDistance;
    private bool isToggled = false;

    void Start()
    {
        if (cameraController != null)
        {
            originalDistance = cameraController.defaultDistance;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && cameraController != null)
        {
            isToggled = !isToggled;
            cameraController.defaultDistance = isToggled ? alternativeDistance : originalDistance;
        }
    }
}
