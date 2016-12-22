using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCircleForce : MonoBehaviour {

    Rigidbody plane;
    public static bool enabled = false;

	// Use this for initialization
	void Start () {
        plane = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (enabled)
        {
            if(plane.velocity.magnitude == 0)
            {
                plane.velocity = Vector3.Cross(plane.position, Vector3.up);
            }
            plane.AddForce(-plane.position, ForceMode.Acceleration);
        }
	}
}
