using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class DataCollection : MonoBehaviour
{

    private bool inA;
    private bool inB;

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

    int totalCars;
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
    }

    // Update is called once per frame
    void Update()
    {
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

        // occurs only when the car spawns –– restarting increment to 1 not working
        if (totalCars == increment) {
            increment++;
            UnityEngine.Debug.Log(CarID());

            stopwatch = new Stopwatch();
            stopwatch.Start();
            UnityEngine.Debug.Log("new stopwatch created and started");

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
                UnityEngine.Debug.Log("slowing is " + slowing);
            }
        }

        stoppedTime = slowingTime + 10f;

        if (slowing && !doOnce)
        {
            timeUntilSlowing = timespanUntilSlowing.ToString();
            UnityEngine.Debug.Log("Time before slowing: " + timeUntilSlowing);
            UnityEngine.Debug.Log("Time slowing: " + slowingTime);
            UnityEngine.Debug.Log("Time stopped: " + stoppedTime);
            doOnce = true;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        // where the pedestrian starts
        if (other.tag == "crossing_a")
        {
            inA = true;
            // Debug.Log("in A");
        }
        // other side of the street
        if (other.tag == "crossing_b")
        {
            inB = true;
            // Debug.Log("in B");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // where the pedestrian starts
        if (other.tag == "crossing_a")
        {
            inA = false;
            // Debug.Log("leaving A");
        }
        // other side of the street
        if (other.tag == "crossing_b")
        {
            inB = false;
            // Debug.Log("leaving B");
        }
    }

    public string CarID() {
        string result = "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
        result += "Car number: " + totalCars + "\n";
        result += "Car type: " + carType + "\n";
        result += "Car speed: " + carSpeed + "\n";
        result += "Car origin: " + carOrigin + "\n";

        return result;
    }
}
