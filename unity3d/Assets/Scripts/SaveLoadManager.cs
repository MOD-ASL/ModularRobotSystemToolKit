using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml; 
using System.Xml.Serialization;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Save (Transform robot, Hashtable connectionTable, string name) {
		XmlDocument xmlDoc = new XmlDocument ();
		XmlDeclaration xmldecl;
		xmldecl = xmlDoc.CreateXmlDeclaration ("1.0", "UTF-8", null);

		XmlNode rootNode = xmlDoc.CreateElement ("configuration");
		xmlDoc.AppendChild (rootNode);
		xmlDoc.InsertBefore (xmldecl, rootNode);

		XmlNode modulesNode = xmlDoc.CreateElement ("modules");
		rootNode.AppendChild (modulesNode);

		foreach (Transform m in robot) {
			XmlNode mNode = xmlDoc.CreateElement ("module");
			XmlNode mName = xmlDoc.CreateElement ("name");
			mName.InnerText = m.name;
			XmlNode position = xmlDoc.CreateElement ("position");
			position.InnerText = m.GetComponent<ModuleController> ().GetPositionInString ();
			XmlNode joints = xmlDoc.CreateElement ("joints");
			joints.InnerText = m.GetComponent<ModuleController> ().GetJointAnglesInString ();
			XmlNode path = xmlDoc.CreateElement ("path");
			path.InnerText = "SMORE.sdf";

			mNode.AppendChild (mName);
			mNode.AppendChild (position);
			mNode.AppendChild (joints);
			mNode.AppendChild (path);
			modulesNode.AppendChild (mNode);
		}

		XmlNode connectionsNode = xmlDoc.CreateElement ("connections");
		rootNode.AppendChild (connectionsNode);
		List<string> doneList = new List<string> ();
		foreach (DictionaryEntry c in connectionTable) {

			string n1 = (string) c.Key;
			if (doneList.Contains (n1)) {
				continue;
			}
			doneList.Add (n1);
			string n2 = (string) c.Value;

			XmlNode cNode = xmlDoc.CreateElement ("connection");
			XmlNode m1 = xmlDoc.CreateElement ("module1");
			m1.InnerText = n1.Split (':') [0];
			XmlNode m2 = xmlDoc.CreateElement ("module2");
			m2.InnerText = n2.Split (':') [0];
			XmlNode node1 = xmlDoc.CreateElement ("node1");
			node1.InnerText = GetNodeIndex (n1.Split (':') [1]);
			XmlNode node2 = xmlDoc.CreateElement ("node2");
			node2.InnerText = GetNodeIndex (n2.Split (':') [1]);
			XmlNode distance = xmlDoc.CreateElement ("distance");
			distance.InnerText = "0.0";
			XmlNode angle = xmlDoc.CreateElement ("angle");
			angle.InnerText = "0.0";

			cNode.AppendChild (m1);
			cNode.AppendChild (m2);
			cNode.AppendChild (node1);
			cNode.AppendChild (node2);
			cNode.AppendChild (distance);
			cNode.AppendChild (angle);
			connectionsNode.AppendChild (cNode);
		}


		string filename = Path.Combine (Application.dataPath, name + ".conf");
		xmlDoc.Save (filename);
	}

	public void Load () {

	}

	string GetNodeIndex (string node) {
		if (node == "FrontWheel") return "0";
		if (node == "LeftWheel") return "1";
		if (node == "RightWheel") return "2";
		return "3";
	}
}
