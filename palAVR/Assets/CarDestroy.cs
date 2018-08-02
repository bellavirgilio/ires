using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDestroy : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "car_north" || other.gameObject.tag == "car_south")
            Destroy(other.gameObject);
    }
}
