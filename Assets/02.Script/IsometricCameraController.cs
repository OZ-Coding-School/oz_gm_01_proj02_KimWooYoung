using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 4;
    Vector3 forward, right;

    private void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = Quaternion.Euler(new Vector3(0,90,0)) * forward;
    }
    private void Update()
    {
            Move();
    }
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement =
            (right * h + forward * v) * moveSpeed * Time.deltaTime;

        transform.position += movement;
    }


}
