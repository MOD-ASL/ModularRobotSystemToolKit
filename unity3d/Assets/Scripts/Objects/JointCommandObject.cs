using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class JointCommandObject {
    
    [XmlAttribute("name")]
    public string name;
    public CommandTypes commandType;
    public float targetValue;
    public float period;

    [XmlIgnore]
    public IEnumerator cmdCoroutine;

    public enum CommandTypes {Position, Velocity};

    public JointCommandObject () {
        name = "";
        commandType = CommandTypes.Position;
        targetValue = 0;
        period = 3.0f;
    }

    public JointCommandObject (string n) {
        name = n;
        commandType = CommandTypes.Position;
        targetValue = 0;
        period = 3.0f;
    }

    public JointCommandObject Clone (bool forConfiguration) {
        JointCommandObject newJco = new JointCommandObject ();
        newJco.name = name;
        newJco.commandType = commandType;
        newJco.targetValue = targetValue;
        if (forConfiguration) {
            newJco.period = 0.0f;
        }
        else {
            newJco.period = period;
        }
        return newJco;
    }
}
