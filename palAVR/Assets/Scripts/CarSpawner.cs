using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {
    
    public bool gesture;
    public bool sensor;
    public bool driver;

    public GameObject sensorCar;
    public GameObject sensorCar_fast;
    public GameObject gestureCar;
    public GameObject gestureCar_fast;
    public GameObject driverCar;
    public GameObject driverCar_fast;

    private GameObject car;

    private bool stopSpawning;
    public float spawnTime;
    public float spawnDelay;

    private int totalCars;
    private int numFastCars;
    private int numNormalCars;

    private void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    public void SpawnObject()
    {
        int random = Random.Range(0, 2);

        if (totalCars <= 4) // changed to 4 from 8 because there are 2 separate spawners with separate counters
        {
            // evens out distribution of normal/fast cars
            if (numFastCars == 2) {
                random = 1;
            } if (numNormalCars == 2) {
                random = 2;
            }

            // increments count of normal cars and fast cars
            if (random == 0) {
                numNormalCars++;
            } if (random == 1) {
                numFastCars++;
            }

            // assigns a car based on the type selected in the inspector and the random integer
            if (gesture) {
                GameObject[] gestureCars = {gestureCar, gestureCar_fast};
                car = gestureCars[random];
            } if (sensor) {
                GameObject[] sensorCars = {sensorCar, sensorCar_fast};
                car = sensorCars[random];
            } if (driver) {
                GameObject[] driverCars = {driverCar, driverCar_fast};
                car = driverCars[random];
            }

            Instantiate(car, transform.position, transform.rotation);
            totalCars++;
        } else {
            stopSpawning = true;
        } if (stopSpawning) {
            CancelInvoke("SpawnObject");
        }
    }
}
