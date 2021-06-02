using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    Animator _anim;
    // Use this for initialization
    void Start () {
        _anim = GetComponent<Animator> ();
	}

    private void OnTriggerEnter ( Collider other )
    {
        if (other.tag.Equals ("Player"))
        {
            _anim.SetBool ("open", true);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
