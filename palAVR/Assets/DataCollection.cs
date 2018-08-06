﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class DataCollection : MonoBehaviour
{

    // experiment stopwatch
    private Stopwatch experimentStop = new Stopwatch();
    private TimeSpan experimentTime = new TimeSpan();

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
    private bool slowing;

    // car stopwatch
    private Stopwatch stopwatch;
    float slowingTime;
    float stoppedTime;
    TimeSpan timespanUntilSlowing;
    String timeUntilSlowing;
    bool doOnce;

    // Use this for initialization
    void Start()
    {
        carSpawnerNorth = spawner_north.GetComponent<CarSpawner>();
        carSpawnerSouth = spawner_south.GetComponent<CarSpawner>();
        increment = totalCars + 1;
        pedStop = new Stopwatch();
        pedStop.Start();
        experimentStop.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (totalCars == 8) {
            experimentTime = experimentStop.Elapsed;
            pedLog += "Experiment total time: " + experimentTime.ToString() + "\n";
            totalCars++;
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

        if (carSpawnerNorth.carSpeed.Equals("fast") || carSpawnerSouth.carSpeed.Equals("fast"))
        {
            carSpeed = "fast";
        }
        if (carSpawnerNorth.carSpeed.Equals("normal") || carSpawnerSouth.carSpeed.Equals("normal"))
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

        if (carOrigin.Equals("north"))
        {
            car = carSpawnerNorth.instantiatedCar;
            if (carType.Equals("gesture"))
            {
                gesturePathManager = car.GetComponent<GesturePathManager>() as GesturePathManager;
            }
            if (carType.Equals("sensor"))
            {
                sensorPathManager = car.GetComponent<SensorPathManager>() as SensorPathManager;
            }
            if (carType.Equals("driver"))
            {
                driverPathManager = car.GetComponent<DriverPathManager>() as DriverPathManager;
            }
        }
        if (carOrigin.Equals("south"))
        {
            car = carSpawnerSouth.instantiatedCar;
            if (carType.Equals("gesture"))
            {
                gesturePathManager = car.GetComponent<GesturePathManager>() as GesturePathManager;
            }
            if (carType.Equals("sensor"))
            {
                sensorPathManager = car.GetComponent<SensorPathManager>() as SensorPathManager;
            }
            if (carType.Equals("driver"))
            {
                driverPathManager = car.GetComponent<DriverPathManager>() as DriverPathManager;
            }
        }



        // only occurs when a new car is spawned
        if (totalCars == increment)
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            // UnityEngine.Debug.Log("new stopwatch created and started");
            doOnce = false;

            pedLog += CarID();
            // UnityEngine.Debug.Log(CarID());
            increment++;
        }

        if (carType.Equals("gesture"))
        {
            slowingTime = gesturePathManager.slowdownPathTime;
            if (gesturePathManager.slowing)
            {
                timespanUntilSlowing = stopwatch.Elapsed;
                slowing = true;
            }
        }
        if (carType.Equals("sensor"))
        {
            slowingTime = sensorPathManager.slowdownPathTime;
            if (sensorPathManager.slowing)
            {
                timespanUntilSlowing = stopwatch.Elapsed;
                slowing = true;
            }
        }
        if (carType.Equals("driver"))
        {
            slowingTime = driverPathManager.slowdownPathTime;
            if (driverPathManager.slowing)
            {
                timespanUntilSlowing = stopwatch.Elapsed;
                slowing = true;
                //UnityEngine.Debug.Log("slowing is " + slowing);
            }
        }

        stoppedTime = slowingTime + 10f;

        if (slowing && !doOnce)
        {
            timeUntilSlowing = timespanUntilSlowing.ToString();
            pedLog += "Time before slowing: " + timeUntilSlowing + "\n";
            pedLog += "Time slowing: " + slowingTime + "\n";
            pedLog += "Time stopped: " + stoppedTime + "\n";
            //UnityEngine.Debug.Log("Time before slowing: " + timeUntilSlowing);
            //UnityEngine.Debug.Log("Time slowing: " + slowingTime);
            //UnityEngine.Debug.Log("Time stopped: " + stoppedTime);
            doOnce = true;
            slowing = false;
        }

        carCollisionDetection = car.GetComponentInChildren<CarCollisionDetection>() as CarCollisionDetection;
        if (carCollisionDetection.collisionDetected) {
            pedestrianCollision = true;
        }

        if (pedestrianCollision) {
            TimeSpan collisionTime = pedStop.Elapsed;
            pedLog += "pedestrian and car collided at " + collisionTime.ToString() + "\n";
            //UnityEngine.Debug.Log("pedestrian and car collided at " + collisionTime.ToString());
            pedestrianCollision = false;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        // where the pedestrian starts
        if (other.tag == "crossing_a")
        {
            pedTime = pedStop.Elapsed;
            behavior = "Entered A at " + pedTime.ToString() + "\n";
            pedLog += behavior;
            //UnityEngine.Debug.Log("pedestrian behavior: " + behavior);
            // Debug.Log("in A");
        }
        // other side of the street
        if (other.tag == "crossing_b")
        {
            // Debug.Log("in B");

            pedTime = pedStop.Elapsed;
            pedLog += behavior;
            behavior = "Entered B at " + pedTime.ToString() + "\n";
            // UnityEngine.Debug.Log("pedestrian behavior: " + behavior);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // where the pedestrian starts
        if (other.tag == "crossing_a")
        {
            // Debug.Log("leaving A");

            pedTime = pedStop.Elapsed;
            pedLog += behavior;
            behavior = "Exited A at " + pedTime.ToString() + "\n";
            // UnityEngine.Debug.Log("pedestrian behavior: " + behavior);
        }
        // other side of the street
        if (other.tag == "crossing_b")
        {
            // Debug.Log("leaving B");

            pedTime = pedStop.Elapsed;
            pedLog += behavior;
            behavior = "Exited B at " + pedTime.ToString() + "\n";
            // UnityEngine.Debug.Log("pedestrian behavior: " + behavior);
        }
    }

    public string CarID()
    {
        string result = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
        result += "Car number: " + totalCars + "\n";
        result += "Car type: " + carType + "\n";
        result += "Car speed: " + carSpeed + "\n";
        result += "Car origin: " + carOrigin + "\n";

        return result;
    }

    public string PedLog() {
        return pedLog;
    }
}
