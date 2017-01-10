using UnityEngine;
using System.Collections;
using System;
using HoloToolkit.Unity;
using System.Collections.Generic;
using Assets.Scripts.Physics;
using Assets.Scripts.Plane;

public class PlaneManager : MonoBehaviour {

    private enum PLANES {
        PlaneA,
        PlaneB
    }

    // Indexes of selected and previous planes
    private GameObject selectedPlane;
    private GameObject previousPlane;
    private GameObject selectedCamera;

    // Planes objects array
    public GameObject[] planes;

    public GameObject planesDistance;
    public GameObject distanceLine;
    public Color lineColor;
    public GameObject AlertDome_1;

    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;
    private bool easterEnabled = false;

    private float rotationFactor;
    private Vector3 defaultScale;

    void Start () {
        // Default Selection
        selectedPlane = planes[(int)PLANES.PlaneA];
        selectedCamera = selectedPlane.GetComponent<PlaneDisplayController>().pilotCamera;

        InitializeDistanceLine();

        MapMovement.Instance.Moved += ChangePosition;
        MapMovement.Instance.ZoomChanged += ChangeZoom;

        foreach (GameObject plane in planes)
        {
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().Stop();
            }
        }

        defaultScale = planes[0].transform.localScale;
        //previousZoom = defaultZoom = OnlineMaps.instance.zoom;
    }

    private void ChangePosition()
    {
        Debug.Log("Map moved " + MapMovement.Instance.MovementVector.ToString());
        foreach (var plane in planes)
        {
            var newPosition = plane.transform.position + MapMovement.Instance.MovementVector;
            plane.transform.position = new Vector3(newPosition.x, plane.transform.position.y, newPosition.z);
        }
    }

    private void ChangeZoom()
    {
        foreach (var plane in planes)
        {
            plane.transform.localScale = MapMovement.Instance.AbsoluteZoomRatio * defaultScale;

            plane.transform.position = OnlineMapsTileSetControl.instance.GetWorldPosition(plane.GetComponent<PlaneDisplayController>().coords);
            plane.transform.localPosition = new Vector3(plane.transform.localPosition.x, plane.GetComponent<PlaneDisplayController>().localHeight * MapMovement.Instance.CurrentZoomRatio, plane.transform.localPosition.z);
        }
    }

    private void InitializeDistanceLine()
    {
        LineRenderer lr = distanceLine.GetComponent<LineRenderer>();
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        SetLinePosition(lr, planesDistance);

        HideDistance();
    }
    
    private void SetLinePosition(LineRenderer lr, GameObject distance)
    {
        lr.SetPosition(0, planes[(int)PLANES.PlaneA].transform.position);
        lr.SetPosition(1, planes[(int)PLANES.PlaneB].transform.position);

        Vector3 middlePoint = (planes[(int)PLANES.PlaneA].transform.position + planes[(int)PLANES.PlaneB].transform.position) / 2;
        distance.transform.position = middlePoint;

        TextMesh text = distance.GetComponent<TextMesh>();
        text.text = Math.Round((planes[(int)PLANES.PlaneA].transform.position - planes[(int)PLANES.PlaneB].transform.position).magnitude, 2) + " km";
    }

    void Update () {
        //RotatePlaneByHandGesture();
        SetLinePosition(distanceLine.GetComponent<LineRenderer>(), planesDistance);

    }

    // Selecting planes using voice commands
    #region Selecting Planes
    public void SelectPlaneA()
    {
        ChangePlane(planes[(int)PLANES.PlaneA]);
    }

    public void SelectPlaneB()
    {
        ChangePlane(planes[(int)PLANES.PlaneB]);
    }

    private void ChangePlane(GameObject currPlane)
    {
        previousPlane = this.selectedPlane;

        // Updating value of the current plane
        this.selectedPlane = currPlane;

        // Deselecting previous plane and selecting the new one
        DeselectPlane(previousPlane);
        SelectPlane(this.selectedPlane);
    }

    private bool Contains(Array array, object val)
    {
        return Array.IndexOf(array, val) != -1;
    }

    public bool UpdateSelectedPlane(GameObject tappedObject)
    {
        // In case the tapped object is a plane in our array
        if (Contains(planes, tappedObject))
        {
            ChangePlane(tappedObject);

            return true;
        }

        return false;
    }

    private void SelectPlane(GameObject plane)
    {
        plane.GetComponent<PlaneDisplayController>().SelectPlane();
    }

    private void DeselectPlane(GameObject plane)
    {
        plane.GetComponent<PlaneDisplayController>().DeselectPlane();
    }
    #endregion

    public void PlaySounds()
    {
        if (previousPlane)
        {
            previousPlane.GetComponent<AudioSource>().Pause();
        }
        
        if (easterEnabled)
        {
            GetComponent<AudioSource>().Play();
        }
        else
        {
            selectedPlane.GetComponent<AudioSource>().Play();
        }
    }

    private void RotatePlaneByHandGesture()
    {
        if (GestureManager.Instance.IsNavigating)
        {
            // This will help control the amount of rotation.
            rotationFactor = GestureManager.Instance.NavigationPosition.x * RotationSensitivity;

            selectedPlane.transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
        }
    }

    #region Plane Animation
    public void AnimatePlane()
    {
        PlaySounds();
        StartCoroutine(selectedPlane.GetComponent<AnimationControl>().PlayAnimation(selectedPlane.name + "Animation"));
    }
    #endregion

    #region Plane Information
    public void ShowInfo()
    {
        selectedPlane.GetComponent<PlaneDisplayController>().ShowPlaneInfo();
    }

    public void HideInfo()
    {
        selectedPlane.GetComponent<PlaneDisplayController>().HidePlaneInfo();
    }
    #endregion

    #region Planes Distance
    public void ShowDistance()
    {
        planesDistance.SetActive(true);
        distanceLine.SetActive(true);
    }

    public void HideDistance()
    {
        planesDistance.SetActive(false);
        distanceLine.SetActive(false);
    }
    #endregion

    #region Plane Camera 
    public void ShowPilotView()
    {
        selectedPlane.GetComponent<PlaneDisplayController>().ShowPilotView();

        // Deselecting the old selectedCamera
        selectedCamera.SetActive(false);

        // Updating selectedCamera
        selectedCamera = selectedPlane.GetComponent<PlaneDisplayController>().planeCamera;
    }

    public void ShowPlaneView()
    {
        selectedPlane.GetComponent<PlaneDisplayController>().ShowPlaneView();

        // Deselecting the old selectedCamera
        selectedCamera.SetActive(false);

        // Updating selectedCamera
        selectedCamera = selectedPlane.GetComponent<PlaneDisplayController>().planeCamera;
    }

    public void ShowGroundView()
    {
        selectedCamera.SetActive(false);
    }
    #endregion
    
    #region Easter Egg
    public void ToggleEasterEgg()
    {
        easterEnabled = !easterEnabled;
    }
    #endregion

    private void AddManeuver(Maneuver newManeuver)
    {
        selectedPlane.GetComponent<ManeuverController>().SetManeuver(newManeuver);
    }

    public void DoCircle()
    {
        AddManeuver(new MakeCircle(selectedPlane.transform.position, selectedPlane.transform.right));
    }

    public void DoLoop()
    {
        AddManeuver(new DoLoop(selectedPlane.transform.position, selectedPlane.transform.forward));
    }

    public void Escape()
    {
        AddManeuver(new LoopThenCircle(selectedPlane.transform.position, selectedPlane.transform.forward));
    }

    public void BeginFlight()
    {
        AddManeuver(new BeginFlightManeuver(selectedPlane.transform.position, selectedPlane.transform.right));
    }

    /*
    public void DoSplitS()
    {
        AddManeuver(new SplitS(selectedPlane.transform.position, selectedPlane.transform.rotation, 1.5f, 0.1f, 1, 1, 1));
    }
    */
}
