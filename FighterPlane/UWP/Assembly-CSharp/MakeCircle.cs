using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharpWSA
{
    class MakeCircle : Maneuver
    {
        float omega = 0.5f;
        float r = 2f;
        public override Vector3 newPos()
        {
            return new Vector3((float)(r*Math.Sin(omega*Time.timeSinceLevelLoad)), 1, (float)(r*Math.Sin(omega * Time.timeSinceLevelLoad)));
        }

        public override Quaternion newRot()
        {
            GameObject go = new GameObject();
            go.transform.rotation = Quaternion.Euler(30, 0, 0);
            go.transform.RotateAroundLocal(Vector3.up, -1*omega*Time.deltaTime);
            return go.transform.rotation;
        }
    }
}
