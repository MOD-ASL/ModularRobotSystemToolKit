using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class File {
    public string name;
    public string userName;
    public string url;
	public string fileType;
	public string configurationID;
    public Button.ButtonClickedEvent thingToDo;
    
    public File (string n, string user, string u, string fType, System.Action<File> callback) {
        name = n;
        userName = user;
        url = u;
		fileType = fType;
        thingToDo = new Button.ButtonClickedEvent ();
		thingToDo.AddListener (() => {callback (this);});
    }
}
