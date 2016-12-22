using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertDome : MonoBehaviour
{

    public Material ActiveAlertDome;
    public Material NonActiveAlertDome;
    private bool showDome = false;


    private void Awake()
    {
        this.GetComponent<Renderer>().material = NonActiveAlertDome;
        this.GetComponent<Renderer>().enabled = showDome;
    }

    private void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.name.StartsWith("Hercules"))
        {
            this.GetComponent<Renderer>().material = ActiveAlertDome;
            this.GetComponent<AudioSource>().Play();
            ShowAlert();
        }
    }

    private void OnTriggerExit(Collider myTrigger)
    {
        if (myTrigger.gameObject.name.StartsWith("Hercules"))
        {
            this.GetComponent<Renderer>().material = NonActiveAlertDome;
            this.GetComponent<AudioSource>().Pause();
            if (!showDome) HideAlert();
        }
    }

    public void ShowAlert()
    {
        this.GetComponent<Renderer>().enabled = true;
        showDome = true;
    }

    public void HideAlert()
    {
        this.GetComponent<Renderer>().enabled = false;
        showDome = false;
    }
}