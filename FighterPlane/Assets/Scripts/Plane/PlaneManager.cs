using UnityEngine;
using System.Collections;
using System;
using HoloToolkit.Unity;
using System.Collections.Generic;
using Assets.Scripts.Physics;

public class PlaneManager : MonoBehaviour {

    private enum PLANES {
        PlaneA,
        PlaneB
    }

    // Indexes of selected and previous planes
    private GameObject selectedPlane;
    private GameObject previousPlane;
    
    // Planes objects array
    public GameObject[] planes;
    private Dictionary<GameObject, Maneuver> maneuvers = new Dictionary<GameObject, Maneuver>();

    public GameObject planesDistance;
    public GameObject distanceLine;
    public Color lineColor;
    public GameObject AlertDome_1;

    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;
    private bool easterEnabled = false;

    private float rotationFactor;

    private System.Random rnd = new System.Random();

    void Start () {
        selectedPlane = planes[0];

        InitializeDistanceLine();

        foreach (GameObject plane in planes)
        {
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().Stop();
            }
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
        lr.SetPosition(0, planes[0].transform.position);
        lr.SetPosition(1, planes[1].transform.position);

        Vector3 middlePoint = (planes[0].transform.position + planes[1].transform.position) / 2;
        distance.transform.position = middlePoint;

        TextMesh text = distance.GetComponent<TextMesh>();
        text.text = Math.Round((planes[0].transform.position - planes[1].transform.position).magnitude, 2) + " km";
    }

    void Update () {
        RotatePlaneByHandGesture();
        SetLinePosition(distanceLine.GetComponent<LineRenderer>(), planesDistance);
        ManageManeuvers();

    }

    private void ManageManeuvers()
    {
        foreach (GameObject plane in maneuvers.Keys)
        {
            plane.transform.position = maneuvers[plane].newPos();
            plane.transform.rotation = maneuvers[plane].newRot();
        }
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
        previousPlane.GetComponent<AudioSource>().Pause();
        
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

    public void AnimatePlane()
    {
        PlaySounds();
        StartCoroutine(selectedPlane.GetComponent<AnimationControl>().PlayAnimation(selectedPlane.name + "Animation"));
    }

    public void ShowInfo()
    {
        selectedPlane.GetComponent<PlaneDisplayController>().ShowPlaneInfo();
    }

    public void HideInfo()
    {
        selectedPlane.GetComponent<PlaneDisplayController>().HidePlaneInfo();
    }

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

    public void ToggleEasterEgg()
    {
        easterEnabled = !easterEnabled;
    }

    private void AddManeuver(Maneuver man)
    {
        if (maneuvers.ContainsKey(selectedPlane))
        {
            maneuvers[selectedPlane] = man;
        }
        else
        {
            maneuvers.Add(selectedPlane, man);
        }
    }

    public void DoCircle()
    {
        AddManeuver(new MakeCircle(selectedPlane.transform.position, (float)rnd.NextDouble(), (float)rnd.NextDouble()));
    }

    public void DoLoop()
    {
        AddManeuver(new DoLoop(selectedPlane.transform.position, (float)rnd.NextDouble(), (float)rnd.NextDouble()));
    }

    public void Escape()
    {
        AddManeuver(new LoopThenCircle(selectedPlane.transform.position, (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble()));
    }

    public void DoSplitS()
    {
        AddManeuver(new SplitS(selectedPlane.transform.position, selectedPlane.transform.rotation, 1.5f, 0.1f, 1, 1, 1));
    }
}
