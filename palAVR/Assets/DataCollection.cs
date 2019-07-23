using System.Collections;
using UnityEngine;
using System.Diagnostics;
using System;

using Pose = Thalmic.Myo.Pose;

//Ensure a DataFileWriter is attached
[RequireComponent (typeof (DataFileWriter))]


public class DataCollection : MonoBehaviour
{
    [SerializeField]
    CarSpawner[] spawnScripts;
    // experiment stopwatch
    private Stopwatch experimentStop = new Stopwatch ();
    private TimeSpan experimentTime = new TimeSpan ();

    // pedestrian stopwatch
    private Stopwatch pedStop;
    private TimeSpan pedTime;
    String behavior = "";
    private String pedLog; // a log of pedestrian behavior
    CarCollisionDetection carCollisionDetection;
    private bool pedestrianCollision;

    // car info
    public GameObject spawner_north;
    public GameObject spawner_south;
    public GameObject destroyer;
    CarSpawner carSpawnerNorth;
    CarSpawner carSpawnerSouth;
    int totalCarsNorth;
    int numFastCarsNorth;
    int numNormalCarsNorth;

    int totalCarsSouth;
    int numFastCarsSouth;
    int numNormalCarsSouth;

    public int totalCars;
    int increment;
    int totalFastCars;
    int totalNormalCars;

    string carType;
    string carSpeed;
    string carOrigin;

    private GameObject car;
    DriverPathManager driverPathManager;
    GesturePathManager gesturePathManager;
    SensorPathManager sensorPathManager;
    CarDestroy carDestroy;
    private bool slowing;

    // car stopwatch
    private Stopwatch stopwatch;
    float slowingTime;
    float stoppedTime;
    TimeSpan timespanUntilSlowing;
    String timeUntilSlowing;
    bool doOnce;

    // myo
    private GameObject myo;
    private Pose _lastPose = Pose.Unknown;

    DataFileWriter fileWriter;
    bool hasLogged = false;

    bool startSpawning = false;
    // Use this for initialization
    void Start ()
    {
        myo = GameObject.Find ("Myo");

        carSpawnerNorth = spawner_north.GetComponent<CarSpawner> ();
        carSpawnerSouth = spawner_south.GetComponent<CarSpawner> ();
        carDestroy = destroyer.GetComponent<CarDestroy> ();
        increment = totalCars + 1;
        pedStop = new Stopwatch ();
        pedStop.Start ();
        experimentStop.Start ();

        fileWriter = GetComponent<DataFileWriter> ();

        StartCoroutine (Experiment ());
    }
    private IEnumerator Experiment ()
    {
        UnityEngine.Debug.Log ("in experiment");
        while (!Input.GetKey (KeyCode.Space))
        {
            yield return new WaitForEndOfFrame ();
            //TODO: Log here when you hit the space key
        }
        UnityEngine.Debug.Log ("hit space key");

        //Space bar buffer
        yield return new WaitForSeconds (.5f);

        yield return StartCoroutine (MeanCrossingCalculation ());

    }
    private IEnumerator MeanCrossingCalculation ()
    {
        //code to have user walk over the street 

        UnityEngine.Debug.Log ("in mean calc ");

        float totalTime = 0f;
        float averageTime;

        for (int crossCounter = 0; crossCounter < 2; crossCounter++) // press space when they're back at the origin
        {
            while (!Input.GetKey (KeyCode.Space))
            {
                //code to have user walk over the street  4x
                //Space bar buffer
                yield return new WaitForSeconds (.5f);
            }

            UnityEngine.Debug.Log (crossCounter.ToString ());
            pedLog += "Space bar occurred at " + pedStop.Elapsed.ToString () + "\n";

            yield return new WaitForSeconds (.5f);
            UnityEngine.Debug.Log ("Finished one crossing");
        }

        UnityEngine.Debug.Log ("Initial crossing logging complete");

        averageTime = totalTime / 4;
        //TODO: Log the average time in the logfile 

        //get access to spawner scripts to be able to tell them to spawn stuff
        foreach (CarSpawner spwn in spawnScripts)
        {
            UnityEngine.Debug.Log ("spawning cars");
            spwn.StartSpawningCars ();
        }

        yield return new WaitForEndOfFrame ();
    }
    // Update is called once per frame
    void Update ()
    {
        if (CheckStopGesture ())
        {
            pedLog += "Stop gesture occurred at " + pedStop.Elapsed.ToString () + "\n";
        }

        if (carDestroy.destroyCount == 4 && !hasLogged)
        {
            experimentTime = experimentStop.Elapsed;
            //    pedLog += "Experiment total time: " + experimentTime.ToString() + "\n";
            float time = Time.realtimeSinceStartup;
            string minutes = Mathf.Floor (time / 60).ToString ("00");
            string seconds = (time % 60).ToString ("00");
            UnityEngine.Debug.Log (minutes + ":" + seconds);
            pedLog += "Experiment total time:" + minutes + ":" + seconds;
            UnityEngine.Debug.Log (pedLog);
            fileWriter.WriteLog (pedLog);
            hasLogged = true;
            totalCars++;

            if (GameObject.Find ("DestroyerSouth").GetComponent<CarDestroy> ().destroyCount >= 4)
            {
                UnityEditor.EditorApplication.isPlaying = false;

            }
        }

        totalCarsNorth = carSpawnerNorth.totalCars;
        numFastCarsNorth = carSpawnerNorth.numFastCars;
        numNormalCarsNorth = carSpawnerNorth.numNormalCars;
        totalCarsSouth = carSpawnerSouth.totalCars;
        numFastCarsSouth = carSpawnerSouth.numFastCars;
        numNormalCarsSouth = carSpawnerSouth.numNormalCars;

        totalCars = totalCarsNorth + totalCarsSouth;
        totalFastCars = numFastCarsNorth + numFastCarsSouth;
        totalNormalCars = numNormalCarsNorth + numNormalCarsSouth;

        if (totalCars % 2 == 1)
        {
            carOrigin = "north";
        }
        else
        {
            carOrigin = "south";
        }

        if (carSpawnerNorth.carSpeed.Equals ("fast") || carSpawnerSouth.carSpeed.Equals ("fast"))
        {
            carSpeed = "fast";
        }
        if (carSpawnerNorth.carSpeed.Equals ("normal") || carSpawnerSouth.carSpeed.Equals ("normal"))
        {
            carSpeed = "normal";
        }

        if (carSpawnerNorth.gesture || carSpawnerSouth.gesture)
        {
            carType = "gesture";
        }
        if (carSpawnerNorth.sensor || carSpawnerSouth.sensor)
        {
            carType = "sensor";
        }
        if (carSpawnerNorth.driver || carSpawnerSouth.driver)
        {
            carType = "driver";
        }

        if (carOrigin.Equals ("north"))
        {
            car = carSpawnerNorth.instantiatedCar;
        }
        else
        {
            car = carSpawnerSouth.instantiatedCar;
        }

        switch (carType)
        {
            case ("gesture"):
                gesturePathManager = car.GetComponent<GesturePathManager> () as GesturePathManager;
                break;

            case ("sensor"):
                sensorPathManager = car.GetComponent<SensorPathManager> () as SensorPathManager;
                break;

            case ("driver"):
                driverPathManager = car.GetComponent<DriverPathManager> () as DriverPathManager;
                break;

            default: break;

        }

        // only occurs when a new car is spawned
        if (totalCars == increment)
        {
            stopwatch = new Stopwatch ();
            stopwatch.Start ();
            // UnityEngine.Debug.Log("new stopwatch created and started");
            doOnce = false;

            pedLog += GetCarID ();
            // UnityEngine.Debug.Log(CarID());
            increment++;
        }

        if (carType.Equals ("gesture"))
        {
            slowingTime = gesturePathManager.slowdownPathTime;
            if (gesturePathManager.slowing)
            {
                timespanUntilSlowing = stopwatch.Elapsed;
                slowing = true;
            }
        }
        else if (carType.Equals ("sensor"))
        {
            slowingTime = sensorPathManager.slowdownPathTime;
            if (sensorPathManager.slowing)
            {
                timespanUntilSlowing = stopwatch.Elapsed;
                slowing = true;
            }
        }
        else if (carType.Equals ("driver"))
        {
            slowingTime = driverPathManager.slowdownPathTime;
            if (driverPathManager.slowing)
            {
                timespanUntilSlowing = stopwatch.Elapsed;
                slowing = true;
                //UnityEngine.Debug.Log("slowing is " + slowing);
            }
        }

        stoppedTime = 12;

        if (slowing && !doOnce)
        {
            timeUntilSlowing = timespanUntilSlowing.ToString ();
            pedLog += "Time before slowing: " + timeUntilSlowing + "\n";
            pedLog += "Time slowing: " + slowingTime + "\n";
            pedLog += "Time stopped: " + stoppedTime + "\n";
            //UnityEngine.Debug.Log("Time before slowing: " + timeUntilSlowing);
            //UnityEngine.Debug.Log("Time slowing: " + slowingTime);
            //UnityEngine.Debug.Log("Time stopped: " + stoppedTime);
            doOnce = true;
            slowing = false;
        }

        carCollisionDetection = car.GetComponentInChildren<CarCollisionDetection> () as CarCollisionDetection;
        if (carCollisionDetection.collisionDetected)
        {
            TimeSpan collisionTime = pedStop.Elapsed;
            pedLog += "pedestrian and car collided at " + collisionTime.ToString () + "\n";
            //UnityEngine.Debug.Log("pedestrian and car collided at " + collisionTime.ToString());
        }

    }

    public void OnTriggerEnter ( Collider other )
    {
        // where the pedestrian starts
        if (other.tag == "crossing_a")
        {
            pedTime = pedStop.Elapsed;
            behavior = "Entered A at " + pedTime.ToString () + "\n";
            pedLog += behavior;
            //UnityEngine.Debug.Log("pedestrian behavior: " + behavior);
            UnityEngine.Debug.Log ("in A");
        }
        // other side of the street
        if (other.tag == "crossing_b")
        {
            // Debug.Log("in B");

            pedTime = pedStop.Elapsed;
            pedLog += behavior;
            behavior = "Entered B at " + pedTime.ToString () + "\n";
            // UnityEngine.Debug.Log("pedestrian behavior: " + behavior);
            UnityEngine.Debug.Log ("in B");
        }
    }

    public void OnTriggerExit ( Collider other )
    {
        // where the pedestrian starts
        if (other.tag == "crossing_a")
        {
            // Debug.Log("leaving A");

            pedTime = pedStop.Elapsed;
            pedLog += behavior;
            behavior = "Exited A at " + pedTime.ToString () + "\n";
            // UnityEngine.Debug.Log("pedestrian behavior: " + behavior);
        }
        // other side of the street
        if (other.tag == "crossing_b")
        {
            // Debug.Log("leaving B");

            pedTime = pedStop.Elapsed;
            pedLog += behavior;
            behavior = "Exited B at " + pedTime.ToString () + "\n";
            // UnityEngine.Debug.Log("pedestrian behavior: " + behavior);
        }
    }

    bool CheckStopGesture ()
    {
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.FingersSpread)
            {
                UnityEngine.Debug.Log ("Stop gesture");

                return true;
            }
        }
        return false;
    }

    public string GetCarID ()
    {
        string result = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
        result += "Car number: " + totalCars + "\n";
        result += "Car type: " + carType + "\n";
        result += "Car speed: " + carSpeed + "\n";
        result += "Car origin: " + carOrigin + "\n";

        return result;
    }

    public string GetPedLog ()
    {
        return pedLog;
    }
}
