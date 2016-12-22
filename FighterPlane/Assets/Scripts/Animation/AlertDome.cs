using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertDome : MonoBehaviour
{

    public Material ActiveAlertDome;
    public Material NonActiveAlertDome;
    public Material HideAlertDome;
    private bool hideDome = true;
    private bool isAlertActive = false;


    private void Awake()
    {
        this.GetComponent<Renderer>().material = HideAlertDome;
    }

    private void OnTriggerEnter(Collider myTrigger)
    {
        if (myTrigger.gameObject.name.StartsWith("Hercules"))
        {
            this.GetComponent<Renderer>().material = ActiveAlertDome;
            this.GetComponent<AudioSource>().Play();
            isAlertActive = true;
        }
    }

    private void OnTriggerExit(Collider myTrigger)
    {
        if (myTrigger.gameObject.name.StartsWith("Hercules"))
        {
            this.GetComponent<Renderer>().material = (hideDome ? HideAlertDome : NonActiveAlertDome);
            this.GetComponent<AudioSource>().Pause();
            isAlertActive = false;
        }
    }

    public void ShowAlert()
    {
        this.GetComponent<Renderer>().material = (isAlertActive ? ActiveAlertDome : NonActiveAlertDome);
        hideDome = false;
    }

    public void HideAlert()
    {
        this.GetComponent<Renderer>().material = HideAlertDome;
        hideDome = true;

    }
}