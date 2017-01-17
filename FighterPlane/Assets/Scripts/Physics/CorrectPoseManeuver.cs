using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class CorrectPoseManeuver : Maneuver
    {
        //fixes the pose of the plane. Does not calculate position.
        public CorrectPoseManeuver(Quaternion initialRotation,  float finalRoll, float finalPitch, float finalYaw, float totalTime = GlobalManager.timeToCorrectPose)
        {

        }

        Quaternion initialRotation;
        Quaternion finalRotation

        public override Vector3 CalculateWorldPosition()
        {
            throw new NotImplementedException();
        }

        public override Quaternion CalculateWorldRotation()
        {
            throw new NotImplementedException();
        }

        public override Vector3 GetCenter()
        {
            throw new NotImplementedException();
        }

        public override void UpdateOnMapMoved(Vector3 movementVector)
        {
            throw new NotImplementedException();
        }

        public override void UpdateOnZoomChanged(Transform relativeTransform, float currentZoomRatio, float absoluteZoomRatio)
        {
            throw new NotImplementedException();
        }
    }
}
