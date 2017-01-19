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
        public abstract Vector3 CalculateWorldPosition();
        public abstract Quaternion CalculateWorldRotation();
        public virtual void UpdateState() { return; }
        public abstract void UpdateOnMapMoved(Vector3 movementVector);
        public abstract void UpdateOnZoomChanged(Transform relativeTransform, float currentZoomRatio, float absoluteZoomRatio);
        public abstract Vector3 GetFocusPoint();
        public abstract void Pause();
        public abstract void Resume();
        public static float CalculateYOnZoomChanged(Transform relativeTransform, float currentZoomRatio, float y)
        {
            var heightRelativeToSurface = relativeTransform.InverseTransformPoint(new Vector3 { y = y }).y;
            heightRelativeToSurface *= currentZoomRatio;
            return relativeTransform.TransformPoint(new Vector3 { y = heightRelativeToSurface }).y;
        }
    }
}
