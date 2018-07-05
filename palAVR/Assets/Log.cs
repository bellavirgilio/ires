using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Log : MonoBehaviour {

    private StreamWriter sw;

	// Use this for initialization
	void Start () {
        string path = "";
        if (!Directory.Exists (Application.dataPath + "/Logs"))
        {
            Directory.CreateDirectory (Application.dataPath + "/Logs");         
        }
        path = Application.dataPath + "/Logs/logInfo.txt";
        sw = new StreamWriter (path, true);
        try
        {
            sw.WriteLine (System.DateTime.Today.ToString ());
        }
        catch (IOException e)
        {
            Debug.Log ("Caught IO Ex" + ":" + e.Message);
        }

        Application.ExternalCall ("logInfo");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void logInfo ()
    {
        GameObject.Find ("ideaal").SetActive (false);
        sw.WriteLine ("Successful JS Call");
    }

    private void OnApplicationQuit ()
    {
        sw.Close ();
    }
}

