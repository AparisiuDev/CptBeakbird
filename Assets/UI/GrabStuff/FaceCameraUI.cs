using UnityEngine;

public class FaceCameraUI : MonoBehaviour
{
    public Camera mainCamera;

    void Start()
    {
    }

    void LateUpdate()
    {
        // Make the UI face the camera
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                         mainCamera.transform.rotation * Vector3.up);
    }
}
