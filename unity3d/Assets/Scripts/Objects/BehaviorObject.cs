using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Behavior")]
public class BehaviorObject { 
    
    [XmlAttribute("name")]
    public string name;
    
    [XmlArray("RobotStates")]
    [XmlArrayItem("RobotState")]
    public List<RobotStateObject> listOfRobotStateObjects;
    
    public BehaviorObject () {
        name = "";
        listOfRobotStateObjects = new List<RobotStateObject> ();
    }
}