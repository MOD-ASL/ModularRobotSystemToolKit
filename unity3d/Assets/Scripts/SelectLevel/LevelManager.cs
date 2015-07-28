using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;

[System.Serializable]
public class Level {
    public string name;
    public string intro;
    public Button.ButtonClickedEvent thingToDo = new Button.ButtonClickedEvent ();

    public Level () {

    }
}

public class LevelManager : MonoBehaviour {

    public LevelList levelList;
    public Image imagePreview;
    public Text textIntro;
    public Button buttonLoadLevel;

    Level selectedLevel;

	// Use this for initialization
	void Start () {
	    LoadLevels ();
        levelList.PopulateList ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadLevels () {
        string levelDirectory = Path.Combine (Application.dataPath, Path.Combine ("Scences", "Levels"));
        string [] fileEntries = Directory.GetFiles (levelDirectory, "*.unity");

        foreach(string fileName in fileEntries) {
            Level l = new Level ();

            l.name = Path.GetFileNameWithoutExtension (fileName);

            string line;
            l.intro = "";
            System.IO.StreamReader file = new System.IO.StreamReader (Path.ChangeExtension (fileName, ".txt"));
            while ((line = file.ReadLine()) != null) {
                l.intro += line;
            }

            l.thingToDo.AddListener (() => {SelectLevel(l);});

            levelList.levelList.Add (l);
        }
    }

    public void SelectLevel (Level l) {
        imagePreview.sprite = Resources.Load<Sprite> (l.name);
        textIntro.text = l.intro;
        selectedLevel = l;
    }

    public void onClickLoadLevel () {
        if (selectedLevel != null) {
			if (selectedLevel.name == "Blank") {
				Application.LoadLevel (selectedLevel.name);
			}
			else {
				Application.LoadLevel ("Blank");
				Application.LoadLevelAdditive (selectedLevel.name);
			}
        }
    }
}
