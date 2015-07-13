using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class SaveLoadManagerNew : MonoBehaviour {

    public Ma2MaComManager ma2MaComManager;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Save (BehaviorObject b) {
        XmlSerializer serializer = new XmlSerializer (typeof (BehaviorObject));
        FileStream stream = new FileStream (Application.persistentDataPath + "/testSave.xml", FileMode.Create);
        serializer.Serialize (stream, b);
        stream.Close ();
    }

    public void Load () {
        XmlSerializer serializer = new XmlSerializer (typeof (BehaviorObject));
        FileStream stream = new FileStream (Application.persistentDataPath + "/testSave.xml", FileMode.Open);
        BehaviorObject container = serializer.Deserialize (stream) as BehaviorObject;

        Debug.Log (container.listOfRobotStateObjects[0].listOfModuleStateObjects[0].listOfJointCommands[0].commandType);
        stream.Close ();
    }
}
