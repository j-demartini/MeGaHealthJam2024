using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, -5f);
    [SerializeField] private Vector3 rotationOffset = new Vector3(0, 1, 0);
    [SerializeField] private float smoothSpeed = 0.125f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 desiredPosition = Player.Instance.transform.position + Player.Instance.transform.forward * offset.z + Vector3.up * offset.y + Player.Instance.transform.right * offset.x;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        transform.LookAt(Player.Instance.transform.position + rotationOffset);
    }
}
