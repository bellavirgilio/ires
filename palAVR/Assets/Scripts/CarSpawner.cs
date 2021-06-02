using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRoad;

public class CarSpawner : MonoBehaviour
{

    public CarKind carKind;

    [SerializeField] GameObject sensorCar;
    [SerializeField] GameObject sensorCar_fast;
    [SerializeField] GameObject gestureCar;
    [SerializeField] GameObject gestureCar_fast;
    [SerializeField] GameObject driverCar;
    [SerializeField] GameObject driverCar_fast;

    private GameObject car;
    private bool stopSpawning;

    [Tooltip ("Settings for spawn frequency")]
    public float spawnTime;
    public float spawnDelay;

    public int totalCars;
    public int numFastCars;
    public int numNormalCars;

    public string carSpeed;

    public string origin;

    // spawned car
    public GameObject instantiatedCar;

    private void Start ()
    {
        StartCoroutine ("WaitForPausePress");
    }

    IEnumerator WaitForPausePress ()
    {

        while (!Input.GetKeyDown (KeyCode.P))
        {
            yield return new WaitForSeconds (1 / 60);
        }

        EditorApplication.isPaused = true;
    }
    //Now called from data collection after the average crossing time calc is done
    public void StartSpawningCars ()
    {
        InvokeRepeating ("SpawnObject", spawnTime, spawnDelay);
    }

    public void SpawnObject ()
    {
        int random = Random.Range (0, 2);

        origin = this.name;

        if (totalCars < 4) // changed to 4 from 8 because there are 2 separate spawners with separate counters
        {
            // evens out distribution of normal/fast cars
            if (numFastCars == 2)
            {
                random = 0;
            }
            if (numNormalCars == 2)
            {
                random = 1;
            }

            // increments count of normal cars and fast cars
            if (random == 0)
            {
                carSpeed = "normal";
                numNormalCars++;
            }
            if (random == 1)
            {
                numFastCars++;
                carSpeed = "fast";
            }

            // assigns a car based on the type selected in the inspector and the random integer
            switch (carKind)
            {
                case (CarKind.GESTURE):
                    GameObject[] gestureCars = { gestureCar, gestureCar_fast };
                    car = gestureCars[random];
                    break;
                case (CarKind.SENSOR):
                    GameObject[] sensorCars = { sensorCar, sensorCar_fast };
                    car = sensorCars[random];
                    break;
                case (CarKind.DRIVER):
                    GameObject[] driverCars = { driverCar, driverCar_fast };
                    car = driverCars[random];
                    break;
                default: break;
            }

            instantiatedCar = Instantiate (car, transform.position, transform.rotation) as GameObject;
            totalCars++;
        }
        else
        {
            stopSpawning = true;
        }

        if (stopSpawning)
        {
            CancelInvoke ("SpawnObject");
        }
    }
}
