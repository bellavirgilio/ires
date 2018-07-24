using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class LightPanelController : MonoBehaviour {

    private float lastX;
    private float lastZ;
    private float lastSpeed;
    private float currentSpeed;
    private float deltaSpeed;

    private bool slowing = false;

    public GameObject myo;
    // private Pose _lastPose = Pose.Unknown;

	// Use this for initialization
	void Start () {
        lastX = transform.position.x;
        lastZ = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
        currentSpeed = GetDistance(transform.position.x, lastX, transform.position.z, lastZ) * 60f;
        deltaSpeed = currentSpeed - lastSpeed;
        // Debug.Log("Delta Speed: " + deltaSpeed);
        lastSpeed = currentSpeed;

        this.LightColor();
	}

    // gets the distance between two points using x and z coords
    public float GetDistance(float x1, float x2, float z1, float z2)
    {
        float deltaX = x2 - x1;
        float deltaZ = z2 - z1;
        return Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
    }

    public void LightColor() {
        Material panel = GetComponent<Renderer>().material;
        Light leftLight = GameObject.Find("LeftLight").GetComponent<Light>();
        Light rightLight = GameObject.Find("RightLight").GetComponent<Light>();

        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        if (thalmicMyo.pose == Pose.FingersSpread)
        {
            slowing = true;
            //Debug.Log("Slowing begins: ");
        } if (slowing) {
            panel.SetColor("_EmissionColor", Color.yellow);
            leftLight.color = Color.yellow;
            rightLight.color = Color.yellow;
            //Debug.Log("DeltaSpeed: " + deltaSpeed);
        }
        if (deltaSpeed < 0.000001f && deltaSpeed > -0.000001f) { // if the car is stopped
            //Debug.Log("Stopped");
            panel.SetColor("_EmissionColor", Color.green);
            leftLight.color = Color.green;
            rightLight.color = Color.green;
            slowing = false;
        } else if (!slowing) { // if the car is not slowing down and not stopped
            panel.SetColor("_EmissionColor", Color.red);
            leftLight.color = Color.red;
            rightLight.color = Color.red;
        }
    }
}
