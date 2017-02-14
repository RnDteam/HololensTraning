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
        HerculesC,
        LeviatanA,
        LeviatanB,
        LeviatanC
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
    public AudioSource attackRecord;
    public AudioSource showPathRecord;

    //private Vector3 defaultScale;

    void Start()
    {
        // Default Selection
        previousPlane = planes[(int)PLANES.HerculesA];
        selectedPlane = planes[(int)PLANES.HerculesA];
        currentCam = selectedPlane.GetComponent<PlaneDisplayController>().pilotCamera;

        InitializeDistanceLine();

        foreach (GameObject plane in planes)
        {
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().Stop();
            }
        }
    }

    public void AllPlanesTakeOff()
    {
        foreach (GameObject plane in planes)
        {
            AddManeuver(new BeginFlightManeuver(plane.transform.position, plane.transform.rotation), plane);
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
        if(previousPlane != selectedPlane)
        {
            lr.SetPosition(0, previousPlane.transform.position);
            lr.SetPosition(1, selectedPlane.transform.position);

            Vector3 middlePoint = (previousPlane.transform.position + selectedPlane.transform.position) / 2;
            distance.transform.position = middlePoint;

            TextMesh text = distance.GetComponent<TextMesh>();
            text.text = Math.Round((previousPlane.transform.position - selectedPlane.transform.position).magnitude, 1) + " mi.";
        }
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

    public void SelectHerculesC()
    {
        ChangePlane(planes[(int)PLANES.HerculesC]);
    }

    public void SelectLeviatanA()
    {
        ChangePlane(planes[(int)PLANES.LeviatanA]);
    }

    public void SelectLeviatanB()
    {
        ChangePlane(planes[(int)PLANES.LeviatanB]);
    }

    public void SelectLeviatanC()
    {
        ChangePlane(planes[(int)PLANES.LeviatanC]);
    }

    public void SelectPlaneByIndex(int index)
    {
        if (index>0 && index < Enum.GetNames(typeof(PLANES)).Length)
        ChangePlane(planes[index]);
    }

    private void ChangePlane(GameObject currPlane)
    {
        previousPlane = this.selectedPlane;

        // Updating value of the current plane
        this.selectedPlane = currPlane;

        // Deselecting previous plane and selecting the new one
        DeselectPlane(previousPlane);
        SelectPlane(this.selectedPlane);

        PlaySounds();
    }

    public void SetCollisionPlanes(string planeName1, string planeName2)
    {
        DeselectPlane(selectedPlane);

        var plane1 = planes.Single(p => p.name == planeName1);
        var plane2 = planes.Single(p => p.name == planeName2);
        previousPlane = plane2;
        selectedPlane = plane1;

        SelectPlane(selectedPlane);
        ShowDistance();

        ShowInfo(plane1);
        ShowInfo(plane2);
    }

    public void UnsetCollisionPlanes()
    {
        HideDistance();
        DeselectPlane(selectedPlane);
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

    public void ShowInfo(GameObject gameObject)
    {
        gameObject.GetComponent<PlaneDisplayController>().ShowPlaneInfo();
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

    private void AddManeuver(Maneuver newManeuver, GameObject selectedPlane)
    {
        selectedPlane.GetComponent<ManeuverController>().SetManeuver(newManeuver);
    }

    private void AddManeuver(Maneuver newManeuver)
    {
        AddManeuver(newManeuver, selectedPlane);
    }

    private void AddManeuver(Maneuver newManeuver, string plane)
    {
        AddManeuver(newManeuver, planes.Single(p => p.name == plane));
    }

    public void DoCircle()
    {
        AddManeuver(new MakeCircle(selectedPlane.transform.position, selectedPlane.transform.rotation));
    }

    public void DoCircle(string planeName)
    {
        var plane = planes.Single(p => p.name == planeName);
        AddManeuver(new MakeCircle(plane.transform.position, plane.transform.rotation), planeName);
    }

    public void DoLoop()
    {
        AddManeuver(new DoLoop(selectedPlane.transform.position, selectedPlane.transform.rotation));
    }

    public void DoLoop(string planeName)
    {
        var plane = planes.Single(p => p.name == planeName);
        AddManeuver(new DoLoop(plane.transform.position, plane.transform.rotation), plane);
    }

    public void Escape()
    {
        AddManeuver(new LoopThenCircle(selectedPlane.transform.position, selectedPlane.transform.rotation));
    }

    public void BeginFlight()
    {
        AddManeuver(new BeginFlightManeuver(selectedPlane.transform.position, selectedPlane.transform.rotation));
    }

    public void StartFlying()
    {
        AllPlanesTakeOff();
        foreach (var plane in planes) {
            StartFlying(plane);
        }
    }

    public void StartFlying(GameObject plane)
    {
        var standardManeuver = new StandardManeuver(plane.transform.position, plane.transform.rotation, plane.transform.position - plane.transform.forward, plane.GetComponent<ManeuverController>().Speed, GlobalManager.defaultCircleRadius, plane.GetComponent<ManeuverController>().Speed * 2);

        standardManeuver.StartStraightFlight += () => plane.GetComponent<BoxCollider>().enabled = true;;
        standardManeuver.FinishedStraightFlight += () => plane.GetComponent<BoxCollider>().enabled = false;

        AddManeuver(standardManeuver, plane);
    }

    public void BeginFlight(GameObject plane)
    {
        AddManeuver(new BeginFlightManeuver(plane.transform.position, plane.transform.rotation), plane);
    }

    public void BeginFlight(string planeName)
    {
        var plane = planes.Single(p => p.name == planeName);
        AddManeuver(new BeginFlightManeuver(plane.transform.position, plane.transform.rotation), plane);
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
        RemoveDistanceLines();

        var building = BuildingManager.Instance.SelectedBuilding;
        if (building == null)
        {
            return;
        }

        var relevantPlanes = planes.Where(p => p.GetComponent<PlaneWeapon>().Weapon != Weapon.None && p.GetComponent<PlaneWeapon>().Weapon == building.GetComponent<BuildingWeapon>().Weapon);
        ShowDistanceLines(building, relevantPlanes);
    }

    private static void ShowDistanceLines(GameObject building, IEnumerable<GameObject> relevantPlanes)
    {
        foreach (var plane in relevantPlanes)
        {
            plane.GetComponent<PlaneDisplayController>().ShowDistanceLine(building);
        }
    }

    private void RemoveDistanceLines()
    {
        foreach (var plane in planes)
        {
            plane.GetComponent<PlaneDisplayController>().HideDistanceLine();
        }
    }

    public void AttackBuilding()
    {
        RemoveDistanceLines();
        attackRecord.Play();
        AddManeuver(new AttackBuildingManeuver(selectedPlane.transform.position, selectedPlane.transform.rotation, OnlineMapsTileSetControl.instance.GetWorldPosition(BuildingManager.Instance.SelectedBuildingCoords), BuildingManager.Instance.SelectedBuilding));
    }

    public void ShowAttackPath()
    {
        showPathRecord.Play();
        RemoveDistanceLines();
        selectedPlane.GetComponent<PlaneDisplayController>().ShowAttackPath();
    }

    public void Climb(float height)
    {
        AddManeuver(new ClimbManeuver((ATCManeuver) selectedPlane.GetComponent<ManeuverController>().getManeuver(), height), selectedPlane);
    }

    public void Climb(float height, string planeName)
    {
        var plane = planes.Single(p => p.name == planeName);
        AddManeuver(new ClimbManeuver((ATCManeuver)plane.GetComponent<ManeuverController>().getManeuver(), height), plane);
    }
}
