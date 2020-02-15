using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

   [SerializeField] private float smoothSpeed = 0.125f;
   [SerializeField] private Vector3 Offset;
    

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + Offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
