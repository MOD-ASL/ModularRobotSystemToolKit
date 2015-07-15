using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class RobotStateObject { 

    [XmlAttribute("name")]
    public string name;

    [XmlIgnoreAttribute]
    public GameObject button;

    [XmlArray("ModuleStates")]
    [XmlArrayItem("ModuleState")]
    public List<ModuleStateObject> listOfModuleStateObjects;

    [XmlArray("Connections")]
    [XmlArrayItem("Connection")]
    public List<ConnectionObject> listOfConnectionObjects;

    public RobotStateObject () {
        name = "";
        listOfModuleStateObjects = new List<ModuleStateObject> ();
        listOfConnectionObjects = new List<ConnectionObject> ();
    }
}