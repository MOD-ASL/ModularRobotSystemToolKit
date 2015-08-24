using UnityEngine;
using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Text;
using System.Xml.Serialization;

public class NetworkManager : MonoBehaviour {

	public Ma2MaComManager ma2MaComManager;
    Uri baseUri = new Uri("http://45.79.174.242:8080");

    void Start () {
        //StartUpload ();
    }

    void StartUpload () {
        StartCoroutine ("GetFileList");
    }

    public IEnumerator Download (File file, System.Action<File, string> callback) {
        Uri myUri = new Uri(baseUri, file.url);
        WWW w = new WWW (myUri.ToString ());
        yield return w;
        //Debug.Log(w.text);
        callback (file, w.text);
    }

    public IEnumerator GetFileList (System.Action<string, string> callback, string fileType) {
        WWWForm form = new WWWForm ();
        form.AddField ("data_type", fileType);
        WWW w = new WWW ("http://45.79.174.242:8080/fileserver/files/list", form);
        yield return w;
        callback (w.text, fileType);
    }

	public IEnumerator SaveFile (string data, string fileName, string userName, string fileType) {
		
		//converting the xml to bytes to be ready for upload
		byte[] byteData = Encoding.UTF8.GetBytes (data);
		
		fileName = fileName + ".xml";
		
		WWWForm form = new WWWForm();
		
		form.AddField ("action", "fileserver/upload/handler");
		form.AddField ("name", fileName);
		form.AddField ("data_type", fileType);
		form.AddField ("user_name", userName);
		form.AddField ("client", "unity");
		if (fileType == "Configuration") {
			string id = System.Guid.NewGuid ().ToString ();
			ma2MaComManager.robotManager.currentConfigurationID = id;
			form.AddField ("configuration_id", id);
		}
		else if (fileType == "Behavior") {
			form.AddField ("configuration_id", ma2MaComManager.robotManager.currentConfigurationID);
		}

		
		form.AddField ("data_file","data_file");
		form.AddBinaryData ("data_file", byteData, fileName,"multipart/form-data");
		
		WWW w = new WWW ("http://45.79.174.242:8080/fileserver/upload/handler",form);
		
		yield return w;
		
		if (w.error != null)
		{
			print ("error");
			print ( w.error );    
		}
	}

    public IEnumerator SaveConfig (string data, string configName, string userName) {
        
        //converting the xml to bytes to be ready for upload
        byte[] configData = Encoding.UTF8.GetBytes (data);

        string fileName = configName + ".xml";

        WWWForm form = new WWWForm();
        
        form.AddField ("action", "fileserver/upload/handler");
        form.AddField ("name", configName);
        form.AddField ("data_type", "Configuration");
        form.AddField ("user_name", userName);
        form.AddField ("client", "unity");
        
        form.AddField ("data_file","data_file");
        form.AddBinaryData ("data_file", configData, fileName,"multipart/form-data");
        
        WWW w = new WWW ("http://45.79.174.242:8080/fileserver/upload/handler",form);
        
        yield return w;
        
        if (w.error != null)
        {
            print ("error");
            print ( w.error );    
        }
        
        Debug.Log (w.text);
    }

    public IEnumerator SaveConfig (XmlDocument configFile, string configName, string userName) {             
        SaveConfig (configFile.OuterXml, configName, userName);
        yield return null;
    }
}
