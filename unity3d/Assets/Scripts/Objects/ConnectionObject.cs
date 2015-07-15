using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class ConnectionObject {
    
    public string moduleName1;
    public string moduleName2;
    public string nodeName1;
    public string nodeName2;
    public float distance;
    public float angle;

    public ConnectionObject () {
        distance = 0f;
        angle = 0f;
    }
}
