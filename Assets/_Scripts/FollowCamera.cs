using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target; //플레이어 타겟
    Transform cam;

    [Range(2.0f, 20.0f)]
    [SerializeField] float distance = 10.0f;

    [Range(0.0f, 10.0f)]
    [SerializeField] float height = 2.0f;

    [SerializeField] float damping = 10.0f;

    [SerializeField] float targetOffset = 2.0f;

    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        cam = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector3 pos = target.position 
                       + (-target.forward * distance)
                       + (Vector3.up * height);

        cam.position = Vector3.SmoothDamp(cam.position, pos, ref velocity, damping);

        cam.LookAt(target.position + (target.up * targetOffset));
    }
}

