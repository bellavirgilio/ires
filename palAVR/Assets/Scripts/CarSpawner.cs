using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour {

    public bool gesture;
    public GameObject sensorCar_fast;
    public GameObject sensorCar;
    public GameObject gestureCar_fast;
    public GameObject gestureCar;

    private bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    private void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    public void SpawnObject()
    {
        int random = Random.Range(1, 3);
        int totalCars = 0;
        int numFastCars = 0;
        int numNormalCars = 0;

        if (totalCars < 8)
        {
            if (numFastCars == 4) {
                random = 1;
            }
            if (numNormalCars == 4) {
                random = 2;
            }
            if (gesture) {
                if (random == 1) // generates a normal gesture car
                {
                    Instantiate(gestureCar, transform.position, transform.rotation);
                    numNormalCars++;
                }
                if (random == 2) // generates a fast gesture car
                {
                    Instantiate(gestureCar_fast, transform.position, transform.rotation);
                    numFastCars++;
                }
            } else {
                if (random == 1) // generates a normal sensor car
                {
                    Instantiate(sensorCar, transform.position, transform.rotation);
                    numNormalCars++;
                }
                if (random == 2) // generates a fast sensor car
                {
                    Instantiate(sensorCar_fast, transform.position, transform.rotation);
                    numFastCars++;
                }
            }
        } else {
            stopSpawning = true;
        }

        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }
    }
}
