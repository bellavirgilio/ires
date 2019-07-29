using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRoad
{

    public class CarCollisionDetection : MonoBehaviour
    {

        public bool collisionDetected;

        public void OnTriggerEnter ( Collider other )
        {
            // where the pedestrian starts
            if (other.tag.Equals ("PedCamera"))
            {
                collisionDetected = true;
                //Debug.Log("Pedestrian collided with car");
            }
        }

        public void OnTriggerExit ( Collider other )
        {
            // where the pedestrian starts
            if (other.tag.Equals ("PedCamera"))
            {
                collisionDetected = false;
                //Debug.Log("Pedestrian and car no longer in contact");
            }
        }

        // test to see if this works, then access the public variable from the car's chassis and record the elapsed time of the collision
    }
}
