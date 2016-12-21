using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
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
        }
        GameObject go = new GameObject();
        float centerX;
        float height;
        float centerZ;
        float omega;
        float r;

        public override Vector3 newPos() {
            return new Vector3((float)(r * Math.Cos(omega * (Time.timeSinceLevelLoad)) + centerX), height, (float)(r * Math.Sin(omega * (Time.timeSinceLevelLoad)) + centerZ));
        }

        public override Quaternion newRot() {
            //TODO: when we have the whole "physics" class, calculate the bank angle (currently 30) based upon r and omega
            go.transform.rotation = Quaternion.AngleAxis(-omega * Time.timeSinceLevelLoad * 180f / (float)Math.PI + 180, Vector3.up) * Quaternion.AngleAxis(-30, Vector3.forward);
            return go.transform.rotation;
        }
    }
}
