using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Crossing : MonoBehaviour {

    public Stopwatch stopwatch = new Stopwatch();
    public TimeSpan ts = new TimeSpan();

    private float pedXStart;
    private float pedX;
    private float pedZ;
    private bool inZoneA;
    private bool inZoneB;

	// Use this for initialization
	void Start () {

        pedXStart = this.transform.position.x;

        if (pedX >= 0.86f) {
            Debug.Log("Starting in Zone B");
        } if (pedX <= -6.44f) {
            Debug.Log("Starting in Zone A");
        }
	}
	
	// Update is called once per frame
	void Update () {
        pedX = this.transform.position.x;
        float pedXdiff = pedX - pedXStart;

        // if the pedestrian is moving
        if (pedXdiff < 0.01f && pedXdiff > -0.01f) {
            stopwatch.Start();
        }
	}
}

// zone B boundary : x = 0.86
// zone A boundary : x = -6.44
// start stopwatch when x < 0.86
// stop stopwatch when x < -6.44
// 