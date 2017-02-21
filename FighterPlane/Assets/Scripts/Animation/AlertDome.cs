using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertDome : MonoBehaviour
{

    public Material ActiveAlertDome;
    public Material NonActiveAlertDome;
    private bool showDome = false;
    private bool mapZoomed = false;

    private Vector3 defaultScale;


    private void Awake()
    {
        GetComponent<Renderer>().material = NonActiveAlertDome;
        // HideAlert();

        defaultScale = transform.localScale;

        //MapMovement.Instance.Moved += MapMoved;
        //MapMovement.Instance.ZoomChanged += MapZoomChanged;
    }

    //private void MapZoomChanged()
    //{
    //    transform.localScale = MapMovement.Instance.AbsoluteZoomRatio * defaultScale;
    //    Debug.Log(transform.localScale);
    //    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y * MapMovement.Instance.CurrentZoomRatio, transform.localPosition.z);
    //    mapZoomed = true; //TODO: fix zoom movemaped
    //    HideAlert();
    //}

    //private void MapMoved()
    //{
    //    var newPosition = transform.position + MapMovement.Instance.MovementVector;
    //    transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
    //    HideAlert();
    //}

    private void OnTriggerEnter(Collider myTrigger)
    {
        if (!mapZoomed && myTrigger.gameObject.CompareTag("Plane"))
        {
            GetComponent<Renderer>().material = ActiveAlertDome;
            GetComponent<AudioSource>().Play();
            GetComponent<Renderer>().enabled = true; ;
        }
    }

    private void OnTriggerExit(Collider myTrigger)
    {
        if (myTrigger.gameObject.CompareTag("Plane"))
        {
            GetComponent<Renderer>().material = NonActiveAlertDome;
            GetComponent<AudioSource>().Pause();
            if (!showDome) HideAlert();
        }
    }

    public void ShowAlert()
    {
        GetComponent<Renderer>().enabled = true;
        showDome = true;
    }

    public void HideAlert()
    {
        //GetComponent<Renderer>().enabled = false;
        //showDome = false;
    }
}