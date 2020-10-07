using UnityEngine;
using System.Collections;

public class ManipulateObject : MonoBehaviour {

    public float rotateSpeed = 0.1f;

    private float lastDistanceBetweenControllers;
    private Vector3 lastAveragePositionOfControllers;
    private float lastRotationOfControllers;
	
    void Start()
    {
        bool isControllerConnedtedL = OVRInput.IsControllerConnected(OVRInput.Controller.LTouch);
        bool isControllerConnedtedR = OVRInput.IsControllerConnected(OVRInput.Controller.RTouch);

        if (isControllerConnedtedL && isControllerConnedtedR)
        {
            Debug.Log("Both controllers connected. Ready for manipulating the object.");
        }
        else
        {
            Debug.Log("One or both of controllers dicconnected. Not ready for manipulating the object.");
        }

        Vector3 controllerPositionL = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        Vector3 controllerPositionR = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

        lastAveragePositionOfControllers = AverageControllerPosition(controllerPositionL, controllerPositionR);
        lastDistanceBetweenControllers = DistanceBetweenControllers(controllerPositionL, controllerPositionR);
        lastRotationOfControllers = RotationInDegreesOfControllers(controllerPositionL, controllerPositionR);
    }

	void Update() 
    {
        OVRInput.Update();

        Vector3 controllerPositionL = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        Vector3 controllerPositionR = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

        UpdateObjectRotation(controllerPositionL, controllerPositionR);
        UpdateObjectScale(controllerPositionL, controllerPositionR);

        lastAveragePositionOfControllers = AverageControllerPosition(controllerPositionL, controllerPositionR);
        lastDistanceBetweenControllers = DistanceBetweenControllers(controllerPositionL, controllerPositionR);
        lastRotationOfControllers = RotationInDegreesOfControllers(controllerPositionL, controllerPositionR);
	}


    void UpdateObjectRotation(Vector3 controllerPositionL, Vector3 controllerPositionR)
    {
        if (BothIndexTriggersPulled())
        {
            float amountToRotate = (RotationInDegreesOfControllers(controllerPositionL, controllerPositionR) - lastRotationOfControllers);
            transform.RotateAround(Vector3.up, amountToRotate);
            // transform.RotateAround(Vector3.right, yDir);
        }
    }


    void UpdateObjectScale(Vector3 controllerPositionL, Vector3 controllerPositionR)
    {
        if (BothHandTriggersPulled())
        {
            float scaleFactor = 1 + (DistanceBetweenControllers(controllerPositionL, controllerPositionR) - lastDistanceBetweenControllers);
            float objectScaleY = transform.localScale.y;
            ScaleObjectAroundPoint(AverageControllerPosition(controllerPositionL, controllerPositionR), scaleFactor);
            transform.localScale.Set(transform.localScale.x, objectScaleY, transform.localScale.z);
        }
    }


    private float DistanceBetweenControllers(Vector3 controllerPositionL, Vector3 controllerPositionR)
    {
        return Vector3.Distance(controllerPositionL, controllerPositionR);
    }


    private Vector3 AverageControllerPosition(Vector3 controllerPositionL, Vector3 controllerPositionR)
    {
        return (controllerPositionL + controllerPositionR) / 2;
    }
    

    private float RotationInDegreesOfControllers(Vector3 controllerPositionL, Vector3 controllerPositionR)
    {
        return Mathf.Atan2(controllerPositionL.z - controllerPositionR.z, controllerPositionL.x - controllerPositionR.x) * Mathf.Rad2Deg * -rotateSpeed;
    }


    void ScaleObjectAroundPoint(Vector3 pivotPoint, float amountToScaleBy)
    {
        float minScale = 0.0001f;
        Vector3 a = transform.position;
        Vector3 endScale = transform.localScale * amountToScaleBy;

        if (endScale.x < minScale)
        {
            endScale = new Vector3(minScale, minScale, minScale);
        }

        Vector3 c = a - pivotPoint;
        Vector3 finalPosition = (c * amountToScaleBy) + pivotPoint;

        finalPosition.y = transform.position.y;

        transform.localScale = endScale;
        transform.position = finalPosition;
    }


    private bool BothIndexTriggersPulled()
    {
        float controllerIndexTriggerL = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        float controllerIndexTriggerR = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);

        return (controllerIndexTriggerL > 0) && (controllerIndexTriggerR > 0);
    }


    private bool BothHandTriggersPulled()
    {
        float controllerHandTriggerL = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
        float controllerHandTriggerR = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger);

        return controllerHandTriggerL > 0 && controllerHandTriggerR > 0;
    }

}
