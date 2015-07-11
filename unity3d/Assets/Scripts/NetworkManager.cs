using UnityEngine;
using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Text;
using System.Xml.Serialization;

public class NetworkManager : MonoBehaviour {

    Uri baseUri = new Uri("http://45.79.174.242:8080");

    void Start () {
        //StartUpload ();
    }

    void StartUpload () {
        StartCoroutine ("GetFileList");
    }

    public IEnumerator Download (File file, System.Action<string, string> callback) {
        Uri myUri = new Uri(baseUri, file.url);
        WWW w = new WWW (myUri.ToString ());
        yield return w;
        //Debug.Log(w.text);
        callback (w.text, file.name);
    }

    public IEnumerator GetFileList (System.Action<string> callback) {
        WWWForm form = new WWWForm ();
        form.AddField ("data_type", "Configuration");

        WWW w = new WWW ("http://45.79.174.242:8080/fileserver/files/list", form);
        yield return w;
        callback (w.text);
    }

    public IEnumerator SaveConfig (XmlDocument configFile, string configName, string userName) {
               
        //converting the xml to bytes to be ready for upload
        byte[] configData = Encoding.UTF8.GetBytes (configFile.OuterXml);
        
        //generate a long random file name , to avoid duplicates and overwriting
        string fileName = configName + ".xml";
        
        //if you save the generated name, you can make people be able to retrieve the uploaded file, without the needs of listings
        //just provide the level code name , and it will retrieve it just like a qrcode or something like that, please read below the method used to validate the upload,
        //that same method is used to retrieve the just uploaded file, and validate it
        //this method is similar to the one used by the popular game bike baron
        //this method saves you from the hassle of making complex server side back ends which enlists available levels
        //this way you could enlist outstanding levels just by posting the levels code on a blog or forum, this way its easier to share, without the need of user accounts or install procedures
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
}
