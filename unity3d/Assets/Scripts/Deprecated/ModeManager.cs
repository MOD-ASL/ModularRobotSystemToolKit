using UnityEngine;
using System.Collections;

public class ModeManager : MonoBehaviour {
	// Variables
	private bool isRecordBehavior = false;// Record behavior mode
    private bool isAddModule = false;// Add module mode
    private bool isSimulate = false;// Simulation mode
    private bool isSystem = false;// System setting mode
    private bool isConnectNodes = false;// Connect node mode

	public bool IsRecordBehavior {
		get { return isRecordBehavior; }
		set { isRecordBehavior = !isRecordBehavior; }
	}

    public bool IsAddModule {
        get { return isAddModule; }
        set { isAddModule = !isAddModule; }
    }

    public bool IsSimulate {
        get { return isSimulate; }
        set { isSimulate = !isSimulate; }
    }

    public bool IsSystem {
        get { return isSystem; }
        set { isSystem = !isSystem; }
    }

    public bool IsConnectNodes {
        get { return isConnectNodes; }
        set { isConnectNodes = !isConnectNodes; }
    }

    public void PrintModeState () {
        Debug.Log ("Record Behavor: " + isRecordBehavior.ToString ());
        Debug.Log ("Add Module: " + isAddModule.ToString ());
        Debug.Log ("Simulate: " + isSimulate.ToString ());
        Debug.Log ("System: " + isSystem.ToString ());
        Debug.Log ("Connect Nodes: " + isConnectNodes.ToString ());
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
