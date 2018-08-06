using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataFileWriter : MonoBehaviour
{

    public DataCollection dataCollection;
    public string log;
    public string fileName;
    public string filePath;


    void Start()
    {
        dataCollection = GameObject.FindWithTag("PedCamera").GetComponent<DataCollection>() as DataCollection;
        fileName = string.Format("{0}", DateTime.Now.ToString("f"));
        filePath = Application.dataPath;
    }

    void Update()
    {
        int totalCars = dataCollection.totalCars;

        if (dataCollection.totalCars == 8)
        {
            log = dataCollection.PedLog();
#if UNITY_EDITOR_OSX
            System.IO.File.WriteAllText(@"" + filePath + "/Study/" + fileName + ".txt", log);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
            File.WriteAllText("C:/Study/")
#endif
            //Debug.Log(log);
            totalCars++;
        }
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
