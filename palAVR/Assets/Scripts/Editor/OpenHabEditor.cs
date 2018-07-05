using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class OpenHabInspector : EditorWindow
{

    // Use this for initialization
    [MenuItem ("OpenHab/openHAB Inspector")]
    public static void ShoWInspectorWindow ()
    {
        var window = GetWindow<OpenHabInspector> ();
    }


    private void OnGUI ()
    {
        EditorGUILayout.LabelField ("Base Settings", EditorStyles.boldLabel);

        bool groupEnabled = true;

        //bool groupEnabled = EditorGUILayout.BeginToggleGroup ("More Settings", groupEnabled);
        //Stuff that must be enabled
        EditorGUILayout.EndToggleGroup ();
    }
}
