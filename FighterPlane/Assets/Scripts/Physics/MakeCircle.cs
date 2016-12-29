using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class MakeCircle : Maneuver
    {
        public MakeCircle(float centerX = 0, float height = 1, float centerZ = 0, float omega = 0.5f, float r = 2)
        {
            this.centerX = centerX;
            this.height = height;
            this.centerZ = centerZ;
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
        }

        public MakeCircle(Vector3 currentPosition, float omega = 0.5f, float r = 2)
        {
            centerX = currentPosition.x - r;
            height = currentPosition.y;
            centerZ = currentPosition.z;
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
        }

        GameObject go = new GameObject();
        float centerX;
        float height;
        float centerZ;
        float omega;
        float r;
        float startTime;

        public override Vector3 newPos()
        {
            return new Vector3(r * (float)Math.Cos(omega * (Time.time - startTime)) + centerX, height, r * (float)Math.Sin(omega * (Time.time - startTime)) + centerZ);
        }

        public override Quaternion newRot()
        {
            //TODO: when we have the whole "physics" class, calculate the bank angle (currently 30) based upon r and omega
            go.transform.rotation = Quaternion.AngleAxis(-omega * (Time.time - startTime) * 180f / (float)Math.PI + 180, Vector3.up) * Quaternion.AngleAxis(-30, Vector3.forward);
            return go.transform.rotation;
        }
    }
}
