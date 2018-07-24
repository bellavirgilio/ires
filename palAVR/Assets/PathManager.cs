using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class PathManager : MonoBehaviour {
    
    public PathType pathType = PathType.Linear;
    public Sequence sequence1;
    public Sequence sequence2;

    public Transform pedestrian; // needs to be SteamVR/[CameraRig], use PedPlaceholder for now
    private float pedX;
    private float pedZ;
    private float stopX;
    private float stopZ;
    private Vector3 stop;
    public float kmSpeed; // km/hr
    private float speed;

    private Vector3[] waypoints1;
    private Vector3[] waypoints2;
    private Vector3[] waypoints3;
    private Vector3[] waypoints4;

    // speed and distance variables
    private float initialPathDistance;
    private float slowdownPathDistance;
    private float continuePathDistance;
    private float nonstopPathDistance;
    private float initialPathTime;
    private float slowdownPathTime;
    private float continuePathTime;
    private float nonstopPathTime;

    // coordinates of car at gesture
    private float gestureX;
    private float gestureZ;
    private Vector3 gestureStop;

    public GameObject myo;
    private Pose _lastPose = Pose.Unknown;

    // keeps track of how many times stop gesture is made
    private int stopCount = 0;

    void Start()
    {
        speed = kmSpeed / 3.6f;

        // x and z coordinates of pedestrian established once
        pedX = pedestrian.position.x;
        pedZ = pedestrian.position.z;
        stopX = pedX + 3.5f;
        stopZ = pedZ + 7f;
        stop = new Vector3(stopX, 0.5f, stopZ);

        // calculates time (speed) for each section of the path
        initialPathDistance = 65.39942f;
        slowdownPathDistance = 20f; // estimate
        continuePathDistance = 178.3923f;
        nonstopPathDistance = 246.048f;
        initialPathTime = initialPathDistance / speed;
        slowdownPathTime = slowdownPathDistance / speed / 2; // needs to be slower
        continuePathTime = continuePathDistance / speed;
        nonstopPathTime = nonstopPathDistance / speed;

        // no gesture path (nonstop)
        waypoints4 = new[] { new Vector3(-7.061379f, 0.5f, 43.32791f), new Vector3(-4.602217f, 0.5f, 42.92312f), new Vector3(-2.219765f, 0.5f, 41.49f), new Vector3(-2f, 0.5f, 38.25f), new Vector3(-1.402732f, 0.5f, -9.364546f), new Vector3(-1.502464f, 0.5f, -38.09794f), new Vector3(-1.756076f, 0.5f, -41.22349f), new Vector3(-4.163716f, 0.5f, -42.69579f), new Vector3(-7.230853f, 0.5f, -42.92153f), new Vector3(-23.32813f, 0.5f, -42.41516f), new Vector3(-32.60156f, 0.5f, -40.63574f), new Vector3(-36.08203f, 0.5f, -39.28369f), new Vector3(-40.13672f, 0.5f, -37.08124f), new Vector3(-43.65625f, 0.5f, -34.58752f), new Vector3(-47.67969f, 0.5f, -30.06952f), new Vector3(-50.73828f, 0.5f, -25.1709f), new Vector3(-51.85156f, 0.5f, -19.78284f), new Vector3(-52.64453f, 0.5f, -13.20251f), new Vector3(-52.57813f, 0.5f, 13.17505f), new Vector3(-51.83594f, 0.5f, 20.64825f), new Vector3(-49.5f, 0.5f, 27.2572f), new Vector3(-44.98828f, 0.5f, 32.98553f), new Vector3(-39.89453f, 0.5f, 37.48236f), new Vector3(-33.46484f, 0.5f, 40.55725f), new Vector3(-26.43164f, 0.5f, 42.14038f), new Vector3(-17.91406f, 0.5f, 42.48737f) };

        Tween initial = transform.DOPath(waypoints4, nonstopPathTime, pathType).SetLookAt(0.01f); // nonstop tween
        sequence1 = DOTween.Sequence();
        sequence1.Append(initial);

        // how to loop while leaving option to use gesture control?
        // destroy the car and create a new one?
        // sequence.SetLoops(100, LoopType.Restart);
    }

    void Update() {
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
        bool stopGesture = false;

        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.FingersSpread)
            {
                Debug.Log("Stop gesture");
                stopGesture = true;
                gestureX = this.transform.position.x;
                gestureZ = this.transform.position.z;
                gestureStop = new Vector3(gestureX, 0.5f, gestureZ);
            }
        }

        // slowdown path
        waypoints2 = new[] {
            gestureStop,
            stop};

        // path 3 (continue after stop)
        waypoints3 = new[] {stop, new Vector3(-0.5429688f,0.5f,-38.03741f), new Vector3(-0.90625f,0.5f,-40.88074f), new Vector3(-3.402344f,0.5f,-42.45447f), new Vector3(-6.457031f,0.5f,-42.87268f), new Vector3(-23.32813f,0.5f,-42.41516f), new Vector3(-32.60156f,0.5f,-40.63574f), new Vector3(-36.08203f,0.5f,-39.28369f), new Vector3(-40.13672f,0.5f,-37.08124f), new Vector3(-43.65625f,0.5f,-34.58752f), new Vector3(-47.67969f,0.5f,-30.06952f), new Vector3(-50.73828f,0.5f,-25.1709f), new Vector3(-51.85156f,0.5f,-19.78284f), new Vector3(-52.64453f,0.5f,-13.20251f), new Vector3(-52.57813f,0.5f,13.17505f), new Vector3(-51.83594f,0.5f,20.64825f), new Vector3(-49.5f,0.5f,27.2572f), new Vector3(-44.98828f,0.5f,32.98553f), new Vector3(-39.89453f,0.5f,37.48236f), new Vector3(-33.46484f,0.5f,40.55725f), new Vector3(-26.43164f,0.5f,42.14038f), new Vector3(-17.91406f,0.5f,42.48737f) };
        
        // Gesture control that prevents user from calling stop more than once
        if (stopGesture && stopCount == 0) {
            stopCount = 1;
            Debug.Log("Stop called once. Stop count is 1.");
        }

        if (stopCount == 1) {
            Debug.Log("Stop running.");
            sequence1.Kill();
            Debug.Log("Car should pause and slow down");
            sequence2 = DOTween.Sequence();
            Tween slowdownPath = transform.DOPath(waypoints2, slowdownPathTime, pathType).SetLookAt(0.01f);
            Tween continuePath = transform.DOPath(waypoints3, continuePathTime, pathType).SetLookAt(0.01f);
            sequence2.Append(slowdownPath);
            sequence2.AppendInterval(10f);
            sequence2.Append(continuePath);
            stopCount += 1;
        }
    }
}