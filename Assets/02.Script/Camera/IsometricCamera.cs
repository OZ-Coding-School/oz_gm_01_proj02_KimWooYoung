using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, 0f);
    [SerializeField] public Transform target;
    [SerializeField] private float fllowSpeed = 10f;

    [SerializeField] private Vector2 minLimit;
    [SerializeField] private Vector2 maxLimit;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;

       desiredPos.x = Mathf.Clamp(desiredPos.x,minLimit.x, maxLimit.x);
       desiredPos.z = Mathf.Clamp(desiredPos.z,minLimit.y, maxLimit.y);

        transform.position = Vector3.Lerp(transform.position,
            desiredPos, fllowSpeed * Time.deltaTime);
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
