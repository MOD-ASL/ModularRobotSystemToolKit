using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

public class NetworkManager : MonoBehaviour {

    void Start () {
        //StartUpload();
    }

    void StartUpload()
    {
        StartCoroutine("Download");
    }

    IEnumerator Download()  
    {
        WWW w = new WWW("http://jimjing.koding.io:8080/myapp/files");
        yield return w;
        //Debug.Log(w.text);

        XmlDocument xmlDoc;
        xmlDoc = new XmlDocument ();
        xmlDoc.LoadXml(w.text);
        XmlNode ul = xmlDoc.SelectSingleNode ("html/body/ul");
        
        foreach (XmlNode el in ul.SelectNodes ("li/a")) {
            Debug.Log(el.InnerText);
        }

    }

    IEnumerator UploadLevel()  
    {
        //making a dummy xml level file
        XmlDocument map = new XmlDocument();
        map.LoadXml("<level></level>");
        
        //converting the xml to bytes to be ready for upload
        byte[] levelData =Encoding.UTF8.GetBytes(map.OuterXml);
        
        //generate a long random file name , to avoid duplicates and overwriting
        string fileName = Path.GetRandomFileName();
        fileName = fileName.Substring(0,6);
        fileName = fileName.ToUpper();
        fileName = fileName + ".xml";
        
        //if you save the generated name, you can make people be able to retrieve the uploaded file, without the needs of listings
        //just provide the level code name , and it will retrieve it just like a qrcode or something like that, please read below the method used to validate the upload,
        //that same method is used to retrieve the just uploaded file, and validate it
        //this method is similar to the one used by the popular game bike baron
        //this method saves you from the hassle of making complex server side back ends which enlists available levels
        //this way you could enlist outstanding levels just by posting the levels code on a blog or forum, this way its easier to share, without the need of user accounts or install procedures
        WWWForm form = new WWWForm();
        
        print("form created ");

        form.AddField("action", "/myapp/list/");

        //form.AddField("password", "111");
        
        form.AddField("docfile","docfile");
        
        form.AddBinaryData ( "docfile", levelData, fileName,"multipart/form-data");
       
        print("binary data added ");
        //change the url to the url of the php file
        WWW w = new WWW("http://jimjing.koding.io:8080/myapp/list/",form);
        print("www created");
        
        yield return w;
        print("after yield w");
        if (w.error != null)
        {
            print("error");
            print ( w.error );    
        }
    }
}
