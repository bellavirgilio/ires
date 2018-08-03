using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics;
using System;

public class DriverPathManager : MonoBehaviour {
    // determining the speed of the vehicle
    private float lastX;
    private float lastZ;
    private float lastSpeed;
    private float currentSpeed;
    private float deltaSpeed;

    // path/sequence variables
    public PathType pathType = PathType.Linear;
    public Sequence sequence1;
    public Sequence sequence2;

    // stopping coordinate variables and pedestrian location (stopping point hard coded because of volatility with VR camera/limited scope of experiment)
    // private Transform pedestrian; // needs to be SteamVR/[CameraRig], use PedPlaceholder for now
    // private float pedX;
    // private float pedZ;
    private float stopX;
    private float stopZ;
    private Vector3 stop;
    public float kmSpeed; // km/hr
    private float speed;

    // waypoints for paths
    private Vector3[] slowdownPoints;
    private Vector3[] continuePoints;
    private Vector3[] initialPoints;

    // speed and distance variables
    private float slowdownPathDistance;
    private float continuePathDistance;
    private float nonstopPathDistance;
    public float slowdownPathTime;
    private float continuePathTime;
    private float nonstopPathTime;

    // coordinates of car when ped is sensed
    private float sensorX;
    private float sensorZ;
    private Vector3 sensorStop;
    private bool sensed; // becomes true OnTriggerEnter, false OnTriggerExit

    // keeps track of how many times a ped is sensed/functions that only run once
    private int senseCount;
    private bool oneTime;
    private bool oneTime2;

    // car behavior booleans for data collection
    public bool slowing;

    void Start()
    {
        lastX = transform.position.x;
        lastZ = transform.position.z;

        // scales km speed
        speed = kmSpeed / 3.6f;

        // sets the car's stopping point based on its direction
        if (gameObject.tag.Equals("car_north"))
        {
            stopX = -2f;
            // stopX = pedX + 3.5f;
            stopZ = 46.7f;
            //stopZ = pedZ + 7f;
            stop = new Vector3(stopX, 0.5f, stopZ);
        }
        if (gameObject.tag.Equals("car_south"))
        {
            stopX = 2f;
            //stopX = pedX + 7f;
            stopZ = 33f;
            //stopZ = pedZ - 7f;
            stop = new Vector3(stopX, 0.5f, stopZ);
        }

        // calculates time (speed) for each section of the path
        slowdownPathDistance = 60f; // estimate
        continuePathDistance = 308.708f;
        nonstopPathDistance = 420.78f;
        slowdownPathTime = slowdownPathDistance / speed / 2; // divided by 2 because it needs to be slower
        continuePathTime = continuePathDistance / speed;
        nonstopPathTime = nonstopPathDistance / speed;

        if (gameObject.tag.Equals("car_north")) {
            initialPoints = new[] { new Vector3(-6.23f, 0.5f, 130.4f), new Vector3(-3.36525f, 0.5f, 129.9668f), new Vector3(-1.734329f, 0.5f, 128.4844f), new Vector3(-1.842972f, 0.5f, 125.5889f), new Vector3(-1.760559f, 0.5f, 22.42285f), new Vector3(-1.502464f, 0.5f, -38.09794f), new Vector3(-1.756076f, 0.5f, -41.22349f), new Vector3(-4.163716f, 0.5f, -42.69579f), new Vector3(-7.230853f, 0.5f, -42.92153f), new Vector3(-23.32813f, 0.5f, -42.41516f), new Vector3(-32.60156f, 0.5f, -40.63574f), new Vector3(-36.08203f, 0.5f, -39.28369f), new Vector3(-40.13672f, 0.5f, -37.08124f), new Vector3(-43.65625f, 0.5f, -34.58752f), new Vector3(-47.67969f, 0.5f, -30.06952f), new Vector3(-50.73828f, 0.5f, -25.1709f), new Vector3(-51.85156f, 0.5f, -19.78284f), new Vector3(-53.3808f, 0.5f, -13.21484f), new Vector3(-52.84766f, 0.5f, 85.06738f), new Vector3(-52.45807f, 0.5f, 108.1045f), new Vector3(-50.43028f, 0.5f, 113.5117f), new Vector3(-48.01571f, 0.5f, 118.5967f), new Vector3(-43.88325f, 0.5f, 122.8066f), new Vector3(-39.57445f, 0.5f, 125.626f), new Vector3(-34.32539f, 0.5f, 128.3672f), new Vector3(-24.60913f, 0.5f, 129.6172f) };
        } if (gameObject.tag.Equals("car_south")) {
            initialPoints = new[] { new Vector3(5.88f, 0.5f, -42.3f), new Vector3(3.729492f, 0.5f, -42.01465f), new Vector3(2f, 0.5f, -39.77148f), new Vector3(2f, 0.5f, -35.18164f), new Vector3(2f, 0.5f, 65.3f), new Vector3(2f, 0.5f, 123.9922f), new Vector3(1.934553f, 0.5f, 126.453f), new Vector3(2.506611f, 0.5f, 128.8034f), new Vector3(4.788719f, 0.5f, 130.0849f), new Vector3(24.65626f, 0.5f, 130.2603f), new Vector3(34.32539f, 0.5f, 128.3672f), new Vector3(39.57445f, 0.5f, 125.626f), new Vector3(43.88325f, 0.5f, 122.8066f), new Vector3(48.01571f, 0.5f, 118.5967f), new Vector3(50.43028f, 0.5f, 113.5117f), new Vector3(52.45807f, 0.5f, 108.1045f), new Vector3(52.84766f, 0.5f, 85.06738f), new Vector3(53.3808f, 0.5f, -13.21484f), new Vector3(51.85156f, 0.5f, -19.78284f), new Vector3(50.73828f, 0.5f, -25.1709f), new Vector3(47.67969f, 0.5f, -30.06952f), new Vector3(43.65625f, 0.5f, -34.58752f), new Vector3(40.13672f, 0.5f, -37.08124f), new Vector3(36.08203f, 0.5f, -39.28369f), new Vector3(32.60156f, 0.5f, -40.63574f), new Vector3(23.32813f, 0.5f, -42.41516f) };
        }

        // nonstop path
        Tween initial = transform.DOPath(initialPoints, nonstopPathTime, pathType).SetLookAt(0.01f); // nonstop tween

        sequence1 = DOTween.Sequence();
        sequence1.Append(initial);
    }

    // when a pedestrian is sensed in the vehicle's collider, sensed becomes true
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("PedCamera"))
        {
            sensed = true;
            slowing = true;
            senseCount++;
            // UnityEngine.Debug.Log("collider entered");
        }
        // UnityEngine.Debug.Log("sensed is " + sensed);
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag.Equals("PedCamera"))
        {
            sensed = false;

            // UnityEngine.Debug.Log("collider exited");
        }
    }

    void Update()
    {
        // speed info
        currentSpeed = GetDistance(transform.position.x, lastX, transform.position.z, lastZ) * 60f;
        deltaSpeed = currentSpeed - lastSpeed;
        lastSpeed = currentSpeed;
        lastX = transform.position.x;
        lastZ = transform.position.z;

        // UnityEngine.Debug.Log("Current speed: " + currentSpeed);

        // if the pedestrian is in the collider, checks the car's position
        // and creates a point at which the car starts to slow down
        if (sensed && !oneTime)
        {
            sensorX = this.transform.position.x;
            sensorZ = this.transform.position.z;
            // UnityEngine.Debug.Log("sensorStop: " + "(" + sensorX + ", 0.5, " + sensorZ + ")");
            sensorStop = new Vector3(sensorX, 0.5f, sensorZ);
            oneTime = true;
        }

        // slowdown path
        slowdownPoints = new[] {
            sensorStop,
            stop};

        // path after the car stops (based on the direction of the car)
        if (gameObject.tag.Equals("car_north"))
        {
            continuePoints = new[] { stop, new Vector3(-1.760559f, 0.5f, 22.42285f), new Vector3(-1.502464f, 0.5f, -38.09794f), new Vector3(-1.756076f, 0.5f, -41.22349f), new Vector3(-4.163716f, 0.5f, -42.69579f), new Vector3(-7.230853f, 0.5f, -42.92153f), new Vector3(-23.32813f, 0.5f, -42.41516f), new Vector3(-32.60156f, 0.5f, -40.63574f), new Vector3(-36.08203f, 0.5f, -39.28369f), new Vector3(-40.13672f, 0.5f, -37.08124f), new Vector3(-43.65625f, 0.5f, -34.58752f), new Vector3(-47.67969f, 0.5f, -30.06952f), new Vector3(-50.73828f, 0.5f, -25.1709f), new Vector3(-51.85156f, 0.5f, -19.78284f), new Vector3(-53.3808f, 0.5f, -13.21484f), new Vector3(-52.84766f, 0.5f, 85.06738f), new Vector3(-52.45807f, 0.5f, 108.1045f), new Vector3(-50.43028f, 0.5f, 113.5117f), new Vector3(-48.01571f, 0.5f, 118.5967f), new Vector3(-43.88325f, 0.5f, 122.8066f), new Vector3(-39.57445f, 0.5f, 125.626f), new Vector3(-34.32539f, 0.5f, 128.3672f), new Vector3(-24.60913f, 0.5f, 129.6172f) };
        }
        if (gameObject.tag.Equals("car_south"))
        {
            continuePoints = new[] { stop, new Vector3(2f, 0.5f, 65.3f), new Vector3(2f, 0.5f, 123.9922f), new Vector3(1.934553f, 0.5f, 126.453f), new Vector3(2.506611f, 0.5f, 128.8034f), new Vector3(4.788719f, 0.5f, 130.0849f), new Vector3(24.65626f, 0.5f, 130.2603f), new Vector3(34.32539f, 0.5f, 128.3672f), new Vector3(39.57445f, 0.5f, 125.626f), new Vector3(43.88325f, 0.5f, 122.8066f), new Vector3(48.01571f, 0.5f, 118.5967f), new Vector3(50.43028f, 0.5f, 113.5117f), new Vector3(52.45807f, 0.5f, 108.1045f), new Vector3(52.84766f, 0.5f, 85.06738f), new Vector3(53.3808f, 0.5f, -13.21484f), new Vector3(51.85156f, 0.5f, -19.78284f), new Vector3(50.73828f, 0.5f, -25.1709f), new Vector3(47.67969f, 0.5f, -30.06952f), new Vector3(43.65625f, 0.5f, -34.58752f), new Vector3(40.13672f, 0.5f, -37.08124f), new Vector3(36.08203f, 0.5f, -39.28369f), new Vector3(32.60156f, 0.5f, -40.63574f), new Vector3(23.32813f, 0.5f, -42.41516f) };
        }

        if (!oneTime2 && senseCount == 1)
        {
            PathFind();
            oneTime2 = true;
        }
    }

    // gets the distance between two points using x and z coords
    public float GetDistance(float x1, float x2, float z1, float z2)
    {
        float deltaX = x2 - x1;
        float deltaZ = z2 - z1;
        return Mathf.Sqrt(deltaX * deltaX + deltaZ * deltaZ);
    }


    //TODO remove slowing + stopped variables... only use slowing
    // datacollection needs to access slowdownpathtime

    public void PathFind()
    {
        //UnityEngine.Debug.Log("Stop running.");
        sequence1.Kill();
        // slowing = true;
        // UnityEngine.Debug.Log("slowing is " + slowing);
        // UnityEngine.Debug.Log("Car should pause and slow down");
        sequence2 = DOTween.Sequence();
        Tween slowdownPath = transform.DOPath(slowdownPoints, slowdownPathTime, pathType).SetLookAt(0.01f);
        Tween continuePath = transform.DOPath(continuePoints, continuePathTime, pathType).SetLookAt(0.01f);

        sequence2.Append(slowdownPath);
        sequence2.AppendInterval(12f); // changed from 10f + slowdownPathTime
        sequence2.Append(continuePath);
    }
}
