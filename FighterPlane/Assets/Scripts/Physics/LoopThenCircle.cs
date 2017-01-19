using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class LoopThenCircle : Maneuver
    {
        public LoopThenCircle(Vector3 currentPos, Quaternion currentRotation, float loopOmega = GlobalManager.defaultLoopOmega, float loopRadius = GlobalManager.defaultLoopRadius, float circleOmega = GlobalManager.defaultCircleOmega, float circleRadius = GlobalManager.defaultCircleRadius)
        {
            canInterrupt = false;
            executedManeuver = new DoLoop(currentPos, currentRotation, loopOmega, loopRadius);
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
                executedManeuver = new MakeCircle(executedManeuver.CalculateWorldPosition(), executedManeuver.CalculateWorldRotation(), circleOmega, circleRadius);
                canInterrupt = true;
            }
        }

        public override Vector3 CalculateWorldPosition()
        {
            return executedManeuver.CalculateWorldPosition();
        }

        public override Quaternion CalculateWorldRotation()
        {
            return executedManeuver.CalculateWorldRotation();
        }

        public override void UpdateOnMapMoved(Vector3 movementVector)
        {
        }

        public override void UpdateOnZoomChanged(Transform relativeTransform, float currentZoomRatio, float absoluteZoomRatio)
        {
            executedManeuver.UpdateOnZoomChanged(relativeTransform, currentZoomRatio, absoluteZoomRatio);
            circleRadius *= currentZoomRatio;
        }

        public override Vector3 GetFocusPoint()
        {
            return executedManeuver.GetFocusPoint();
        }

        public override void Pause()
        {
            executedManeuver.Pause();
        }

        public override void Resume()
        {
            executedManeuver.Resume();
        }
    }
}
