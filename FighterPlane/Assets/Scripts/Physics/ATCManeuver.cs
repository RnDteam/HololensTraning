using Assets.Scripts.Physics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ATCManeuver : Maneuver {
    public abstract Vector3 GetEndpoint();
    public abstract float GetFlightSpeed();
    public abstract float GetRadius();
    public abstract float GetOmega();
}
