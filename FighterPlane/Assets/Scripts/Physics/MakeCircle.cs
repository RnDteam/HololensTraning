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

        public MakeCircle(Vector3 currentPosition, Vector3 currentRight, float omega = 0.5f, float r = 2)
        {
            centerX = currentPosition.x - r * Vector3.Dot(currentRight, Vector3.right);
            height = currentPosition.y;
            centerZ = currentPosition.z + r * Vector3.Dot(currentRight, Vector3.back);
            this.omega = omega;
            this.r = r;
            startTime = Time.time;
            phase = (float) Math.Atan2(Vector3.Dot(currentRight, Vector3.back), Vector3.Dot(currentRight, Vector3.right));
        }

        public float phase = 0;
        public float centerX;
        public float height;
        public float centerZ;
        public float omega;
        public float r;
        float startTime;

        public override Vector3 newPos()
        {
            return new Vector3(r * (float)Math.Cos(omega * (Time.time - startTime) + phase) + centerX, height, -r * (float)Math.Sin(omega * (Time.time - startTime) + phase) + centerZ);
        }

        public override Quaternion newRot()
        {
            //TODO: when we have the whole "physics" class, calculate the bank angle (currently 30) based upon r and omega
            return Quaternion.LookRotation(new Vector3((float)Math.Sin(omega * (Time.time - startTime) + phase), 0, (float)Math.Cos(omega * (Time.time - startTime) + phase)), Vector3.up) * Quaternion.AngleAxis(30, Vector3.forward);
        }
    }
}
