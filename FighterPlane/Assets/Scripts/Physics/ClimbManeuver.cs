using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class ClimbManeuver : StandardManeuver
    {
        Maneuver executedManeuver;
        Maneuver finalManeuver;
        Vector3 endpoint;
        float loopOmega;
        finalManeuver parameters...;
        bool isClimbing = true;
        fdbzdfbfdzb
        public ClimbManeuver(StandardManeuver currentManuever, float height, float loopOmega = GlobalManager.DefaultLoopOmega)
        {
            parameters = currentManeuver.getParameters()...
            endpoint = currentManeuver.getEndpoint();
            endpoint.y += height;
            executedManeuver = new DoLoop(currentManeuver.CalculateWorldPosition(), currentManeuver.CalculateWorldRotation(), height, loopOmega);
        }

        public Vector3 getEndpoint()
        {
            return endpoint;
        }

        public override void Pause()
        {
            executedManeuver.Pause();
        }

        public override void Resume()
        {
            executedManeuver.Resume();
        }

        public override void UpdateState()
        {
            if (isClimbing && Vector3.Dot(Vector3.forward * executedManeuver.CalculateWorldRotation(), Vector3.up) > 0.99)
            {
                isClimbing = false;
                executedManeuver = new StandardManeuver(executedManeuver.CalculateWorldPosition, executedManeuver.CalculateWorldRotation(), endpoint, otherParams...);
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

        public override Vector3 GetFocusPoint()
        {
            return executedManeuver.GetFocusPoint();
        }

        public override void UpdateOnMapMoved(Vector3 movementVector)
        {
            endpoint += movementVector;
            executedManeuver.UpdateOnMapMoved(movementVector);
        }

        public override void UpdateOnZoomChanged(Transform relativeTransform, float currentZoomRatio, float absoluteZoomRatio)
        {
            AttackCoords.y = CalculateYOnZoomChanged(relativeTransform, currentZoomRatio, AttackCoords.y);
            executedManeuver.UpdateOnZoomChanged(relativeTransform, currentZoomRatio, absoluteZoomRatio);
        }
    }
}
