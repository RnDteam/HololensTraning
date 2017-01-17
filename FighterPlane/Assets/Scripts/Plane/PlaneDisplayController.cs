﻿using UnityEngine;
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

    private Color selectedColor;
    public Color defaultColor;
    public GameObject planeInfo;
    public GameObject lackOfGasAlert;
    public GameObject planeCamera;
    public GameObject pilotCamera;
    public float gasAmount = 100;

    public Vector2 coords;
    public float localHeight;
    private Vector3 defaultScale;



    private PhysicsParameters pParams;

    public bool IsVisible;

    public void Start()
    {
        selectedColor = Color.blue;
        ConvertColors(defaultColor);

        defaultScale = transform.localScale;
        MapMovement.Instance.ZoomChanged += ChangeZoom;
        MapMovement.Instance.Moved += ChangePosition;

        pParams = new PhysicsParameters(transform);
        IsGasAlertActive = false;

        if (PlaneManager.Instance.PlaneVisibilityWhenOffMap || MapCommands.Instance.Contains(coords))
        {
            IsVisible = true;
        }
        else
        {
            SetVisibility(false);
        }
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

        localHeight = transform.localPosition.y;
        coords = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(transform.position);

        if (!PlaneManager.Instance.PlaneVisibilityWhenOffMap)
        {
            if (IsVisible && !MapCommands.Instance.Contains(coords))
            {
                SetVisibility(false);
            }
            else if (!IsVisible && MapCommands.Instance.Contains(coords))
            {
                SetVisibility(true);
            }
        }
    }


    private void ChangeZoom()
    {
        //if (!plane.GetComponent<ManeuverController>().IsFlying) //TODO: Ziv WTF?!

        transform.localScale = MapMovement.Instance.AbsoluteZoomRatio * defaultScale;
        transform.position = OnlineMapsTileSetControl.instance.GetWorldPosition(GetComponent<PlaneDisplayController>().coords);
        transform.localPosition = new Vector3(transform.localPosition.x, GetComponent<PlaneDisplayController>().localHeight * MapMovement.Instance.CurrentZoomRatio, transform.localPosition.z);
    }

    private void ChangePosition()
    {
        //if (!plane.GetComponent<ManeuverController>().IsFlying) //TODO: Ziv WTF?!
        {
            var newPosition = transform.position + MapMovement.Instance.MovementVector;
            transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        }

    }

    public void SetVisibility(bool value)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<MeshRenderer>() != null)
            {
                child.GetComponent<MeshRenderer>().enabled = value;
                continue;
            }
            if (child.GetComponent<ParticleRenderer>())
            {
                child.GetComponent<ParticleRenderer>().enabled = value;
            }
        }
        IsVisible = value;
    }

    #region Plane's Gas
    public void HandleGasAmount()
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
    public void SelectPlane()
    {
        ConvertColors(selectedColor);
    }

    public void DeselectPlane()
    {
        ConvertColors(defaultColor);
    }

    protected abstract void ConvertColors(Color color);
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
