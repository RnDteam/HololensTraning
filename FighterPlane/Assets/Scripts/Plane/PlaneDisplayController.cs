using UnityEngine;
using System.Collections;
using System;

public class PlaneDisplayController : MonoBehaviour
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

    public int planeNumber;

    // Gas variables
    public float gasAmount = 100;

    private Color selectedColor;
    public Color defaultColor;
    public GameObject planeInfo;
    public GameObject planeName;
    public GameObject lackOfGasAlert;
    public GameObject planeCamera;
    public GameObject pilotCamera;

    public Vector2 coords;
    public float localHeight;

    private GameObject wings;
    private GameObject mainbody;

    private PhysicsParameters pParams;

    public bool IsVisible;
    
    void Start () {
        // Assigning wings and plane body for color purposes
        wings = transform.Find("Wings").gameObject;
        mainbody = transform.Find("Main_Body").gameObject;

        pParams = new PhysicsParameters(transform);
        IsGasAlertActive = false;
        selectedColor = Color.blue;
        ConvertColors(defaultColor);
        
        if (PlaneManager.Instance.PlaneVisibilityWhenOffMap || MapCommands.Instance.Contains(coords))
        {
            IsVisible = true;
        }
        else
        {
            SetVisibility(false);
        }

        setPlaneName();
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
    private void HandleGasAmount()
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

    private void ConvertColors(Color color)
    {
        wings.GetComponent<Renderer>().material.color = color;
        mainbody.GetComponent<Renderer>().material.color = color;
    }
    #endregion

    #region Plane Details
    private void DisplayUpdatedInfo()
    {
        planeInfo.GetComponent<TextMesh>().text = "Weapon: " + GetComponent<PlaneWeapon>().Weapon.ToString() + "\n" + pParams.ToString() + "\n" + "Gas Amount(Liters): " + gasAmount.ToString("000.0");
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

    private void setPlaneName()
    {
        planeName.GetComponent<TextMesh>().text = name;
    }

    #endregion
}
