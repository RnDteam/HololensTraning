using UnityEngine;
using System.Collections;
using System;

public class Selected : MonoBehaviour {

    public bool isSelected;
    //private Color wingsFirstColor;
    private Color wingsSelectedColor;
    //private Color mainbodyFirstColor;
    private Color randomColor;
    private Color mainbodySelectedColor;
    private GameObject wings;
    private GameObject mainbody;

    private static System.Random rand = new System.Random();
    private Color[] colors = { Color.magenta, Color.yellow, new Color(255, 218, 185), new Color(187, 41, 187), new Color(175, 238, 238) };
    const double gravityMag = 9.8;
    const double hercArea = 162.1;
    const double hercMass = 34400;
    private Vector3 velocity;
    private Vector3 accelaration;
    private Vector3 prevVelocity = new Vector3(0f, 0f, 0f);
    private Vector3 position;
    private Vector3 prevPosition;
    private PlaneManager planeManager;
    public int planeNumber;

    // Plane details
    public double speed;
    public double angleOfAttack;
    public double angleOfAscent;
    public double lift;
    public double inducedDrag;
    public double parasiticDrag;
    public double totalDrag;
    public double thrust;

    private double CalculateAirPressure()
    {
        return 101325 * Math.Exp((-gravityMag * 0.0289644 * transform.position.y / (8.3144598 * 273.6)));
    }

    void Start () {
        GameObject g = GameObject.Find("PlaneManager");
        planeManager = g.GetComponent<PlaneManager>();

        wings = transform.Find("Wings").gameObject;
        mainbody = transform.Find("Main_Body").gameObject;
        position = transform.position;
        prevPosition = position;
        velocity = new Vector3(0f, 0f, 0f);
        prevVelocity = velocity;
        accelaration = new Vector3(0f, 0f, 0f);
        randomColor = colors[rand.Next(0, colors.Length - 1)];//Color.Lerp(Color.yellow, Color.magenta, (float) rand.NextDouble());
        //wingsFirstColor = wings.GetComponent<Renderer>().material.color;
        wingsSelectedColor = Color.blue;
        //mainbodyFirstColor = mainbody.GetComponent<Renderer>().material.color;
        mainbodySelectedColor = Color.blue;

        isSelected = false;
    }
    
    void FixedUpdate()
    {
        //position = transform.position;
        velocity = (transform.position - prevPosition) / Time.fixedDeltaTime;
            //new Vector3((position.x - prevPosition.x) / Time.deltaTime, (position.y - prevPosition.y) / Time.deltaTime, (position.z - prevPosition.z) / Time.deltaTime);
        accelaration = new Vector3((velocity.x - prevVelocity.x) / Time.deltaTime, (velocity.y - prevVelocity.y) / Time.deltaTime, (velocity.z - prevVelocity.z) / Time.deltaTime);

        speed = velocity.magnitude;
        angleOfAttack = transform.rotation.x;
        if (speed == 0)
        {
            angleOfAscent = 0;
        }
        else
        {
            angleOfAscent = Math.Asin(velocity.y / velocity.magnitude);
        }
        parasiticDrag = 0.5 * CalculateAirPressure() * Math.Pow(speed, 2) * 0.4 * 162.1;
        lift = Math.Cos(angleOfAttack) * (hercMass * (accelaration.y + gravityMag) + parasiticDrag * Math.Sin(angleOfAscent)) - Math.Sin(angleOfAttack) * (hercMass * accelaration.x + parasiticDrag * Math.Cos(angleOfAscent));
        inducedDrag = lift * Math.Sin(angleOfAttack) * Math.Cos(angleOfAscent);
        totalDrag = inducedDrag + parasiticDrag;
        thrust = Math.Sin(angleOfAttack) * (hercMass * (accelaration.y + gravityMag) + parasiticDrag * Math.Sin(angleOfAscent)) + Math.Cos(angleOfAttack) * (hercMass * Math.Sqrt(Math.Pow(accelaration.x, 2) + Math.Pow(accelaration.z, 2)) + parasiticDrag * Math.Cos(angleOfAscent));

        prevVelocity = velocity;
        prevPosition = transform.position;
    }

    void Update () {
        Selection();
    }

    private void Selection()
    {
        if (isSelected)
        {
            wings.GetComponent<Renderer>().material.color = wingsSelectedColor;
            mainbody.GetComponent<Renderer>().material.color = mainbodySelectedColor;
        }
        else
        {
            wings.GetComponent<Renderer>().material.color = randomColor;//wingsFirstColor;
            mainbody.GetComponent<Renderer>().material.color = randomColor;// mainbodyFirstColor;
        }
    }

    /// <summary>
    /// Called when our object is selected.  Generally called by
    /// a gesture management component.
    /// </summary>
    public void OnSelect()
    {
        planeManager.SelectPlaneByNumber(planeNumber);
    }
}
