using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.Plane;
using Assets.Scripts.Physics;
using System.Collections.Generic;
using HoloToolkit.Unity;

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
    public GameObject planeName;
    public GameObject lackOfGasAlert;
    public GameObject planeCamera;
    public GameObject pilotCamera;
    public GameObject distanceText;
    public GameObject distanceLine;
    public float gasAmount = 100;

    public Vector2 coords;
    public float localHeight;

    private Vector3 defaultScale;
    private Vector3 targetPosition;
    private bool isDistanceShown = false;

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

        setPlaneName();

        distanceText.transform.localScale /= transform.localScale.x;
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

        if (isDistanceShown)
        {
            SetLinePosition(transform.position, targetPosition, Color.white, Color.white);
        }
    }



    private void ChangeZoom()
    {
        transform.localScale = MapMovement.Instance.AbsoluteZoomRatio * defaultScale;
        Debug.Log("zoom: " + MapMovement.Instance.AbsoluteZoomRatio.ToString());
        //GetComponentInChildren<EllipsoidParticleEmitter>().emit = MapMovement.Instance.AbsoluteZoomRatio > 0.25;
        if (MapMovement.Instance.AbsoluteZoomRatio <= 0.25)//number is not arbitrary; this is the level above which the default smoke trail swallows the plane
        {
            GetComponentInChildren<EllipsoidParticleEmitter>().minSize = 0.003f;
            GetComponentInChildren<EllipsoidParticleEmitter>().maxSize = 0.003f;
        }
        else
        {
            GetComponentInChildren<EllipsoidParticleEmitter>().minSize = 0.03f;
            GetComponentInChildren<EllipsoidParticleEmitter>().maxSize = 0.03f;
        }
        //if plane is flying then the manuver changes the zoom
        if (!GetComponent<ManeuverController>().IsFlying)
        {
            transform.position = OnlineMapsTileSetControl.instance.GetWorldPosition(GetComponent<PlaneDisplayController>().coords);
            transform.localPosition = new Vector3(transform.localPosition.x, GetComponent<PlaneDisplayController>().localHeight * MapMovement.Instance.CurrentZoomRatio, transform.localPosition.z);
        }
    }

    private void ChangePosition()
    {
        //if plane is flying then the manuver changes the zoom
        if (!GetComponent<ManeuverController>().IsFlying)
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
                TextToSpeechManager.Instance.SpeakText(string.Format("{0} has no gas. Would you like to go home?", gameObject.name));
                
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
		int height = (int)Math.Round(localHeight) * GlobalManager.heightDisplayFactor;
		planeInfo.GetComponent<TextMesh>().text =height+"ft" + GlobalManager.Reverse("גובה: ") + "\n"+ pParams.ToString() + "\n";
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

    #region Planes Distance
    public void ShowDistance()
    {
        distanceText.SetActive(true);
        distanceLine.SetActive(true);
    }

    public void HideDistance()
    {
        distanceText.SetActive(false);
        distanceLine.SetActive(false);
    }

    private void SetLinePosition(Vector3 startPoint, Vector3 endPoint, Color startColor, Color endColor)
    {
        distanceLine.GetComponent<LineRenderer>().SetPosition(0, startPoint);
        distanceLine.GetComponent<LineRenderer>().SetPosition(1, endPoint);
        distanceLine.GetComponent<LineRenderer>().SetColors(startColor, endColor);

        distanceText.transform.position = Vector3.Lerp(startPoint, endPoint, 0.5f);
        distanceText.GetComponent<TextMesh>().text = Math.Round((startPoint - endPoint).magnitude, 2) + "לימ";
    }

    private void SetLinePositions(List<Vector3> points)
    {
        distanceLine.GetComponent<LineRenderer>().numPositions = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            distanceLine.GetComponent<LineRenderer>().SetPosition(i, points[i]);
        }

        distanceLine.GetComponent<LineRenderer>().startColor = Color.white;
        distanceLine.GetComponent<LineRenderer>().endColor = Color.white;

        //distanceText.transform.position = Vector3.Lerp(startPoint, endPoint, 0.5f);
        //distanceText.GetComponent<TextMesh>().text = Math.Round((startPoint - endPoint).magnitude, 2) + " mi.";
    }

    public void ShowDistanceLine(GameObject target)
    {
        targetPosition = target.transform.TransformPoint(target.transform.position);
        isDistanceShown = true;
        ShowDistance();
    }

    public void HideDistanceLine()
    {
        isDistanceShown = false;
        HideDistance();
    }

    public void ShowAttackPath()
    {
        isDistanceShown = false;
        distanceLine.SetActive(true);
        distanceText.SetActive(false);

        SetLinePositions(gameObject.GetComponent<ManeuverController>().GetAttackPoints());
    }
    #endregion
}
