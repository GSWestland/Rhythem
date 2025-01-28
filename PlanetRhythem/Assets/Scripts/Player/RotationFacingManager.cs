using UnityEngine;

public class RotationFacingManager : MonoBehaviour
{
    public Vector3 targetRotation;
    void Update()
    {
        transform.eulerAngles = targetRotation;
    }
}
