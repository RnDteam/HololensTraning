using UnityEngine;
using System.Collections;

public class TextRotation : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        Vector3 directionToTarget = Camera.main.transform.position - gameObject.transform.position;
        transform.rotation = Quaternion.LookRotation(-directionToTarget);
    }
}
