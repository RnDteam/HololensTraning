using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    public class DoLoop : Maneuver
    {
        public DoLoop(float centerX = 0, float centerY = 1, float z = 0, float omega = 0.5f, float r = 2, bool insideLoop = true)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.z = z;
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
            if (insideLoop)
            {
                loopOrientation = Quaternion.AngleAxis(180, Vector3.forward);
            }
        }

        public DoLoop(Vector3 currentPosition, float omega = 0.5f, float r = 2, bool insideLoop = true)
        {
            centerX = currentPosition.x - r;
            centerY = currentPosition.y;
            z = currentPosition.z;
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
            if (insideLoop)
            {
                loopOrientation = Quaternion.AngleAxis(180, Vector3.forward);
            }
        }

        GameObject go = new GameObject();
        float centerX;
        float centerY;
        float z;
        float omega;
        float r;
        float startTime;
        bool insideLoop;
        private Quaternion loopOrientation = Quaternion.identity;

        public override Vector3 newPos()
        {
            return new Vector3(r * (float)Math.Cos(omega * (Time.time - startTime)) + centerX, r * (float)Math.Sin(omega * (Time.time - startTime)) + centerY, z);
        }

        public override Quaternion newRot()
        {
            //go.transform.rotation = Quaternion.AngleAxis(-omega * (Time.time - startTime) * 180f / (float)Math.PI + 180, Vector3.up) * Quaternion.AngleAxis(-30, Vector3.forward);
            go.transform.rotation = Quaternion.AngleAxis(90, Vector3.up)  * Quaternion.AngleAxis(-omega * (Time.time - startTime) * 180f / (float)Math.PI + 90, Vector3.right) * loopOrientation;
            return go.transform.rotation;
        }
    }
}
