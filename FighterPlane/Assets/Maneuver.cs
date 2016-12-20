using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    abstract class Maneuver
    {
        public abstract Vector3 newPos();
        public abstract Quaternion newRot();
    }
}
