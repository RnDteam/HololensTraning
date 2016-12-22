using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempDomeScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        Transform dome = this.GetComponent<Transform>();
        TextMesh domeText = this.GetComponentInChildren<TextMesh>();
        print("Dome Position: "+dome.position.ToString());
        print("Dome Scale: " + dome.localScale.ToString());

        domeText.text = "Dome Position: " + (dome.position*10).ToString()+ "\nScalePosition: " + dome.localScale.ToString();
    }
}
