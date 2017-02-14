using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class StandardManeuver : Maneuver
    {
        public const float permissibleAngleErrorDegrees = 1f;
        Vector3 AttackCoords;
        Vector3 finalCoords = new Vector3();
        Maneuver executedManeuver;
        float flightSpeed;
        float radius;
        float omega;
        Vector3 initialPosition;
        Vector3 initialRight;
        GameObject building;
        GameObject line;
        enum stagesOfAttack
        {
            initialAttackCircle, straightFlightToTarget, circleSegmentAboveTarget, straightFlightBackToCircle, finalCircle
        };
        int stage = (int) stagesOfAttack.initialAttackCircle;
        public AudioSource alphaOnTargetRecord;

        //in the future, we can add different radii and omegas for the different stages of the attack; in the meantime, we'll just use one set for simplicity
        public StandardManeuver(Vector3 currentPosition, Quaternion currentRotation, Vector3 CoordsToAttack, float flightSpeed = GlobalManager.defaultAttackSpeed, float radius = GlobalManager.defaultCircleRadius, float omega = GlobalManager.defaultCircleOmega)
        {
            alphaOnTargetRecord = GameObject.Find("AlphaOnTarget").GetComponent<AudioSource>();
            AttackCoords = CoordsToAttack;
            AttackCoords.y = AttackCoords.y + GlobalManager.heightAboveBuildingToAttack;
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

        public override void UpdateState()
        {
            executedManeuver.UpdateState();
            Vector3 position = executedManeuver.CalculateWorldPosition();
            Quaternion rotation = executedManeuver.CalculateWorldRotation();
            if (stage == (int) stagesOfAttack.initialAttackCircle && Vector3.Angle(rotation * Vector3.forward, position - new Vector3(AttackCoords.x, position.y, AttackCoords.z)) < permissibleAngleErrorDegrees)
            {
                stage = (int) stagesOfAttack.straightFlightToTarget;
                Vector3 planesRight = rotation * Vector3.right;
                planesRight.y = 0;
                planesRight.Normalize();
                finalCoords = position - 2 * radius * planesRight;
                //the "forward" vector in the LookRotation call is multpiplied by -1 because the forward vector of the Hercules model is towards its tail
                executedManeuver = new StraightFlightManeuver(position, AttackCoords, flightSpeed, rotation);
            }
            if(stage == (int)stagesOfAttack.straightFlightToTarget && ((StraightFlightManeuver) executedManeuver).finished)
            {
                stage = (int)stagesOfAttack.circleSegmentAboveTarget;
                MapCommands.Instance.UnlockMap();
                executedManeuver = new MakeCircle(position, rotation, omega, radius);
                line.SetActive(false);
            }
            if(stage == (int)stagesOfAttack.circleSegmentAboveTarget && Vector3.Angle(rotation * Vector3.forward, position - new Vector3(finalCoords.x, position.y, finalCoords.z)) < permissibleAngleErrorDegrees)
            {
                stage = (int)stagesOfAttack.straightFlightBackToCircle;
                executedManeuver = new StraightFlightManeuver(position, finalCoords, flightSpeed, rotation);
            }
            if(stage == (int)stagesOfAttack.straightFlightBackToCircle && ((StraightFlightManeuver) executedManeuver).finished)
            {
                alphaOnTargetRecord.Play();
                stage = (int)stagesOfAttack.finalCircle;
                executedManeuver = new MakeCircle(position, rotation, omega, radius);
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
            AttackCoords += movementVector;
            finalCoords += movementVector;
            executedManeuver.UpdateOnMapMoved(movementVector);
        }

        public override void UpdateOnZoomChanged(Transform relativeTransform, float currentZoomRatio, float absoluteZoomRatio)
        {
            AttackCoords.y = CalculateYOnZoomChanged(relativeTransform, currentZoomRatio, AttackCoords.y);
            finalCoords.y = CalculateYOnZoomChanged(relativeTransform, currentZoomRatio, finalCoords.y);
            executedManeuver.UpdateOnZoomChanged(relativeTransform, currentZoomRatio, absoluteZoomRatio);
        }
    }
}
