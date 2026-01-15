using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IsometricCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, 0f);
    [SerializeField] public Transform target;
    [SerializeField] private float fllowSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position,
            desiredPos, fllowSpeed * Time.deltaTime);
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
