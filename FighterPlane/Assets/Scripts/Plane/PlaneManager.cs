using UnityEngine;
using System;
using HoloToolkit.Unity;
using Assets.Scripts.Physics;
using Assets.Scripts.Plane;
using HoloToolkit;
using System.Collections.Generic;
using System.Linq;

public partial class PlaneManager : Singleton<PlaneManager>
{
    private enum PLANES
    {
        HerculesA,
        HerculesB,
        LeviatanA,
        LeviatanB
    }

    // Indexes of selected and previous planes
    private GameObject selectedPlane;
    private GameObject previousPlane;
    private GameObject currentCam;
    public GameObject mainCamera;

    // Planes objects array
    public GameObject[] planes;

    public GameObject planesDistance;
    public GameObject distanceLine;
    public Color lineColor;
    public GameObject AlertDome_1;

    public bool PlaneVisibilityWhenOffMap = true;

    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;
    private bool easterEnabled = false;

    private float rotationFactor;
    //private Vector3 defaultScale;

    void Start()
    {
        // Default Selection
        previousPlane = planes[(int)PLANES.HerculesA];
        selectedPlane = planes[(int)PLANES.HerculesA];
        currentCam = selectedPlane.GetComponent<PlaneDisplayController>().pilotCamera;

        InitializeDistanceLine();

        //MapMovement.Instance.Moved += ChangePosition;
        //MapMovement.Instance.ZoomChanged += ChangeZoom;

        foreach (GameObject plane in planes)
        {
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().Stop();
            }
        }

        //defaultScale = planes[0].transform.localScale;
    }

    //private void ChangePosition()
    //{
    //    foreach (var plane in planes)
    //    {
    //        if (!plane.GetComponent<ManeuverController>().IsFlying)
    //        {
    //            var newPosition = plane.transform.position + MapMovement.Instance.MovementVector;
    //            plane.transform.position = new Vector3(newPosition.x, plane.transform.position.y, newPosition.z);
    //        }
    //    }
    //}

    //private void ChangeZoom()
    //{
    //    foreach (var plane in planes)
    //    {
    //        plane.transform.localScale = MapMovement.Instance.AbsoluteZoomRatio * defaultScale;

    //        if (!plane.GetComponent<ManeuverController>().IsFlying)
    //        {
    //            plane.transform.position = OnlineMapsTileSetControl.instance.GetWorldPosition(plane.GetComponent<PlaneDisplayController>().coords);
    //            plane.transform.localPosition = new Vector3(plane.transform.localPosition.x, plane.GetComponent<PlaneDisplayController>().localHeight * MapMovement.Instance.CurrentZoomRatio, plane.transform.localPosition.z);
    //        }
    //    }
    //}

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
        lr.SetPosition(0, previousPlane.transform.position);
        lr.SetPosition(1, selectedPlane.transform.position);

        Vector3 middlePoint = (previousPlane.transform.position + selectedPlane.transform.position) / 2;
        distance.transform.position = middlePoint;

        TextMesh text = distance.GetComponent<TextMesh>();
        text.text = Math.Round((previousPlane.transform.position - selectedPlane.transform.position).magnitude, 2) + " km";
    }

    private IEnumerable<GameObject> GetPlanesWithWeapon(Weapon weapon)
    {
        return planes.Where(p => p.GetComponent<PlaneWeapon>().Weapon == weapon);
    }

    void Update()
    {
        SetLinePosition(distanceLine.GetComponent<LineRenderer>(), planesDistance);
    }

    // Selecting planes using voice commands
    #region Selecting Planes
    public void SelectHerculesA()
    {
        ChangePlane(planes[(int)PLANES.HerculesA]);
    }

    public void SelectHerculesB()
    {
        ChangePlane(planes[(int)PLANES.HerculesB]);
    }

    public void SelectLeviatanA()
    {
        ChangePlane(planes[(int)PLANES.LeviatanA]);
    }

    public void SelectLeviatanB()
    {
        ChangePlane(planes[(int)PLANES.LeviatanB]);
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

    #region Plane Sounds
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
    #endregion

    private void RotateHerculesByHandGesture()
    {
        if (GestureManager.Instance.IsNavigating)
        {
            // This will help control the amount of rotation.
            rotationFactor = GestureManager.Instance.NavigationPosition.x * RotationSensitivity;

            selectedPlane.transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
        }
    }

    #region Plane Information
    public void ShowInfo()
    {
        selectedPlane.GetComponent<PlaneDisplayController>(). ShowPlaneInfo();
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
        // Deselecting the old currentCam
        DisableCamera(currentCam);

        // Updating currentCam
        currentCam = EnableCamera(selectedPlane.GetComponent<PlaneDisplayController>().pilotCamera);
    }

    public void ShowPlaneView()
    {
        // Deselecting the old currentCam
        DisableCamera(currentCam);

        // Updating currentCam
        currentCam = EnableCamera(selectedPlane.GetComponent<PlaneDisplayController>().planeCamera);
    }

    public void ShowGroundView()
    {
        DisableCamera(currentCam);

        EnableCamera(mainCamera);
    }

    private GameObject EnableCamera(GameObject cam)
    {
        cam.GetComponent<Camera>().enabled = true;
        cam.GetComponent<AudioListener>().enabled = true;

        return cam;
    }

    private void DisableCamera(GameObject cam)
    {
        mainCamera.GetComponent<AudioListener>().enabled = false;

        cam.GetComponent<Camera>().enabled = false;
        cam.GetComponent<AudioListener>().enabled = false;
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
        AddManeuver(new MakeCircle(selectedPlane.transform.position, selectedPlane.transform.rotation));
    }

    public void DoLoop()
    {
        AddManeuver(new DoLoop(selectedPlane.transform.position, selectedPlane.transform.rotation));
    }

    public void Escape()
    {
        AddManeuver(new LoopThenCircle(selectedPlane.transform.position, selectedPlane.transform.rotation));
    }

    public void BeginFlight()
    {
        PlaySounds();
        AddManeuver(new BeginFlightManeuver(selectedPlane.transform.position, selectedPlane.transform.rotation));
    }

    public Vector3 GetPlaneCenter()
    {
        var position = selectedPlane.GetComponent<ManeuverController>().transform.position;
        if (selectedPlane.GetComponent<ManeuverController>().IsFlying)
        {
            position = selectedPlane.GetComponent<ManeuverController>().ManeuverCenter;
        }
        return position;
    }

    public void GetRelevantPlanes()
    {
        foreach (var plane in planes)
        {
            plane.GetComponent<PlaneDisplayController>().HideDistanceLine();
        }

        var building = BuildingManager.Instance.SelectedBuilding;
        if (building == null)
        {
            return;
        }

        var relevantPlanes = planes.Where(p => p.GetComponent<PlaneWeapon>().Weapon != Weapon.None && p.GetComponent<PlaneWeapon>().Weapon == building.GetComponent<BuildingWeapon>().Weapon);
        foreach (var plane in relevantPlanes)
        {
            plane.GetComponent<PlaneDisplayController>().ShowDistanceLine(building);
        }
    }

    public void AttackBuilding()
    {
        AddManeuver(new AttackBuildingManeuver(selectedPlane.transform.position, selectedPlane.transform.rotation, OnlineMapsTileSetControl.instance.GetWorldPosition(BuildingManager.Instance.SelectedBuildingCoords), BuildingManager.Instance.SelectedBuilding));
    }

    /*
    public void DoSplitS()
    {
        AddManeuver(new SplitS(selectedPlane.transform.position, selectedPlane.transform.rotation, 1.5f, 0.1f, 1, 1, 1));
    }
    */
}
