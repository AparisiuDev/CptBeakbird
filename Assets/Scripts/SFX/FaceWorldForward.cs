using UnityEngine;

public class FaceWorldForward : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }
}
