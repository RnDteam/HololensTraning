using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class CorrectPoseManeuver
    {
        //fixes the pose of the plane
        public CorrectPoseManeuver(Quaternion initialRotation,  Quaternion finalRotation, float totalTime = GlobalManager.timeToCorrectPose)
        {
            this.initialRotation = initialRotation;
            this.finalRotation = finalRotation;
            this.totalTime = totalTime;
            startTime = Time.time;
        }

        Quaternion initialRotation;
        Quaternion finalRotation;
        float startTime;
        float totalTime;
        public Quaternion CalculateWorldRotation()
        {
            return Quaternion.Slerp(initialRotation, finalRotation, (Time.time - startTime) / totalTime);
        }
    }
}
