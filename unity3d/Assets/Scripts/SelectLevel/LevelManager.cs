using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

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
        string[] taskDirectories = Directory.GetDirectories(levelDirectory);

        foreach (string taskDirectory in taskDirectories) {
            if (System.IO.File.Exists (Path.ChangeExtension (taskDirectory, ".unity"))) {

                string taskName = Path.GetFileNameWithoutExtension (taskDirectory);
                levelList.taskDictionary.Add (taskName, new List<Level> ());


                string[] workspaceScenceFiles = Directory.GetFiles (taskDirectory, "*.unity");
                foreach (string workspaceScenceFile in workspaceScenceFiles) {
                    Level l = new Level ();

                    l.name = Path.GetFileNameWithoutExtension (workspaceScenceFile);

                    string line;
                    l.intro = "";
                    System.IO.StreamReader file = new System.IO.StreamReader (Path.ChangeExtension (workspaceScenceFile, ".txt"));
                    while ((line = file.ReadLine ()) != null) {
                        l.intro += line;
                    }

                    l.thingToDo.AddListener (() => { SelectLevel (l); });

                    levelList.taskDictionary[taskName].Add (l);
                }
            }
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
                Application.LoadLevelAdditive (levelList.selectedTask);
			}
        }
    }
}
