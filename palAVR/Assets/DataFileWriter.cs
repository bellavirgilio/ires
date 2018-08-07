using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;

public class DataFileWriter : MonoBehaviour
{
    [HideInInspector]
    public DataCollection dataCollection;
    [HideInInspector]
    public string log;
    [HideInInspector]
    public string fileName;
    [HideInInspector]
    public string filePath;

    [SerializeField]
    int participantNumber;

    private bool logged = false;

    void Start()
    {
        dataCollection = GameObject.FindWithTag("PedCamera").GetComponent<DataCollection>() as DataCollection;
        fileName = string.Format ("{0}_@_ {1} ", participantNumber.ToString (), DateTime.Now.ToString ("dd-MM-HH-mm"));
        filePath = Application.dataPath;

        if (participantNumber == 0)
        {
            Debug.Break ();
            Debug.LogWarning ("No Participant ID has been set. Please set a Participant ID");
        }
    }

    void Update()
    {
    //    int totalCars = dataCollection.totalCars;

    //    if (dataCollection.totalCars == 8)
    //    {
    //        log = dataCollection.PedLog();
    //        if (!logged)
    //            WriteLog ();
    //        //Debug.Log(log);

    //        totalCars++;
    //    }
   }

    public void WriteLog(string log)
    {
      
#if UNITY_EDITOR_OSX
            System.IO.File.WriteAllText(@"" + filePath + "/Study/" + fileName + ".txt", log);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_64
        if (!Directory.Exists (Application.dataPath + "/Study/"))
            Directory.CreateDirectory (Application.dataPath + "/Study/");

      //  File.WriteAllText (Application.persistentDataPath + "/Study/" +fileName + ".txt", log);
        System.IO.File.WriteAllText (Application.dataPath + "\\Study\\" + fileName + ".txt", log);
#endif
        logged = true;
    }
    //Output (to WriteLines.txt):
    //   First line
    //   Second line
    //   Third line

    //Output (to WriteText.txt):
    //   A class is the most powerful data type in C#. Like a structure, a class defines the data and behavior of the data type.

    //Output to WriteLines2.txt after Example #3:
    //   First line
    //   Third line

    //Output to WriteLines2.txt after Example #4:
    //   First line
    //   Third line
    //   Fourth line
}
