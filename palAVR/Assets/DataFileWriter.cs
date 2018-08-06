using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataFileWriter : MonoBehaviour {

    public DataCollection dataCollection;
    public string log;

    void Start()
    {
        dataCollection = GameObject.FindWithTag("PedCamera").GetComponent<DataCollection>() as DataCollection;
    }

    //static void Main()
    //{
    //    // Write one string to a text file.
    //    string text = "A class is the most powerful data type in C#. Like a structure, " +
    //                   "a class defines the data and behavior of the data type. ";
    //    // WriteAllText creates a file, writes the specified string to the file,
    //    // and then closes the file.    You do NOT need to call Flush() or Close().
    //   System.IO.File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", text);
    //}

    void Update()
    {
        if (dataCollection.totalCars == 8) {
            log = dataCollection.PedLog();

            System.IO.File.WriteAllText(@"Desktop\IRES\StudyData\Test.txt", log);

            Debug.Log(log);
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
