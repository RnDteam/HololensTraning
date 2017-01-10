using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    //calculate a straight-line flight between two points. Does NOT change rotation: if the plane is incorrectly oriented to fly
    //between the two points, then this will preserve the incorrect orientation as the plane moves. When the plane reaches the
    //endpoint, the plane will cease to move.
    //
    //Calculate the line between two points using one of the definitions of a line between A and B:
    //line = tB + (1 - t)A, where 0 <= t <= 1
    class StraightFlightManeuver : Maneuver
    {
        public StraightFlightManeuver(Vector3 startPoint, Vector3 endPoint, float flightSpeed, Quaternion constantOrientation)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.flightSpeed = flightSpeed;
            orientation = constantOrientation;
            startTime = Time.time;
            tNormFactor = flightSpeed / (endPoint - startPoint).magnitude;
        }

        public bool finished = false;
        Vector3 startPoint, endPoint;
        float flightSpeed;
        Quaternion orientation;
        float startTime;
        float tNormFactor;
        float t;

        public override void UpdateState()
        {
            finished = t >= 1;
        }

        public override Vector3 CalculateWorldPosition()
        {
            if (!finished)
            {
                t = tNormFactor * (Time.time - startTime);
                return t * endPoint + (1 - t) * startPoint;
            }
            else
            {
                return endPoint;
            }
        }

        public override Quaternion CalculateWorldRotation()
        {
            return orientation;
        }
    }
}
