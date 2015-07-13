using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class RobotStateObject { 

    [XmlAttribute("name")]
    public string name;

    [XmlArray("ModuleStates")]
    [XmlArrayItem("ModuleState")]
    public List<ModuleStateObject> listOfModuleStateObjects;

    public RobotStateObject () {
        name = "";
        listOfModuleStateObjects = new List<ModuleStateObject> ();
    }
}