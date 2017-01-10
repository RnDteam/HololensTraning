using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class BeginFlightManeuver : MakeCircle
    {
        public BeginFlightManeuver(Vector3 currentPosition, Vector3 currentRight, float omega = GlobalManager.defaultCircleOmega, float r = GlobalManager.defaultCircleRadius) : base(currentPosition, currentRight, omega, r)
        {
        }
    }
}
