using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public TextAsset nodesFile;
    public TextAsset linksFile;

    public GameObject graphHolder;
    
    private JsonData data;
    private JsonReader jsonReader;
    private Plot plot;

    void Start()
    {
        jsonReader = GameObject.FindObjectOfType<JsonReader>();
        data = jsonReader.ReadFromJson(nodesFile, linksFile);

        plot = GameObject.FindObjectOfType<Plot>();
        plot.PlotNodes(data, graphHolder); 
        plot.PlotLinks(data, graphHolder); 

    }

}
