using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    public delegate void StartStraightFlight();
    public delegate void FinishedStraightFlight();

    public class StandardManeuver : ATCManeuver
    {
        public const float permissibleAngleErrorDegrees = 1f;
        Vector3 DestinationCoords, newDestinationCoords;
        Vector3 finalCoords = new Vector3();
        Maneuver executedManeuver;
        float flightSpeed;
        float radius;
        float omega;
        Vector3 initialPosition;
        Vector3 initialRight;
        GameObject building;
        GameObject line;
        enum FlightStage
        {
            initialCircle, straightFlightToDestination, circleSegmentAboveDestination, straightFlightBackToCircle
        };
        FlightStage stage = FlightStage.initialCircle;


        #region events
        public event StartStraightFlight StartStraightFlight;
        public event FinishedStraightFlight FinishedStraightFlight;

        protected virtual void OnStartStraightFlight()
        {
            if (StartStraightFlight != null)
            {
                StartStraightFlight();
            }
        }

        protected virtual void OnFinishedStraightFlight()
        {
            if (FinishedStraightFlight != null)
            {
                FinishedStraightFlight();
            }
        }
        #endregion


        //in the future, we can add different radii and omegas for the different stages of the attack; in the meantime, we'll just use one set for simplicity
        public StandardManeuver(Vector3 currentPosition, Quaternion currentRotation, Vector3 destCoords, float flightSpeed = GlobalManager.defaultAttackSpeed, float radius = GlobalManager.defaultCircleRadius, float omega = GlobalManager.defaultCircleOmega)
        {
            DestinationCoords = newDestinationCoords = destCoords;
            initialPosition = currentPosition;
            initialRight = currentRotation * Vector3.right;
            this.flightSpeed = flightSpeed;
            this.radius = radius;
            this.omega = omega;
            executedManeuver = new MakeCircle(currentPosition, currentRotation, omega, radius);
            initialPosition = currentPosition;
            initialRight = currentRotation * Vector3.right;
            MapCommands.Instance.LockMap();
        }

        public override void Pause()
        {
            executedManeuver.Pause();
        }

        public override void Resume()
        {
            executedManeuver.Resume();
        }

        public void SetLine(GameObject line)
        {
            this.line = line;
        }

        public override Vector3 GetEndpoint()
        {
            return DestinationCoords;
        }

        public override void UpdateState()
        {
            executedManeuver.UpdateState();
            Vector3 position = executedManeuver.CalculateWorldPosition();
            Quaternion rotation = executedManeuver.CalculateWorldRotation();
            if (stage == FlightStage.initialCircle && Vector3.Angle(rotation * Vector3.forward, position - new Vector3(DestinationCoords.x, position.y, DestinationCoords.z)) < permissibleAngleErrorDegrees)
            {
                if (DestinationCoords != newDestinationCoords)
                {
                    DestinationCoords = newDestinationCoords;
                }
                stage = FlightStage.straightFlightToDestination;
                Vector3 planesRight = rotation * Vector3.right;
                planesRight.y = 0;
                planesRight.Normalize();
                finalCoords = position - 2 * radius * planesRight;
                //the "forward" vector in the LookRotation call is multpiplied by -1 because the forward vector of the Hercules model is towards its tail
                OnStartStraightFlight();
                executedManeuver = new StraightFlightManeuver(position, DestinationCoords, flightSpeed, rotation);
            }
            if (stage == FlightStage.straightFlightToDestination && ((StraightFlightManeuver)executedManeuver).finished)
            {
                stage = FlightStage.circleSegmentAboveDestination;
                MapCommands.Instance.UnlockMap();
                OnFinishedStraightFlight();
                executedManeuver = new MakeCircle(position, rotation, omega, radius);
                if (line != null)
                    line.SetActive(false);
            }
            if(stage == FlightStage.circleSegmentAboveDestination && Vector3.Angle(rotation * Vector3.forward, position - new Vector3(finalCoords.x, position.y, finalCoords.z)) < permissibleAngleErrorDegrees)
            {
                stage = FlightStage.straightFlightBackToCircle;
                OnStartStraightFlight();
                executedManeuver = new StraightFlightManeuver(position, finalCoords, flightSpeed, rotation);
            }
            if (stage == FlightStage.straightFlightBackToCircle && ((StraightFlightManeuver)executedManeuver).finished)
            {
                stage = FlightStage.initialCircle;
                OnFinishedStraightFlight();
                executedManeuver = new MakeCircle(position, rotation, omega, radius);
            }
        }

        public void SetEndPoint(Vector3 newDestination)
        {
            newDestinationCoords = newDestination;
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
            DestinationCoords += movementVector;
            finalCoords += movementVector;
            executedManeuver.UpdateOnMapMoved(movementVector);
        }

        public override void UpdateOnZoomChanged(Transform relativeTransform, float currentZoomRatio, float absoluteZoomRatio)
        {
            DestinationCoords.y = CalculateYOnZoomChanged(relativeTransform, currentZoomRatio, DestinationCoords.y);
            finalCoords.y = CalculateYOnZoomChanged(relativeTransform, currentZoomRatio, finalCoords.y);
            executedManeuver.UpdateOnZoomChanged(relativeTransform, currentZoomRatio, absoluteZoomRatio);
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
