using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationSync : MonoBehaviour {

    public double Latitude, Longitude;
	
	void Update () {
        var coords = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(transform.TransformPoint(transform.position));
        Longitude = coords.x;
        Latitude = coords.y;
    }
}
