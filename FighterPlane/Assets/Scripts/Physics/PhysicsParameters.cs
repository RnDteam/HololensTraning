using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsParameters
{
    public double Speed { get; set; }
    public double AngleOfAttack { get; set; }
    public double AngleOfAscent { get; set; }
    public double Lift { get;  set; }
    public double InducedDrag { get; set; }
    public double ParasiticDrag { get; set; }
    public double TotalDrag { get; set; }
    public double Thrust { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Position { get; set; }
    public Vector3 prevPosition { get; set; }
    public Vector3 Velocity { get; set; }
    public Vector3 prevVelocity { get; set; }
    public Vector3 Accelaration { get; set; }

    public PhysicsParameters(Transform transform)
    {
        Velocity = new Vector3(0f, 0f, 0f);
        Accelaration = new Vector3(0f, 0f, 0f);

        Rotation = transform.rotation;
        Position = transform.position;
        prevPosition = Position;
        prevVelocity = Velocity;
    }

    public override string ToString()
    {
        return string.Format("Plane Speed: {0:0}\nAzimuth: {1}", (Speed * 100).ToString("000"), Rotation.eulerAngles.y.ToString("000"));
    }

    internal void UpdatePhysics(Transform transform)
    {
        Rotation = transform.rotation;
        Position = transform.position;
        
        Velocity = (Position - prevPosition) / Time.deltaTime;
        Accelaration = new Vector3((Velocity.x - prevVelocity.x) / Time.deltaTime, (Velocity.y - prevVelocity.y) / Time.deltaTime, (Velocity.z - prevVelocity.z) / Time.deltaTime);

        PhysicsManager.CalculateFlightParameters(this);

        prevVelocity = Velocity;
        prevPosition = Position;
    }

    internal void SetPreviousVariables()
    {
        
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