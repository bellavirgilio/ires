using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour {

    public float lastCarPositionX;
    public float lastCarPositionZ;
    public float lastSpeed;
    public float deltaSpeed;

	// Use this for initialization
	void Start () {
        lastCarPositionX = transform.position.x;
        lastCarPositionZ = transform.position.z;
	}

	
    // Update is called once per frame
    void Update()
    {
        float deltaX = transform.position.x - lastCarPositionX;
        float deltaZ = transform.position.z - lastCarPositionZ;
        float distance = GetDistance(deltaX, deltaZ); // distance traveled in 1/60 of a second
        float speed = GetDistance(deltaX, deltaZ) * 60f; // speed per second
        float wheelCircumference = Mathf.PI * 0.65f; // wheel diameter = 0.65f
        float wheelRevsPerSec = speed / wheelCircumference;

        deltaSpeed = speed - lastSpeed;

        // update runs 60 times per second

        // rotates around local X axis at 200 degrees per second
        transform.Rotate(Time.deltaTime * wheelRevsPerSec * -200f, 0, 0);
        lastCarPositionX = transform.position.x;
        lastCarPositionZ = transform.position.z;
        lastSpeed = speed;
    }

    // gets the distance between two points using x and z coords
    public float GetDistance(float deltaX, float deltaZ) {
        return Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
    }
}
