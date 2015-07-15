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

    public void OnClickLoadConfiguration () {
        if (ma2MaComManager.modeManagerNew.GetOrCreateModeByName ("LoadConf").status) {
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (true);
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.fileType = FileType.Configuration;
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ClearList ();
            StartCoroutine (ma2MaComManager.networkManager.GetFileList (DisplayFileList));
        }
        else {
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (false);
        }
    }

    public void DisplayFileList (string fileList) {
        
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.ShowPanelOrNot (true);
        
        xmlDoc = new XmlDocument ();
        xmlDoc.LoadXml(fileList);
        XmlNode config_list = xmlDoc.SelectSingleNode ("config_list");
        
        foreach (XmlNode el in config_list.SelectNodes ("config")) {
            string name = el.SelectSingleNode ("name").InnerText;
            string userName = el.SelectSingleNode ("user_name").InnerText;
            string url = el.SelectSingleNode ("url").InnerText;
            
            File newFile = new File (name, userName, url, DownloadAndLoad);
            ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.fileList.Add (newFile);
        }
        
        ma2MaComManager.ma2UIComManager.uI2MaComDirector.panelFileSelectionDirector.PopulateList ();
    }

    public void DownloadAndLoad (File file) {
        StartCoroutine (ma2MaComManager.networkManager.Download (file, LoadConfiguration));
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
            StartCoroutine (ma2MaComManager.networkManager.SaveConfig (textWriter.ToString(), fileName, userName));
        }

        //FileStream stream = new FileStream (Application.persistentDataPath + "/" + fileName + ".xml", FileMode.Create);
        //serializer.Serialize (stream, ma2MaComManager.robotManager.GetRobotStateObject (true));
        //stream.Close ();

    }

    public void LoadConfiguration (string fileName, string fileContent) {

        XmlSerializer serializer = new XmlSerializer (typeof (RobotStateObject));

        XmlReaderSettings settings = new XmlReaderSettings ();
        // No settings need modifying here
        
        using (StringReader textReader = new StringReader (fileContent)) {
            using (XmlReader xmlReader = XmlReader.Create (textReader, settings)) {
                RobotStateObject rso = serializer.Deserialize (xmlReader) as RobotStateObject;
                ma2MaComManager.robotManager.SpawnRobot (rso);
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
