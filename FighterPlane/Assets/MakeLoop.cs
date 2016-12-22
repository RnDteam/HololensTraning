using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class MakeLoop : Maneuver
    {
        public MakeLoop(float centerX = 0, float centerY = 1, float centerZ = 0, float omega = 0.5f, float r = 2)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            this.centerZ = centerZ;
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
        }
        GameObject go = new GameObject();
        float centerX;
        float centerY;
        float centerZ;
        float omega;
        float r;
        float startTime;

        public override Vector3 newPos()
        {
            return new Vector3(r * (float)Math.Cos(omega * (Time.time - startTime)) + centerX, r * (float)Math.Sin(omega * (Time.time - startTime)) + centerY, centerZ);
        }

        public override Quaternion newRot()
        {
            //TODO: when we have the whole "physics" class, calculate the bank angle (currently 30) based upon r and omega
            go.transform.rotation = Quaternion.AngleAxis(270, Vector3.up) * Quaternion.AngleAxis(omega * (Time.time - startTime) * 180f / (float)Math.PI + 270, Vector3.right);
            return go.transform.rotation;
        }
    }
}