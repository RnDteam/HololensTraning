using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    public abstract class Maneuver
    {
        public bool canInterrupt = true;
        public abstract Vector3 UpdateWorldPosition();
        public abstract Quaternion UpdateWorldRotation();
        public virtual void UpdateState() { return; }
    }
}
