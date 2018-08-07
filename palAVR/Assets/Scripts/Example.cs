using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Example : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //DOTweenPath newPath = new DOTweenPath();
        // newPath.onStart += onStartHandler;
	}

    // Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider col)
    {
        Debug.Log("Collider entered");
        if (col.gameObject.name == "Hydrant")
        {
            Debug.Log("Object detected. Slowing down.");
            // DOTween.timeScale = 0.5f;
            DOTween.PauseAll();
        }
    }
}
