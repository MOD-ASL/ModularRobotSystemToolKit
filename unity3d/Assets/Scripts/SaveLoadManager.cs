using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System;
using System.Xml.Serialization;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveLoadManager : MonoBehaviour {
	public Transform SMORES;
	enum jointNameEnum {FrontWheel, LeftWheel, RightWheel, Body};
	enum nodeNameEnum {FrontWheel, LeftWheel, RightWheel, BackPlate};
	public Hashtable connectionTable = new Hashtable ();
	public int newConfCount;
    public NetworkManager networkManager;
    public FileSelectionManager fileSelectionManager;
    public Designer designer;
    public Text userName;
	GameObject newConf;
	XmlDocument xmlDoc;

	// Use this for initialization
	void Start () {
		newConfCount = 0;
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
			doneList.Add (n2);

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

//		#if UNITY_EDITOR
//		string filename = EditorUtility.SaveFilePanel(
//			"Save Configuration",
//			Application.dataPath,
//			name + ".conf",
//			"conf");
//		#else
//		string filename = Path.Combine (Application.dataPath, name + ".conf");
//		#endif
//		xmlDoc.Save (filename);
        StartCoroutine (networkManager.SaveConfig (xmlDoc, name, userName.text));
	}

    public void DownloadAndLoad (File file) {
        StartCoroutine (networkManager.Download (file, Load));
    }

	public void Load (string fileContent, string name) {

//		#if UNITY_EDITOR
//		string filename = EditorUtility.OpenFilePanel(
//			"Save Configuration",
//			Application.dataPath,
//			"conf");
//		#else
//		string filename = "";
//		#endif

   		newConf = new GameObject ();
        newConf.name = name;
		connectionTable = new Hashtable ();

		xmlDoc = new XmlDocument ();
        xmlDoc.LoadXml(fileContent);
		XmlNode modulesEL = xmlDoc.SelectSingleNode ("configuration/modules");

		foreach (XmlNode el in modulesEL.SelectNodes ("module")) {
			CreateModule (el);
		}

        fileSelectionManager.ShowPanelOrNot (false);
        designer.OnLoadConfig (newConf);
	}

    public void OnClickLoad () {
        fileSelectionManager.ClearList ();
        StartCoroutine (networkManager.GetFileList (DisplayFileList));
    }

    public void DisplayFileList (string fileList) {

        fileSelectionManager.ShowPanelOrNot (true);

        xmlDoc = new XmlDocument ();
        xmlDoc.LoadXml(fileList);
        XmlNode config_list = xmlDoc.SelectSingleNode ("config_list");

        foreach (XmlNode el in config_list.SelectNodes ("config")) {
            string name = el.SelectSingleNode ("name").InnerText;
            string userName = el.SelectSingleNode ("user_name").InnerText;
            string url = el.SelectSingleNode ("url").InnerText;

            File newFile = new File (name, userName, url, DownloadAndLoad);
            fileSelectionManager.fileList.Add (newFile);
        }

        fileSelectionManager.PopulateList ();
    }

	public void ConnectAllModules () {
		XmlNode connectionsEL = xmlDoc.SelectSingleNode ("configuration/connections");
		foreach (XmlNode el in connectionsEL.SelectNodes ("connection")) {
			string module1Name = el.SelectSingleNode ("module1").InnerText;
			string module2Name = el.SelectSingleNode ("module2").InnerText;
			string node1Name = Enum.GetName(typeof(nodeNameEnum), int.Parse(el.SelectSingleNode ("node1").InnerText));
			string node2Name = Enum.GetName(typeof(nodeNameEnum), int.Parse(el.SelectSingleNode ("node2").InnerText));
			
			Transform module1 = newConf.transform.FindChild (module1Name+"_"+newConf.name);
			Transform module2 = newConf.transform.FindChild (module2Name+"_"+newConf.name);

			Transform node1 = module1.FindChild(node1Name);
			Transform node2 = module2.FindChild(node2Name);
			
			Connect (node1.gameObject, node2.gameObject);
			module1.GetComponent<ModuleController> ().SetMode (1);
			module2.GetComponent<ModuleController> ().SetMode (1);
			module1.GetComponent<ModuleController> ().SetToTrigger (false);
			module2.GetComponent<ModuleController> ().SetToTrigger (false);
		}
	}

	Transform CreateModule (XmlNode el) {
		string name = el.SelectSingleNode ("name").InnerText;
		string[] position_str = el.SelectSingleNode ("position").InnerText.Split(' ');
		string[] joints_str = el.SelectSingleNode ("joints").InnerText.Split(' ');
		
		float x = float.Parse(position_str[0]);
		float y = float.Parse(position_str[1]);
		float z = float.Parse(position_str[2]);
		Quaternion rot;
		
		if (position_str.Length == 6) {
			rot = Quaternion.identity;
		}
		else {
			float rotW = float.Parse(position_str[3]);
			float rotX = float.Parse(position_str[4]);
			float rotY = float.Parse(position_str[5]);
			float rotZ = float.Parse(position_str[6]);

			rot = new Quaternion(rotX, rotY, rotZ, rotW);
		}
		
		Transform clone = Instantiate(SMORES, new Vector3(x, y, z), rot) as Transform;
		clone.name = name+"_"+newConf.name;
		clone.transform.Rotate (Vector3.right, 90.0f);
		clone.SetParent(newConf.transform);
		clone.GetComponent<ModuleController> ().SetToTrigger (true);

		int id = -1;
		foreach(string joint_str in joints_str) {
			id++;
			string jointName = Enum.GetName(typeof(jointNameEnum), id);
			clone.GetComponent<ModuleController>().UpdateJointAngle (float.Parse(joint_str), jointName);
		}
		return clone;
	}

	void Connect (GameObject m1, GameObject m2) {
		connectionTable.Add (m1.transform.parent.name+":"+m1.name, m2.transform.parent.name+":"+m2.name);
		connectionTable.Add (m2.transform.parent.name+":"+m2.name, m1.transform.parent.name+":"+m1.name);
		FixedJoint j = m1.AddComponent<FixedJoint> ();
		j.connectedBody = m2.GetComponent<Rigidbody> ();
		m1.transform.parent.GetComponent<ModuleController> ().OnConnectNode (m1, m2);
		m2.transform.parent.GetComponent<ModuleController> ().OnConnectNode (m2, m1);
	}

	string GetNodeIndex (string node) {
		if (node == "FrontWheel") return "0";
		if (node == "LeftWheel") return "1";
		if (node == "RightWheel") return "2";
		return "3";
	}
}
