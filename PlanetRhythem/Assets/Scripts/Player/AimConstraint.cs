using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class AimConstraint : MonoBehaviour
{
    public GameObject targetObject;
    [Title("OR")]
    public Quaternion targetRotation;

    private bool useGO = false;
    [HideInInspector] public bool active;

    void Start()
    {
        useGO = targetObject != null;
    }

    void Update()
    {
        if (!active)
        {
            return;
        }

        if (useGO)
        {
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetObject.transform.rotation, 360f);
        }
        transform.rotation = targetRotation;
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }
}
