using UnityEngine;
using System.Collections;

public class TextRotation : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = new Quaternion(0, Camera.main.transform.rotation.y, 0, 1);
	}
}
