using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.IO;

namespace VRoad
{

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

        void Start ()
        {
            dataCollection = GetComponent<DataCollection> () as DataCollection;
            fileName = string.Format ("{0}_@_ {1} ", participantNumber.ToString (), DateTime.Now.ToString ("dd-MM-HH-mm"));
            filePath = Application.dataPath;

            if (participantNumber == 0)
            {
                Debug.Break ();
                Debug.LogWarning ("No Participant ID has been set. Please set a Participant ID");
            }
        }

        public void WriteLog ( string log )
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
    }
}
