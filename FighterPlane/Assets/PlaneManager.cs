using UnityEngine;
using System.Collections;
using System;
using HoloToolkit.Unity;

public class PlaneManager : MonoBehaviour {

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
            plane.GetComponent<Selected>().isSelected = false;
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

        for (int i=0; i < isTextDisplayed.Length; i++)
        {
            if(isTextDisplayed[i])
            {
                DisplayPlaneDetails(i);
            }
            else
            {
                HidePlaneDetails(i);
            }
        }
    }

    public void SelectPlaneByNumber(int planeNumber)
    {
        if(planeNumber == 1)
        {
            SelectPlaneA();
        }
        else if (planeNumber == 2)
        {
            SelectPlaneB();
        }
    }
    public void SelectPlaneA()
    {
        selectedPlaneIndex = 0;
        SelectPlane();
    }

    private void SelectPlane()
    {
        foreach(GameObject plane in planes)
        {
            plane.GetComponent<Selected>().isSelected = false;
        }
        
        planes[selectedPlaneIndex].GetComponent<Selected>().isSelected = true;
    }

    public void PlayMusic()
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

    public void SelectPlaneB()
    {
        selectedPlaneIndex = 1;
        SelectPlane();
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
        PlayMusic();
        StartCoroutine(planes[selectedPlaneIndex].GetComponent<AnimationControl>().PlayAnimation(planes[selectedPlaneIndex].name + "Animation"));
    }

    public void CheckDisplaySign()
    {
        isTextDisplayed[selectedPlaneIndex] = true;
    }

    public void UncheckDisplaySign()
    {
        isTextDisplayed[selectedPlaneIndex] = false;
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

        
        //"Selected Plane:\n"
        // curPlane.name + "\n"
        //+ "Angle of Attack: " + (selectedPlane.angleOfAttack*180/Math.PI) + "\n"
        //+ "Angle of Ascent: " + (selectedPlane.angleOfAscent * 180 / Math.PI) + "\n"
        //+ "Lift: " + selectedPlane.lift + "\n"
        //+ "Total Drag: " + selectedPlane.totalDrag + "\n"
        //+ "Induced Drag: " + selectedPlane.inducedDrag + "\n"
        //+ "Parasitic Drag: " + selectedPlane.parasiticDrag + "\n"
        //+ "Thrust: " + selectedPlane.thrust;
    }

    public void HidePlaneDetails(int curPlaneIndex)
    {
        planes[curPlaneIndex].GetComponentInChildren<TextMesh>().text = "";
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
