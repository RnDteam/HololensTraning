using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class MakeCircle : Maneuver
    {
        public MakeCircle(float centerX = 0, float height = 1, float centerZ = 0, float omega = GlobalManager.defaultCircleOmega, float r = GlobalManager.defaultCircleRadius)
        {
            this.centerX = centerX;
            this.height = height;
            this.centerZ = centerZ;
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
        }

        public MakeCircle(Vector3 currentPosition, Vector3 currentRight, float omega = GlobalManager.defaultCircleOmega, float r = GlobalManager.defaultCircleRadius)
        {
            centerX = currentPosition.x - r * Vector3.Dot(currentRight, Vector3.right);
            height = currentPosition.y;
            centerZ = currentPosition.z + r * Vector3.Dot(currentRight, Vector3.back);//"back" is towards the nose of the hercules
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
            phase = (float) Math.Atan2(Vector3.Dot(currentRight, Vector3.back), Vector3.Dot(currentRight, Vector3.right));
        }

        float phase = 0;
        float centerX;
        float height;
        float centerZ;
        float omega;
        float r;
        float startTime;

        public override Vector3 CalculateWorldPosition()
        {
            //calculate the new position based on the parametric equation for circular motion:
            //
            //x(t) = Rcos(omega*t + phi) + x0
            //y(t) = Rcos(omega*t + phi) + y0
            //
            //where R is the radius of the circle, omega is the angular frequency, t is time, phi is the phase,
            //x0 is the x component of the center of the circle, and y0 is the y component of the center of the circle.
            //
            //A one-dimensional form of this equation can be found at https://en.wikipedia.org/wiki/Simple_harmonic_motion
            return new Vector3(r * (float)Math.Cos(omega * (Time.time - startTime) + phase) + centerX, height, -r * (float)Math.Sin(omega * (Time.time - startTime) + phase) + centerZ);
        }

        public override Quaternion CalculateWorldRotation()
        {
            //Calculate the "forward" direction by taking the derivative of the position with respect to time, removing factors shared
            //by all components of the vector like r and omega
            //The minus sign in front of the vector is because our Hercules model's "forward" direction is in the direction of the tail
            //The bank angle is calculated as that angle which distributes the lift from the plane in the upwards direction and to the
            //center of the circle in the correct proportions, assuming that lift is in the plane's +y direction, with gravity acting
            //downwards and an accelaration of v^2/R = (omega^2)R towards the direction of the circle.
            //The "UnphysicalBankAngle" is added to the calculated bank angle because the calculated bank angle was "too small;" it looks
            //better with a larger bank angle, even though it isn't physically correct
            return Quaternion.LookRotation(-new Vector3((float) -Math.Sin(omega * (Time.time - startTime) + phase), 0, -(float)Math.Cos(omega * (Time.time - startTime) + phase)), Vector3.up) * Quaternion.AngleAxis((float) (Math.Atan((r * Math.Pow(omega, 2)) / GlobalManager.gravityMag) * 180 / Math.PI + GlobalManager.unphysicalBankAngle), Vector3.forward);
        }

        public override void UpdateOnMapMoved(Vector3 movementVector)
        {
            centerX += movementVector.x;
            centerZ += movementVector.z;
        }

        public override void UpdateOnZoomChanged(Transform relativeTransform, float currentZoomRatio, float absoluteZoomRatio)
        {
            height = CalculateYOnZoomChanged(relativeTransform, currentZoomRatio, height);

            r *= MapMovement.Instance.CurrentZoomRatio;
        }

        public override Vector3 GetCenter()
        {
            return new Vector3(centerX, height, centerZ);
        }
    }
}
