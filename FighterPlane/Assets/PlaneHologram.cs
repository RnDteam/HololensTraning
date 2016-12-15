using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class PlaneHologram : MonoBehaviour {

    public bool isPlacing;
    //private Vector3 planeOffsetFromCamera;

    // Use this for initialization
    void Start () {
        GestureManager.Instance.OverrideFocusedObject = gameObject;


        //planeOffsetFromCamera = transform.position - Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the object is being placed using the placeable script
        //if (gameObject.GetComponent<Placeable>().IsPlacing)
        //{
        //    transform.position = Vector3.Lerp(transform.position, ProposeTransformPosition(), 0.2f);
        //    transform.rotation = Camera.main.transform.rotation;
        //}
    }

    //Vector3 ProposeTransformPosition()
    //{
    //    // Put the model 2m in front of the user.
    //    // Vector3 retval = Camera.main.transform.position + Camera.main.transform.forward * 2;

    //    Vector3 retval = Camera.main.transform.position + Camera.main.transform.rotation * planeOffsetFromCamera;

    //    return retval;
    //}


}
