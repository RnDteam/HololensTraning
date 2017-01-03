using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    public class DoLoop : Maneuver
    {
        public DoLoop(float centerX = 0, float centerY = 1, float z = 0, float omega = 0.5f, float r = 2)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            centerZ = z;
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
        }

        public DoLoop(Vector3 currentPosition, Vector3 currentForward, float omega = 0.5f, float r = 2)
        {
            centerX = currentPosition.x;
            centerY = currentPosition.y + r;
            centerZ = currentPosition.z;
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
            zComponentOfHorizontal = Vector3.Dot(-currentForward, Vector3.forward);
            xComponentOfHorizontal = Vector3.Dot(-currentForward, Vector3.right);
        }

        public float centerX;
        public float centerY;
        public float centerZ;
        public float omega;
        public float r;
        float startTime;
        bool insideLoop;
        private float zComponentOfHorizontal = 1;
        private float xComponentOfHorizontal = 0;
        private float phase = (float) -Math.PI/2;

        public override Vector3 newPos()
        {
            return new Vector3(xComponentOfHorizontal * r * (float)Math.Cos(omega * (Time.time - startTime) + phase) + centerX, r * (float)Math.Sin(omega * (Time.time - startTime) + phase) + centerY, zComponentOfHorizontal * r * (float)Math.Cos(omega * (Time.time - startTime) + phase) + centerZ);
        }

        public override Quaternion newRot()
        {
            //the minus sign on the "forward" vector is because our herculese model's "forward" vector is towards the tail; I don't understand why the "upwards" vector is AWAY from the center of the circle, but that is the only way to make it work
            return Quaternion.LookRotation(-new Vector3((float)(-xComponentOfHorizontal * Math.Sin(omega * (Time.time - startTime) + phase)), (float)Math.Cos(omega * (Time.time - startTime) + phase), (float)(-zComponentOfHorizontal * Math.Sin(omega * (Time.time - startTime) + phase))), -newPos() + new Vector3(centerX, centerY, centerZ));
        }
    }
}
