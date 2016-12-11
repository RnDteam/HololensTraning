using UnityEngine;
using System.Collections;
using System;
using HoloToolkit.Unity;

public class PlaneManager : MonoBehaviour {

    private int selectedPlaneIndex;
    private bool isDistanceShown;
    public TextMesh planesDistance;
    public GameObject[] planes;
    private bool[] isTextDisplayed;// True means displayed, false - hidden.

    [Tooltip("Rotation max speed controls amount of rotation.")]
    public float RotationSensitivity = 10.0f;

    private float rotationFactor;

    void Start () {
        isDistanceShown = false;
        selectedPlaneIndex = 0;
        isTextDisplayed = new bool[planes.Length];

        foreach (GameObject plane in planes)
        {
            plane.GetComponent<Selected>().isSelected = false;
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().Stop();
            }
        }
    }
	
	void Update () {
        CalculateDistance();
        PerformRotation();

        for(int i=0; i < isTextDisplayed.Length; i++)
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
            plane.GetComponent<AudioSource>().Pause();
        }

        planes[selectedPlaneIndex].GetComponent<AudioSource>().Play();
        planes[selectedPlaneIndex].GetComponent<Selected>().isSelected = true;
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
        
        planeText.text = "Selected Plane:\n" + curPlane.name + "\n"
            + "Plane Speed: " + selectedPlane.speed + "\n"
            + "Angle of Attack: " + (selectedPlane.angleOfAttack*180/Math.PI) + "\n"
            + "Angle of Ascent: " + (selectedPlane.angleOfAscent * 180 / Math.PI) + "\n"
            + "Lift: " + selectedPlane.lift + "\n"
            + "Total Drag: " + selectedPlane.totalDrag + "\n"
            + "Induced Drag: " + selectedPlane.inducedDrag + "\n"
            + "Parasitic Drag: " + selectedPlane.parasiticDrag + "\n"
            + "Thrust: " + selectedPlane.thrust;

    }

    public void HidePlaneDetails(int curPlaneIndex)
    {
        planes[curPlaneIndex].GetComponentInChildren<TextMesh>().text = "";
    }


    private void CalculateDistance()
    {
        if (isDistanceShown)
        {
            planesDistance.text = "The Distance \n Between Planes is \n" + (planes[0].transform.position - planes[1].transform.position).magnitude + " km";
        }
        else
        {
            planesDistance.text = "";
        }
    }

    public void DisplayDistance()
    {
        isDistanceShown = true;
    }

    public void HideDistance()
    {
        isDistanceShown = false;
    }
}
