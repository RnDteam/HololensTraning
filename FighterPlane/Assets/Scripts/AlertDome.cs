using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertDome : MonoBehaviour {

    public Material ActiveAlertDome;
    public Material NonActiveAlertDome;

    private void Awake()
    {
        this.GetComponent<Renderer>().material = NonActiveAlertDome;
    }

    private void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.name.StartsWith("hercules"))
        {
            this.GetComponent<Renderer>().material = ActiveAlertDome;
            this.GetComponent<AudioSource>().Play();
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        this.GetComponent<Renderer>().material = NonActiveAlertDome;
        this.GetComponent<AudioSource>().Pause();
    }
}
