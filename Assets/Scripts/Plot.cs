using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public float linkWidth = 0.025f;
    public float coordinationScale = 5;

    public GameObject NodePrefab;
    public GameObject LinkPrefab;
    public GameObject LaserPointer;

    public void PlotNodes(JsonData data, GameObject graphHolder)
    {
        foreach (Node node in data.nodesInJson.nodes)
        {
            float[] p = extractPosition(node);
            
            GameObject nodeObject = Instantiate(NodePrefab, new Vector3(p[0], p[1], p[2])*coordinationScale, Quaternion.identity);
        
            nodeObject.transform.SetParent(graphHolder.transform);
            nodeObject.transform.name = node.id;

            var nodeCanvas = nodeObject.transform.Find("Canvas");
            nodeCanvas.GetComponent<OVRRaycaster>().pointer = LaserPointer;

            var nodeIdObj = nodeObject.transform.Find("Canvas/NodeId");
            nodeIdObj.GetComponent<Text>().text = node.id;

            var nodeIdRenderer = nodeIdObj.GetComponent<Renderer>();
            nodeIdRenderer.enabled = false;

            if (ColorUtility.TryParseHtmlString(node.color, out Color nodeColor))
            {
                var nodeRenderer = nodeObject.GetComponent<Renderer>();
                nodeRenderer.material.SetColor("_Color", nodeColor);
            }
        }
    }
    
    public void PlotLinks(JsonData data, GameObject graphHolder)
    {
        Node[] nodes = data.nodesInJson.nodes;
        Link[] links = data.linksInJson.links;

        foreach(Link link in links)
        {
            float[] startP = extractPosition(nodes[link.source]);
            float[] endP = extractPosition(nodes[link.target]);
            
            Vector3 start = new Vector3(startP[0], startP[1], startP[2])*coordinationScale;
            Vector3 end = new Vector3(endP[0], endP[1], endP[2])*coordinationScale;

            var offset = end - start;
            var weightedLinkWidth = linkWidth*link.value / 4.0f;
            var scale = new Vector3(weightedLinkWidth, offset.magnitude / 2.0f, weightedLinkWidth);
            var position = start + (offset / 2.0f);
    
            var linkObject = Instantiate(LinkPrefab, position, Quaternion.identity);
            linkObject.transform.up = offset;
            linkObject.transform.localScale = scale;
            linkObject.transform.SetParent(graphHolder.transform);
        }
        
    }

    private float[] extractPosition(Node node)
    {
        float[] position = {node.x, node.y, node.z};

        return position;
    }
}
