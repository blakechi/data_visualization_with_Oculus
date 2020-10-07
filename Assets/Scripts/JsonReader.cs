//using System.IO; 
using UnityEngine;
using System.Collections;

public class JsonReader : MonoBehaviour
{
    JsonData jsonData;
    // public struct JsonData
    // {
    //     public Nodes nodesInJson;
    //     public Links linksInJson;
    // }

    public JsonData ReadFromJson(TextAsset nodesJson, TextAsset linksJson)
    {

        jsonData = new JsonData();

        jsonData.nodesInJson = JsonUtility.FromJson<Nodes>(nodesJson.text);
        jsonData.linksInJson = JsonUtility.FromJson<Links>(linksJson.text);

        return jsonData;
    }
}
