using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsParameters
{
    public double Speed { get; set; }
    public double AngleOfAttack { get; set; }
    public double AngleOfAscent { get; set; }
    public double Lift { get; set; }
    public double InducedDrag { get; set; }
    public double ParasiticDrag { get; set; }
    public double TotalDrag { get; set; }
    public double Thrust { get; set; }
    public Quaternion Rotation { get; set; }


    public override string ToString()
    {
        return string.Format("Plane Speed: {0:0}\nAzimuth: {1}", (Speed * 100).ToString("000"), Rotation.eulerAngles.y.ToString("000"));
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