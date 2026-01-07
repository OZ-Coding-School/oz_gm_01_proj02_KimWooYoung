using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IsometricCamera : MonoBehaviour
{
    [SerializeField] float offsetY = 4f;
    public Transform player;



    void LateUpdate()
    {
        if (player == null) return;

        transform.position = new Vector3(
            player.position.x,
            offsetY,
            player.position.z
        );
    }
}
