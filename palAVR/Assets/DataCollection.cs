using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

namespace VRoad
{

    public class DataCollection : MonoBehaviour
    {
        [SerializeField]
        CarSpawner[] spawnScripts;

        [Tooltip ("Enable this to do test crosswalks and calculate mean required time")]
        [SerializeField]
        bool doMeanCalc;

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
        int totalCarsNorth;
        int numFastCarsNorth;
        int numNormalCarsNorth;

        CarSpawner carSpawnerSouth;
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
        private bool experimentStarted;

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
            UnityEngine.Debug.Log ("in experiment ");
            while (!Input.GetKey(KeyCode.Space))
            {
                yield return new WaitForSeconds (.167f);
             
                //Log here when you hit the space key
            }
           //Space bar buffer
            yield return new WaitForSeconds (.5f);

            if (doMeanCalc)
                yield return StartCoroutine (MeanCrossingCalculation ());

            else
                StartCarSpawning ();
        }

        private IEnumerator MeanCrossingCalculation ()
        {
            //code to have user walk over the street 

            UnityEngine.Debug.Log ("in mean calc ");

            float totalTime = 0f;

            /* This part of the code works as follows:
             * The user walks across the street, then the Experimenter presses space. Do that
             * *crosscounter* times. The software will then calculate a mean time that the user
             * needs for one crossing of the street */

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

            pedLog += "Mean time for street crossing : " + (totalTime/4).ToString() + "\n";

            StartCarSpawning ();

           
            yield return new WaitForEndOfFrame ();
        }

        private void StartCarSpawning ()
        {
            foreach (CarSpawner spwn in spawnScripts)
            {
                UnityEngine.Debug.Log ("spawning cars ");
                spwn.StartSpawningCars ();
            }
        }

        // Update is called once per frame
        void Update ()
        {
            if (StopGestureOccured ())
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
                fileWriter.WriteLog (pedLog);
                hasLogged = true;
                totalCars++;

                //End program when at least 4 cars have been destroyed at southern position
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

            if (experimentStarted)
            {
                if (totalCars % 2 == 1)
                {
                    CreateCarLog ("north");
                }
                else
                {
                    CreateCarLog ("south");
                }
            }

            // only occurs when a new car is spawned
            if (totalCars == increment)
            {
                stopwatch = new Stopwatch ();
                stopwatch.Start ();
                doOnce = false;

                pedLog += CarID ();
                increment++;
            }

            stoppedTime = 12;

            if (slowing && !doOnce)
            {
                timeUntilSlowing = timespanUntilSlowing.ToString ();
                pedLog += "Time before slowing: " + timeUntilSlowing + "\n";
                pedLog += "Time slowing: " + slowingTime + "\n";
                pedLog += "Time stopped: " + stoppedTime + "\n";
          
                doOnce = true;
                slowing = false;
            }


            //Check for collision
            //ToDo: use eventhandler
            if (experimentStarted)
            {
                carCollisionDetection = car.GetComponentInChildren<CarCollisionDetection> () as CarCollisionDetection;
                if (carCollisionDetection.collisionDetected)
                {
                    TimeSpan collisionTime = pedStop.Elapsed;
                    pedLog += "pedestrian and car collided at " + collisionTime.ToString () + "\n";
                }
            }
        }

        private void CreateCarLog ( string v )
        {
            if (v.Equals ("north"))
            {
                carOrigin = "north";
                carSpeed = carSpawnerNorth.carSpeed;
                carType = carSpawnerNorth.carKind.ToString ();

                car = carSpawnerNorth.instantiatedCar;

                CreatePathManager (carSpawnerNorth.carKind);

            }

            else if (v.Equals ("south"))
            {
                carOrigin = "south";
                carSpeed = carSpawnerSouth.carSpeed;
                carType = carSpawnerSouth.carKind.ToString ();

                car = carSpawnerSouth.instantiatedCar;
                CreatePathManager (carSpawnerSouth.carKind);
            }

            else
            {
                throw new NotImplementedException ();
            }
        }

        private void CreatePathManager ( CarKind carKind )
        {
            switch (carKind)
            {
                case (CarKind.GESTURE):
                    gesturePathManager = car.GetComponent<GesturePathManager> () as GesturePathManager;

                    slowingTime = gesturePathManager.slowdownPathTime;
                    if (gesturePathManager.slowing)
                    {
                        timespanUntilSlowing = stopwatch.Elapsed;
                        slowing = true;
                    }
                    break;
                case (CarKind.SENSOR):
                    sensorPathManager = car.GetComponent<SensorPathManager> () as SensorPathManager;

                    slowingTime = sensorPathManager.slowdownPathTime;
                    if (sensorPathManager.slowing)
                    {
                        timespanUntilSlowing = stopwatch.Elapsed;
                        slowing = true;
                    }
                    break;
                case (CarKind.DRIVER):
                    driverPathManager = car.GetComponent<DriverPathManager> () as DriverPathManager;

                    slowingTime = driverPathManager.slowdownPathTime;
                    if (driverPathManager.slowing)
                    {
                        timespanUntilSlowing = stopwatch.Elapsed;
                        slowing = true;
                        //UnityEngine.Debug.Log("slowing is " + slowing);
                    }

                    break;

                default: break;
            }
        }

        public void LogCarCollision()
        {
            TimeSpan collisionTime = pedStop.Elapsed;
            pedLog += "pedestrian and car collided at " + collisionTime.ToString () + "\n";
        }

        public void OnTriggerEnter ( Collider other )
        {
            // where the pedestrian starts
            if (other.tag == "crossing_a")
            {
                pedTime = pedStop.Elapsed;
                behavior = "Entered A at " + pedTime.ToString () + "\n";
                pedLog += behavior;

            }
            // other side of the street
            if (other.tag == "crossing_b")
            {
                // Debug.Log("in B");

                pedTime = pedStop.Elapsed;
                pedLog += behavior;
                behavior = "Entered B at " + pedTime.ToString () + "\n";

            }
        }

        public void OnTriggerExit ( Collider other )
        {
            // where the pedestrian starts
            if (other.tag == "crossing_a")
            {
                pedTime = pedStop.Elapsed;
                pedLog += behavior;
                behavior = "Exited A at " + pedTime.ToString () + "\n";            
            }
            // other side of the street
            if (other.tag == "crossing_b")
            {
                pedTime = pedStop.Elapsed;
                pedLog += behavior;
                behavior = "Exited B at " + pedTime.ToString () + "\n";
            }
        }

        bool StopGestureOccured ()
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
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public string CarID ()
        {
            string result = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
            result += "Car number: " + totalCars + "\n";
            result += "Car type: " + carType + "\n";
            result += "Car speed: " + carSpeed + "\n";
            result += "Car origin: " + carOrigin + "\n";

            return result;
        }

        public string PedLog ()
        {
            return pedLog;
        }
    }
}