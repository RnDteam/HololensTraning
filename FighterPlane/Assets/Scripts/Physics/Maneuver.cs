using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    public abstract class Maneuver
    {
        public bool isEnabled = false;
        public abstract Vector3 newPos();
        public abstract Quaternion newRot();
    }
}
