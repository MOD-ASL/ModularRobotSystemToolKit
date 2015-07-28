using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class SaveLoadManagerNew : MonoBehaviour {

    public Ma2MaComManager ma2MaComManager;
    public enum FileType {Configuration, Behavior};
    private XmlDocument xmlDoc;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickSaveConfiguration () {
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.saveMenuDirector.ShowPanelOrNot (true);
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.saveMenuDirector.fileType = FileType.Configuration;
    }

	public void OnClickLoadOrInsertConfiguration () {
		if (ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("InsertConf").status || ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("LoadConf").status) {
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (true);
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.fileType = FileType.Configuration;
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ClearList ();
			StartCoroutine (ma2MaComManager.networkManager.GetFileList (DisplayFileList, FileType.Configuration.ToString ()));
		}
		else {
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (false);
		}
	}

	public void OnClickSaveBehavior () {
		if (ma2MaComManager.behaviorManager.currentBehaviorObject.listOfRobotStateObjects.Count == 0) {
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Cannot save behavior: No robot state added.");
			return;
		}
		ma2MaComManager.ma2UIComManager.uI2MaComDirector.saveMenuDirector.ShowPanelOrNot (true);
		ma2MaComManager.ma2UIComManager.uI2MaComDirector.saveMenuDirector.fileType = FileType.Behavior;
	}

	public void OnClickLoadBehavior () {
		if (ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("LoadBehavior").status) {
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (true);
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.fileType = FileType.Behavior;
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ClearList ();
			StartCoroutine (ma2MaComManager.networkManager.GetFileList (DisplayFileList, FileType.Behavior.ToString ()));
		}
		else {
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (false);
		}
	}

    public void DisplayFileList (string fileList, string fileType) {
        
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (true);
        
        xmlDoc = new XmlDocument ();
        xmlDoc.LoadXml(fileList);
        XmlNode list = xmlDoc.SelectSingleNode (fileType + "_list");
        
		foreach (XmlNode el in list.SelectNodes (fileType)) {
            string name = el.SelectSingleNode ("name").InnerText;
            string userName = el.SelectSingleNode ("user_name").InnerText;
            string url = el.SelectSingleNode ("url").InnerText;
			string configurationID = el.SelectSingleNode ("configuration_id").InnerText;
            File newFile = new File (name, userName, url, fileType, DownloadAndLoad);
			newFile.configurationID = configurationID;
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.fileList.Add (newFile);
        }
        
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.PopulateList ();
    }

	public void DownloadAndLoad (File file) {
		if (file.fileType == FileType.Configuration.ToString ()) {
			if (ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("InsertConf").status) {
				StartCoroutine (ma2MaComManager.networkManager.Download (file, InsertConfiguration));
			}
			else {
				StartCoroutine (ma2MaComManager.networkManager.Download (file, LoadConfiguration));
			}
		}
		else if (file.fileType == FileType.Behavior.ToString ()) {
			StartCoroutine (ma2MaComManager.networkManager.Download (file, LoadBehavior));
		}
        
    }

	public void SaveCurrentBehavior (string fileName, string userName) {
		XmlSerializer serializer = new XmlSerializer (typeof (BehaviorObject));
		
		XmlWriterSettings settings = new XmlWriterSettings ();
		settings.Encoding = new UnicodeEncoding (false, false); // no BOM in a .NET string
		settings.Indent = false;
		settings.OmitXmlDeclaration = false;
		
		using (StringWriter textWriter = new StringWriter ()) {
			using (XmlWriter xmlWriter = XmlWriter.Create (textWriter, settings)) {
				serializer.Serialize(xmlWriter, ma2MaComManager.behaviorManager.currentBehaviorObject);
			}
			StartCoroutine (ma2MaComManager.networkManager.SaveFile (textWriter.ToString(), fileName, userName, FileType.Behavior.ToString ()));
		}
		
		FileStream stream = new FileStream (Application.persistentDataPath + "/" + fileName + ".xml", FileMode.Create);
		serializer.Serialize (stream, ma2MaComManager.behaviorManager.currentBehaviorObject);
		stream.Close ();
	}

    public void SaveCurrentConfiguration (string fileName, string userName) {
        XmlSerializer serializer = new XmlSerializer (typeof (RobotStateObject));

        XmlWriterSettings settings = new XmlWriterSettings ();
        settings.Encoding = new UnicodeEncoding (false, false); // no BOM in a .NET string
        settings.Indent = false;
        settings.OmitXmlDeclaration = false;
        
        using (StringWriter textWriter = new StringWriter ()) {
            using (XmlWriter xmlWriter = XmlWriter.Create (textWriter, settings)) {
                serializer.Serialize(xmlWriter, ma2MaComManager.robotManager.GetRobotStateObject (true));
            }
			StartCoroutine (ma2MaComManager.networkManager.SaveFile (textWriter.ToString(), fileName, userName, FileType.Configuration.ToString ()));
        }

        FileStream stream = new FileStream (Application.persistentDataPath + "/" + fileName + ".xml", FileMode.Create);
        serializer.Serialize (stream, ma2MaComManager.robotManager.GetRobotStateObject (true));
        stream.Close ();
    }

    public void LoadConfiguration (File file, string fileContent) {
        XmlSerializer serializer = new XmlSerializer (typeof (RobotStateObject));

        XmlReaderSettings settings = new XmlReaderSettings ();
        // No settings need modifying here
        
        using (StringReader textReader = new StringReader (fileContent)) {
            using (XmlReader xmlReader = XmlReader.Create (textReader, settings)) {
                RobotStateObject rso = serializer.Deserialize (xmlReader) as RobotStateObject;
                StartCoroutine (ma2MaComManager.robotManager.ReplaceRobot (rso));
            }
        }

		ma2MaComManager.robotManager.currentConfigurationID = file.configurationID;
    }

	public void InsertConfiguration (File file, string fileContent) {
		XmlSerializer serializer = new XmlSerializer (typeof (RobotStateObject));
		
		XmlReaderSettings settings = new XmlReaderSettings ();
		// No settings need modifying here
		
		using (StringReader textReader = new StringReader (fileContent)) {
			using (XmlReader xmlReader = XmlReader.Create (textReader, settings)) {
				RobotStateObject rso = serializer.Deserialize (xmlReader) as RobotStateObject;
				ma2MaComManager.robotManager.SpawnRobot (rso);
			}
		}
		
		ma2MaComManager.robotManager.currentConfigurationID = file.configurationID;
	}

	public void LoadBehavior (File file, string fileContent) {
		if (file.configurationID != ma2MaComManager.robotManager.currentConfigurationID) {
			ma2MaComManager.ma2UIComManager.uI2MaComDirector.statusBarDirector.SetTempTextMessage ("Cannot load behavior: Wrong configuration.");
			return;
		}

		XmlSerializer serializer = new XmlSerializer (typeof (BehaviorObject));
		
		XmlReaderSettings settings = new XmlReaderSettings ();
		// No settings need modifying here
		
		using (StringReader textReader = new StringReader (fileContent)) {
			using (XmlReader xmlReader = XmlReader.Create (textReader, settings)) {
				BehaviorObject bo = serializer.Deserialize (xmlReader) as BehaviorObject;
				ma2MaComManager.behaviorManager.currentBehaviorObject = bo;
				ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.ClearAllButtons ();
				foreach (RobotStateObject rso in bo.listOfRobotStateObjects) {
					rso.button = ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelBehaviorDirector.AddButton (rso);
				}
				ma2MaComManager.behaviorManager.robotStateObjectIndex = 0;
			}
		}
	}

    public void Load (string fileName, string fileContent) {
        Debug.Log (fileName);
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (false);
//        XmlSerializer serializer = new XmlSerializer (typeof (BehaviorObject));
//        FileStream stream = new FileStream (Application.persistentDataPath + "/testSave.xml", FileMode.Open);
//        BehaviorObject container = serializer.Deserialize (stream) as BehaviorObject;
//
//        Debug.Log (container.listOfRobotStateObjects[0].listOfModuleStateObjects[0].listOfJointCommands[0].commandType);
//        stream.Close ();
    }
}
