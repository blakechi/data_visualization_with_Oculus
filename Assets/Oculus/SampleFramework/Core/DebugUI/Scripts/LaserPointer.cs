/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided �AS IS� WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class LaserPointer : OVRCursor
{
    public enum LaserBeamBehavior
    {
        On,        // laser beam always on
        Off,        // laser beam always off
        OnWhenHitTarget,  // laser beam only activates when hit valid target
    }

    public GameObject cursorVisual;
    public float maxLength = 50.0f;

    [SerializeField]
    private LaserBeamBehavior _laserBeamBehavior;

    public LaserBeamBehavior laserBeamBehavior
    {
        set {
            _laserBeamBehavior = value;
            if(laserBeamBehavior == LaserBeamBehavior.Off || laserBeamBehavior == LaserBeamBehavior.OnWhenHitTarget)
            {
                lineRenderer.enabled = false;
            }
            else
            {
                lineRenderer.enabled = true;
            }
        }
        get
        {
            return _laserBeamBehavior;
        }
    }
    private Vector3 _startPoint;
    private Vector3 _forward;
    private Vector3 _endPoint;
    private bool _hitTarget;
    private LineRenderer lineRenderer;

    // self-defined
    public Color normalColor;
    public Color selectedColor;
    private Color fadedColor = new Color(1, 1, 1, 0);
    private GameObject savedObj;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        if (cursorVisual) cursorVisual.SetActive(false);

        // self-defined
        savedObj = new GameObject();
    }

    public override void SetCursorStartDest(Vector3 start, Vector3 dest, Vector3 normal)
    {
        _startPoint = start;
        _endPoint = dest;
        _hitTarget = true;
    }

    public override void SetCursorRay(Transform t)
    {
        _startPoint = t.position;
        _forward = t.forward;
        _hitTarget = false;
    }

    // original: private void LateUpdate()
    private void Update()
    {
        OVRInput.Update();


        if (BothIndexTriggersPulled() || BothHandTriggersPulled())
        {
            lineRenderer.enabled = false;
            try
            {
                savedObj.transform.SendMessage("onPointerExit");
                savedObj = null;
            }
            catch(Exception e)
            {
                //Debug.LogException(e, this);
            }
        }
        else
        {
            lineRenderer.enabled = true;
            HandleLineRenderer();
        } 
        // if (_hitTarget)
        // {
        //     lineRenderer.SetPosition(1, _endPoint);
        //     UpdateLaserBeam(_startPoint, _endPoint);
        //     if (cursorVisual)
        //     {
        //         cursorVisual.transform.position = _endPoint;
        //         cursorVisual.SetActive(true);
        //     }
        // }
        // else
        // {
        //     UpdateLaserBeam(_startPoint, _startPoint + maxLength * _forward);
        //     lineRenderer.SetPosition(1, _startPoint + maxLength * _forward);
        //     if (cursorVisual) cursorVisual.SetActive(false);
        // }
    }

    // make laser beam a behavior with a prop that enables or disables
    private void UpdateLaserBeam(Vector3 start, Vector3 end)
    {
        if(laserBeamBehavior == LaserBeamBehavior.Off)
        {
            return;
        }
        else if(laserBeamBehavior == LaserBeamBehavior.On)
        {
            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, end);
        }
        else if(laserBeamBehavior == LaserBeamBehavior.OnWhenHitTarget)
        {
            if(_hitTarget)
            {
                if (!lineRenderer.enabled)
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, start);
                    lineRenderer.SetPosition(1, end);
                }
            }
            else
            {
                if(lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                }
            }
        }
    }

    void OnDisable()
    {
        if(cursorVisual) cursorVisual.SetActive(false);
    }

    // self-defined
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


    private void HandleLineRenderer()
    {
        lineRenderer.SetColors(normalColor, fadedColor);
        lineRenderer.SetPosition(0, _startPoint);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                lineRenderer.SetColors(selectedColor, fadedColor);
                lineRenderer.SetPosition(1, hit.point);

                if (hit.transform.gameObject.tag == "Node")
                {
                    try
                    {
                        savedObj.transform.SendMessage("onPointerExit");
                    }
                    catch(Exception e)
                    {
                        //Debug.LogException(e, this);
                    }
                    
                    savedObj = hit.transform.gameObject;
                    hit.transform.SendMessage("onPointerOver");
                }
                else
                {
                    try
                    {
                        savedObj.transform.SendMessage("onPointerExit");
                        savedObj = null;
                    }
                    catch(Exception e)
                    {
                        //Debug.LogException(e, this);
                    }
                } 
            }
        }
        else
        {
            lineRenderer.SetPosition(1,  transform.position + (transform.forward*5000));
            
            try
            {
                savedObj.transform.SendMessage("onPointerExit");
                savedObj = null;
            }
            catch(Exception e)
            {
                //Debug.LogException(e, this);
            }

            savedObj = null;
        }
    }
}
