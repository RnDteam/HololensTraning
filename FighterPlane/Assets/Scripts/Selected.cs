using UnityEngine;
using System.Collections;
using System;

public class Selected : MonoBehaviour {


    // New
    public bool IsTextShown {
        get;
        private set;
    }
    // todo if still needed
    // isTraceShown
    // isRegularSoundPlayed


    private PlaneManager planeManager;
    public int planeNumber;
    private Color selectedColor;
    private Color defaultColor;

    private GameObject wings;
    private GameObject mainbody;

    private static System.Random rand = new System.Random();

    // todo Refactor later

    private Color[] colors = { Color.magenta, Color.yellow, new Color(255, 218, 185), new Color(187, 41, 187), new Color(175, 238, 238) };

    #region physics
    private Vector3 velocity;
    private Vector3 accelaration;
    private Vector3 prevVelocity = new Vector3(0f, 0f, 0f);
    private Vector3 position;
    private Vector3 prevPosition;
    #endregion
    


    void Start () {
        planeManager = GameObject.Find("PlaneManager").GetComponent<PlaneManager>();
		defaultColor = colors[rand.Next(0, colors.Length - 1)];
        selectedColor = Color.blue;

        // Assigning wings and plane body for color purposes
        wings = transform.Find("Wings").gameObject;
        mainbody = transform.Find("Main_Body").gameObject;

        #region physics
        position = transform.position;
        prevPosition = position;
        velocity = new Vector3(0f, 0f, 0f);
        prevVelocity = velocity;
        accelaration = new Vector3(0f, 0f, 0f);
        #endregion
    }

    void Update()
    {
        velocity = (transform.position - prevPosition) / Time.fixedDeltaTime;
        accelaration = new Vector3((velocity.x - prevVelocity.x) / Time.deltaTime, (velocity.y - prevVelocity.y) / Time.deltaTime, (velocity.z - prevVelocity.z) / Time.deltaTime);


        if (IsTextShown)
		{
			UpdateText();
		}
        
        prevVelocity = velocity;
        prevPosition = transform.position;
    }

    private void UpdateText()
    {
        gameObject.GetComponentInChildren<TextMesh>().text = PhysicsManager.CalculateFlightParameters(accelaration, velocity, transform, prevPosition, prevVelocity).ToString();
    }

    #region Selecting Plane
    public void OnSelect()
    {
        SelectPlane();

        // Notifying plane manager a plane was picked
        planeManager.SelectPlaneByTap(planeNumber - 1);
    }

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

    #region Visibility of Plane Details
    public void HidePlaneInfo()
    {
        gameObject.GetComponentInChildren<TextMesh>().text = "";
        IsTextShown = false;
    }

    public void ShowPlaneInfo()
    {
        IsTextShown = true;
    }
    #endregion
}
