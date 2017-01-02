using UnityEngine;
using System.Collections;

public class TextRotation : MonoBehaviour {

    void Update () {
        Vector3 directionToTarget = Camera.main.transform.position - gameObject.transform.position;
        transform.rotation = Quaternion.LookRotation(-directionToTarget);
        GetComponent<TextMesh>().text = directionToTarget.ToString();
    }
}
