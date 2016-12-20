using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyCSharpWSA
{
    abstract class Maneuver
    {
        public abstract Vector3 newPos();
        public abstract Quaternion newRot();
    }
}
