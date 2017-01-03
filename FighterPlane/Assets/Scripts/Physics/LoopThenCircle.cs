using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class LoopThenCircle : Maneuver
    {
        public LoopThenCircle(Vector3 currentPos, Vector3 currentForward, float loopOmega = GlobalManager.defaultLoopOmega, float loopRadius = GlobalManager.defaultLoopRadius, float circleOmega = GlobalManager.defaultCircleOmega, float circleRadius = GlobalManager.defaultCircleRadius)
        {
            canInterrupt = false;
            executedManeuver = new DoLoop(currentPos, currentForward, loopOmega, loopRadius);
            startTime = Time.time;
            this.loopOmega = loopOmega;
            this.circleOmega = circleOmega;
            this.circleRadius = circleRadius;
        }

        float circleOmega;
        float circleRadius;
        float loopOmega;
        Maneuver executedManeuver;
        float startTime;

        public override void UpdateState()
        {
            if (!canInterrupt && loopOmega * (Time.time - startTime) >= 2 * Math.PI)
            {
                executedManeuver = new MakeCircle(executedManeuver.UpdateWorldPosition(), executedManeuver.UpdateWorldRotation() * Vector3.right, circleOmega, circleRadius);
                canInterrupt = true;
            }
        }

        public override Vector3 UpdateWorldPosition()
        {
            return executedManeuver.UpdateWorldPosition();
        }

        public override Quaternion UpdateWorldRotation()
        {
            return executedManeuver.UpdateWorldRotation();
        }
    }
}
