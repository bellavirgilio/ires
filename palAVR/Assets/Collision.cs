using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour {

	bool collision;

	public void OnTriggerEnter(Collider other)
    {
        // where the pedestrian starts
		if (other.tag.Equals("PedCamera"))
        {
			collision = true;
			Debug.Log("Pedestrian collided with car");
		}
	}
    
	public void OnTriggerExit(Collider other)
	{
		// where the pedestrian starts
        if (other.tag.Equals("PedCamera"))
        {
            collision = false;
			Debug.Log("Pedestrian and car no longer in contact");
        }
	}

    // test to see if this works, then access the public variable from the car's chassis and record the elapsed time of the collision
}
