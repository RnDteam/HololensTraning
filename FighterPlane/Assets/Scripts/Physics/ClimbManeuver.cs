using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class ClimbManeuver : ATCManeuver
    {
        Maneuver executedManeuver;
        Maneuver finalManeuver;
        Vector3 endpoint;
        float loopOmega;
        float flightSpeed;
        float radius;
        float omega;
        bool isClimbing = true;
        
        public ClimbManeuver(ATCManeuver currentManeuver, float height, float loopOmega = GlobalManager.defaultLoopOmega)
        {
            flightSpeed = currentManeuver.GetFlightSpeed();
            radius = currentManeuver.GetRadius();
            omega = currentManeuver.GetOmega();
            endpoint = currentManeuver.GetEndpoint();
            endpoint.y += height;
            executedManeuver = new DoLoop(currentManeuver.CalculateWorldPosition(), currentManeuver.CalculateWorldRotation(), loopOmega, 2 * height);
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
            Debug.Log(executedManeuver.CalculateWorldPosition() + " " + executedManeuver.CalculateWorldRotation() + " " + endpoint + " " + flightSpeed + " " + radius + " " + omega);
            if (isClimbing && Vector3.Dot(executedManeuver.CalculateWorldRotation() * Vector3.forward, Vector3.down) >= Math.Sqrt(2)/2)
            {
                isClimbing = false;
                executedManeuver = new StandardManeuver(executedManeuver.CalculateWorldPosition(), executedManeuver.CalculateWorldRotation(), endpoint, flightSpeed, radius, omega);
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
            endpoint.y = CalculateYOnZoomChanged(relativeTransform, currentZoomRatio, endpoint.y);
            executedManeuver.UpdateOnZoomChanged(relativeTransform, currentZoomRatio, absoluteZoomRatio);
        }

        public override Vector3 GetEndpoint()
        {
            return endpoint;
        }
        public override float GetFlightSpeed()
        {
            return flightSpeed;
        }

        public override float GetRadius()
        {
            return radius;
        }

        public override float GetOmega()
        {
            return omega;
        }
    }
}
