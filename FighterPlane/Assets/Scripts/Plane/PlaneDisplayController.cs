using UnityEngine;
using System.Collections;
using System;

public abstract class PlaneDisplayController : MonoBehaviour
{

    public bool IsInfoShown
    {
        get;
        private set;
    }

    public bool IsGasAlertActive
    {
        get;
        private set;
    }

    public GameObject planeInfo;
    public GameObject lackOfGasAlert;
    public GameObject planeCamera;
    public GameObject pilotCamera;
    public float gasAmount = 100;

    private PhysicsParameters pParams;

    public void Start()
    {
        pParams = new PhysicsParameters(transform);
        IsGasAlertActive = false;
    }

    void Update()
    {
        // Update Gas Amount
        HandleGasAmount();

        // Calculate inforamtion only if text is shown
        if (IsInfoShown)
        {
            // Calculate physics information
            pParams.UpdatePhysics(transform);

            DisplayUpdatedInfo();
        }
    }

    #region Plane's Gas
    protected void HandleGasAmount()
    {
        gasAmount = gasAmount > 0 ? gasAmount - Time.deltaTime : 0;

        // If there is a lack of gas display alert
        if (gasAmount <= GlobalManager.GasThreshold)
        {
            // todo will be changed if found a better way to avoid boolea parameter
            if (!IsGasAlertActive)
            {
                IsGasAlertActive = true;
                lackOfGasAlert.GetComponent<MeshRenderer>().enabled = true;
                lackOfGasAlert.GetComponent<AudioSource>().Play();
            }
        }
    }
    #endregion

    #region Selecting Plane
    public abstract void SelectPlane();
    public abstract void DeselectPlane();
    #endregion

    #region Plane Details
    private void DisplayUpdatedInfo()
    {
        planeInfo.GetComponent<TextMesh>().text = this.name + "\n" + pParams.ToString()
                                                            + "\n" + "Gas Amount(Liters): " + this.gasAmount.ToString("000.0");
    }

    public void HidePlaneInfo()
    {
        planeInfo.SetActive(false);
        IsInfoShown = false;
    }

    public void ShowPlaneInfo()
    {
        planeInfo.SetActive(true);
        IsInfoShown = true;
    }
    #endregion
}
