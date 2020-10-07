using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectWithThumbstick : MonoBehaviour
{
    public float rotateSpeed = 2;

    void FixedUpdate()
    {
        OVRInput.FixedUpdate();
        OnThumbstickMove();
    }

    void OnThumbstickMove()
    {
        Vector2 movement = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        float xDir = movement.x*rotateSpeed*Mathf.Deg2Rad;
        float yDir = movement.y*rotateSpeed*Mathf.Deg2Rad;

        transform.RotateAround(Vector3.up, -xDir);
        transform.RotateAround(Vector3.right, yDir);
    }
}
