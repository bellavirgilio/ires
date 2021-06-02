using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDestroy : MonoBehaviour {

    public int destroyCount;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "car_north" || other.gameObject.tag == "car_south")
            Destroy(other.gameObject);

        if (this.name.Equals("DestroyerSouth"))
        {
            destroyCount++;

        }

    }
}
