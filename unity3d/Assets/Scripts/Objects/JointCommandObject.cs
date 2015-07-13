using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class JointCommandObject {
    
    [XmlAttribute("name")]
    public string name;
    public CommandTypes commandType;
    public float targetValue;

    public enum CommandTypes {Position, Velocity};

    public JointCommandObject () {
        name = "";
        commandType = CommandTypes.Position;
        targetValue = 0;
    }

    public JointCommandObject (string n) {
        name = n;
        commandType = CommandTypes.Position;
        targetValue = 0;
    }

    public JointCommandObject Clone () {
        JointCommandObject newJco = new JointCommandObject ();
        newJco.name = name;
        newJco.commandType = commandType;
        newJco.targetValue = targetValue;
        return newJco;
    }
}
