using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLogger : MonoBehaviour {

    void OnCollisionEnter (Collision collision)
    {
        Debug.Log("Collider entered");
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision stay");
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collider exited");
    }
}
