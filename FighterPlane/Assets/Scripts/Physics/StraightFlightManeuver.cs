using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    //calculate a straight-line flight between two points. When the plane reaches the endpoint, the plane will cease to move.
    //
    //Calculate the line between two points using one of the definitions of a line between A and B:
    //line = tB + (1 - t)A, where 0 <= t <= 1
    class StraightFlightManeuver : Maneuver
    {
        public StraightFlightManeuver(Vector3 startPoint, Vector3 endPoint, float flightSpeed, Quaternion currentOrientation)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            orientation = Quaternion.LookRotation(startPoint - endPoint, Vector3.up);
            startTime = Time.time;
            tNormFactor = flightSpeed / (endPoint - startPoint).magnitude;
            correctPoseManeuver = new CorrectPoseManeuver(currentOrientation, orientation);
        }

        public bool finished = false;
        Vector3 startPoint, endPoint;
        Quaternion orientation;
        float startTime;
        float tNormFactor;
        float t;
        CorrectPoseManeuver correctPoseManeuver;

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
            if (Time.time - startTime >= GlobalManager.timeToCorrectPose)
            {
                return orientation;
            }
            else
            {
                return correctPoseManeuver.CalculateWorldRotation();
            }
        }

        public override Vector3 GetCenter()
        {
            return endPoint;
        }

        public override void UpdateOnMapMoved(Vector3 movementVector)
        {
            startPoint += movementVector;
            endPoint += movementVector;
        }

        public override void UpdateOnZoomChanged(Transform relativeTransform, float currentZoomRatio, float absoluteZoomRatio)
        {
            startPoint.y = CalculateYOnZoomChanged(relativeTransform, currentZoomRatio, startPoint.y);
            endPoint.y = CalculateYOnZoomChanged(relativeTransform, currentZoomRatio, endPoint.y);
        }

    }
}
