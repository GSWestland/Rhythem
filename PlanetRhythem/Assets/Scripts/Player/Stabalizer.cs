using UnityEngine;

public class Stabalizer : MonoBehaviour
{
    public Vector3 targetRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.eulerAngles = targetRotation;
    }
}
