using UnityEngine;
using System.Collections;
using System;
using HoloToolkit.Unity;

public class PlaneManager : MonoBehaviour {

    private enum PLANES {
        PlaneA,
        PlaneB
    }
    private int selectedPlaneIndex;
    public GameObject planesDistance;
    public GameObject[] planes;
    private bool[] isTextDisplayed;// True means displayed, false - hidden.
    public Color lineColor;
    public GameObject distanceLine;
    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;
    private bool easterEnabled = false;

    private float rotationFactor;

    void Start () {
        selectedPlaneIndex = 0;
        isTextDisplayed = new bool[planes.Length];

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
        lr.SetColors(lineColor, lineColor);
        SetLinePosition(lr, planesDistance);
        lr.SetWidth(0.01f, 0.01f);

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
        PerformRotation();
        SetLinePosition(distanceLine.GetComponent<LineRenderer>(), planesDistance);
    }

    // Selecting planes using voice commands
    #region Selecting Planes
    public void SelectPlaneA()
    {
        ChangePlane((int)PLANES.PlaneA);
    }

    public void SelectPlaneB()
    {
        ChangePlane((int)PLANES.PlaneB);
    }

    private void ChangePlane(int currPlaneIndex)
    {
        int prevPlaneIndex = this.selectedPlaneIndex;

        // Updating value of the current plane index
        this.selectedPlaneIndex = currPlaneIndex;

        // Deselecting previous plane and selecting the new one
        DeselectPlane(prevPlaneIndex);
        SelectPlane(this.selectedPlaneIndex);
    }

    public void SelectPlaneByTap(int planeNumber)
    {
        // Deselect previous plane
        DeselectPlane(this.selectedPlaneIndex);

        // Update value of selected plane
        this.selectedPlaneIndex = planeNumber;
    }

    private void SelectPlane(int planeIndex)
    {
        planes[planeIndex].GetComponent<Selected>().SelectPlane();
    }

    private void DeselectPlane(int planeIndex)
    {
        planes[planeIndex].GetComponent<Selected>().DeselectPlane();
    }
    #endregion

    public void PlaySounds()
    {
        foreach (GameObject plane in planes)
        {
            plane.GetComponent<AudioSource>().Pause();
        }
        if (easterEnabled)
        {
            GetComponent<AudioSource>().Play();
        }
        else
        {
            planes[selectedPlaneIndex].GetComponent<AudioSource>().Play();
        }
    }

    private void PerformRotation()
    {
        if (GestureManager.Instance.IsNavigating)
        {
            // This will help control the amount of rotation.
            rotationFactor = GestureManager.Instance.NavigationPosition.x * RotationSensitivity;

            planes[selectedPlaneIndex].transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
        }
    }

    public void AnimatePlane()
    {
        PlaySounds();
        StartCoroutine(planes[selectedPlaneIndex].GetComponent<AnimationControl>().PlayAnimation(planes[selectedPlaneIndex].name + "Animation"));
    }

    public void CheckDisplaySign()
    {
        planes[selectedPlaneIndex].GetComponent<Selected>().ShowPlaneInfo();
    }

    public void UncheckDisplaySign()
    {
        planes[selectedPlaneIndex].GetComponent<Selected>().HidePlaneInfo();
    }

    public void DisplayPlaneDetails(int curPlaneIndex)
    {
        GameObject curPlane = planes[curPlaneIndex];

        Selected selectedPlane = curPlane.GetComponent<Selected>();
        TextMesh planeText = curPlane.GetComponentInChildren<TextMesh>();

        if(curPlane.GetComponent<Animator>().enabled)
        {
            if (selectedPlane.speed != 0)
            {
                planeText.text = string.Format("Plane Speed: {0:0}\nAzimuth: {1}", (selectedPlane.speed * 100).ToString("000"), selectedPlane.transform.rotation.eulerAngles.y.ToString("000"));
            }
        }
        else
        {
            planeText.text = string.Format("Plane Speed: {0:0}\nAzimuth: {1}", "000", selectedPlane.transform.rotation.eulerAngles.y.ToString("000"));
        }
    }

    public void HidePlaneDetails(int curPlaneIndex)
    {
        planes[curPlaneIndex].GetComponentInChildren<Selected>().HidePlaneInfo();
    }

    public void DisplayDistance()
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
}
