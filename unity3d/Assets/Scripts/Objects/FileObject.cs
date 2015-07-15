using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class File {
    public string name;
    public string userName;
    public string url;
    public Button.ButtonClickedEvent thingToDo;
    
    public File (string n, string user, string u, System.Action<File> callback) {
        name = n;
        userName = user;
        url = u;
        thingToDo = new Button.ButtonClickedEvent ();
        thingToDo.AddListener (() => {callback (this);});
    }
}
