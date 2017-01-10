﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    public abstract class Maneuver
    {
        public bool canInterrupt = true;
        public abstract Vector3 CalculateWorldPosition();
        public abstract Quaternion CalculateWorldRotation();
        public virtual void UpdateState() { return; }
        public abstract void UpdateOnMapMoved();
        public abstract void UpdateOnZoomChanged();

        public Maneuver()
        {
            MapMovement.Instance.Moved += UpdateOnMapMoved;
            MapMovement.Instance.ZoomChanged += UpdateOnZoomChanged;
        }
    }
}
