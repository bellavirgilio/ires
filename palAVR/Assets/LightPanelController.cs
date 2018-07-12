using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPanelController : MonoBehaviour {

    public float lastX;
    public float lastZ;
    public float lastSpeed;
    public float currentSpeed;
    public float deltaSpeed;

	// Use this for initialization
	void Start () {
        lastX = transform.position.x;
        lastZ = transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
        currentSpeed = GetDistance(transform.position.x, lastX, transform.position.z, lastZ) * 60f;
        deltaSpeed = currentSpeed - lastSpeed;
        Debug.Log("Delta Speed: " + deltaSpeed);
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

        if (deltaSpeed < 0) { // if the car is slowing down
            panel.SetColor("_EmissionColor", Color.yellow);
            leftLight.color = Color.yellow;
            rightLight.color = Color.yellow;
        } if (deltaSpeed < 0.000001f && deltaSpeed > -0.000001f) { // if the car is stopped
            panel.SetColor("_EmissionColor", Color.green);
            leftLight.color = Color.green;
            rightLight.color = Color.green;
        } else {
            panel.SetColor("_EmissionColor", Color.red);
            leftLight.color = Color.red;
            rightLight.color = Color.red;
        }
    }
}
