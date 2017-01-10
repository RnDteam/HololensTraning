using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertDome : MonoBehaviour
{

    public Material ActiveAlertDome;
    public Material NonActiveAlertDome;
    private bool showDome = false;

    private Vector3 defaultScale;


    private void Awake()
    {
        GetComponent<Renderer>().material = NonActiveAlertDome;
        HideAlert();

        defaultScale = transform.localScale;

        MapMovement.Instance.Moved += MapMoved;
        MapMovement.Instance.ZoomChanged += MapZoomChanged;
    }

    private void MapZoomChanged()
    {
        transform.localScale = MapMovement.Instance.AbsoluteZoomRatio * defaultScale;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y * MapMovement.Instance.CurrentZoomRatio, transform.localPosition.z);
    }

    private void MapMoved()
    {
        var newPosition = transform.position + MapMovement.Instance.MovementVector;
        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
    }

    private void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.name.StartsWith("Hercules"))
        {
            GetComponent<Renderer>().material = ActiveAlertDome;
            GetComponent<AudioSource>().Play();
            GetComponent<Renderer>().enabled = true; ;
        }
    }

    private void OnTriggerExit(Collider myTrigger)
    {
        if (myTrigger.gameObject.name.StartsWith("Hercules"))
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
        GetComponent<Renderer>().enabled = false;
        showDome = false;
    }
}