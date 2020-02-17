using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private List<Transform> target;

   [SerializeField] private float smoothSpeed = 0.125f;
   [SerializeField] private Vector3 Offset;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + Offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
        transform.position = smoothedPosition;
        
        //Vector3 desiredPosition = target.position + Offset;
    }

    Vector3 GetCenterPoint()
    {
        if(target.Count == 1)
        {
            return target[0].position;
        }

        var bounds = new Bounds(target[0].position, Vector3.zero);

        for(int i = 0; i< target.Count; i++)
        {
            bounds.Encapsulate(target[i].position);
        }

        return bounds.center;
    }
}
