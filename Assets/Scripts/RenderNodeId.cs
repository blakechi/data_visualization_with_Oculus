using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderNodeId : MonoBehaviour
{
    public GameObject nodeId;
    private GameObject mainCamera;
    private string nodeIdString; 
    private string emptyString = "";

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        
        nodeIdString = nodeId.GetComponent<Text>().text;
        nodeId.GetComponent<Text>().text = emptyString;
    }

    public void onPointerOver()
    {
        nodeId.transform.LookAt(mainCamera.transform, new Vector3(0, -1, 0));
        nodeId.transform.Rotate(180f, 0f, 0f);
        nodeId.GetComponent<Text>().text = nodeIdString;
    }

    public void onPointerExit()
    {
        nodeId.GetComponent<Text>().text = emptyString;
    }
}