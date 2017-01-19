using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    public class DoLoop : Maneuver
    {
        public DoLoop(Vector3 currentPosition, Quaternion currentRotation, float omega = GlobalManager.defaultLoopOmega, float r = GlobalManager.defaultLoopRadius)
        {
            Vector3 currentForward = currentRotation * Vector3.forward;
            r *= MapMovement.Instance.AbsoluteZoomRatio;
            centerX = currentPosition.x;
            centerY = currentPosition.y + r;
            centerZ = currentPosition.z;
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
            //The minus sign is because our hercules model's forward vector is towards its tail
            zComponentOfHorizontal = Vector3.Dot(-currentForward, Vector3.forward);
            xComponentOfHorizontal = Vector3.Dot(-currentForward, Vector3.right);
            correctPoseManeuver = new CorrectPoseManeuver(currentRotation, Quaternion.LookRotation(-new Vector3((float)(-xComponentOfHorizontal * Math.Sin(omega * GlobalManager.timeToCorrectPose + phase)), (float)Math.Cos(omega * GlobalManager.timeToCorrectPose + phase), (float)(-zComponentOfHorizontal * Math.Sin(omega * GlobalManager.timeToCorrectPose + phase))), -new Vector3(xComponentOfHorizontal * r * (float)Math.Cos(omega * GlobalManager.timeToCorrectPose + phase) + centerX, r * (float)Math.Sin(omega * GlobalManager.timeToCorrectPose + phase) + centerY, zComponentOfHorizontal * r * (float)Math.Cos(omega * GlobalManager.timeToCorrectPose + phase) + centerZ) + new Vector3(centerX, centerY, centerZ)));
        }

        float centerX;
        float centerY;
        float centerZ;
        float omega;
        float r;
        float startTime;
        float zComponentOfHorizontal = 1;
        float xComponentOfHorizontal = 0;
        float phase = (float) -Math.PI/2;
        CorrectPoseManeuver correctPoseManeuver;

        public override Vector3 CalculateWorldPosition()
        {
            //calculate the new position based on the parametric equation for circular motion:
            //
            //horizontal(t) = Rcos(omega*t + phi) + horizontal0
            //vertical(t) = Rcos(omega*t + phi) + vertical0
            //
            //where R is the radius of the circle, omega is the angular frequency, t is time, phi is the phase,
            //horizontal0 is the horizontal component of the center of the circle, and vertical0 is the vertical
            //component of the center of the circle.
            //Additionaly, since we are working in world coordinates, we divide the horizontal component into x and y components
            //
            //A one-dimensional form of this equation can be found at https://en.wikipedia.org/wiki/Simple_harmonic_motion
            return new Vector3(xComponentOfHorizontal * r * (float)Math.Cos(omega * (Time.time - startTime) + phase) + centerX, r * (float)Math.Sin(omega * (Time.time - startTime) + phase) + centerY, zComponentOfHorizontal * r * (float)Math.Cos(omega * (Time.time - startTime) + phase) + centerZ);
        }

        public override Quaternion CalculateWorldRotation()
        {
            //Calculate the "forward" direction by taking the derivative of the position with respect to time, removing factors shared
            //by all components of the vector like r and omega
            //The minus sign in front of the vector is because our Hercules model's "forward" direction is in the direction of the tail
            //I don't know why the "upward" direction is AWAY from the center of the circle instead of TOWARDS the center of the circle,
            //but that is the only way to make it work.
            if (Time.time - startTime >= GlobalManager.timeToCorrectPose)
            {
                return Quaternion.LookRotation(-new Vector3((float)(-xComponentOfHorizontal * Math.Sin(omega * (Time.time - startTime) + phase)), (float)Math.Cos(omega * (Time.time - startTime) + phase), (float)(-zComponentOfHorizontal * Math.Sin(omega * (Time.time - startTime) + phase))), -CalculateWorldPosition() + new Vector3(centerX, centerY, centerZ));
            }
            else
            {
                return correctPoseManeuver.CalculateWorldRotation();
            }
        }

        public override void UpdateOnMapMoved(Vector3 movementVector)
        {
            centerX += movementVector.x;
            centerZ += movementVector.z;
        }

        public override void UpdateOnZoomChanged(Transform relativeTransform, float currentZoomRatio, float absoluteZoomRatio)
        {
            centerY = CalculateYOnZoomChanged(relativeTransform, currentZoomRatio, centerY);
            r *= currentZoomRatio;
        }

        public override Vector3 GetCenter()
        {
            return new Vector3(centerX, centerY, centerZ);
        }
    }
}
