using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Physics
{
    class MakeCircle : Maneuver
    {
        float omega = 0.5f;
        float r = 2;

        //planes[selectedPlaneIndex].transform.position = new Vector3(1, 1, 0);
        //planes[selectedPlaneIndex].transform.localRotation = Quaternion.Euler(0, (float)(-1*omega*Time.time), 0);
        public override Vector3 newPos() { return new Vector3((float)(r * Math.Cos(omega * (Time.timeSinceLevelLoad))), 1, (float)(r * Math.Sin(omega * (Time.timeSinceLevelLoad)))); }

        //planes[selectedPlaneIndex].transform.Rotate(Vector3.Reflect(Vector3.up * (float)(1*omega*Time.deltaTime), Vector3.up));
        public override Quaternion newRot()
        {
            GameObject go = new GameObject();
            go.transform.rotation = Quaternion.Euler(30, 0, 0);//TODO make this depend on omega, r
            go.transform.RotateAroundLocal(Vector3.up, (float)(-1 * omega * (Time.deltaTime)));
            return go.transform.rotation;
        }
    }
}
