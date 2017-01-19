using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class AttackBuildingManeuver : Maneuver
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

        //in the future, we can add different radii and omegas for the different stages of the attack; in the meantime, we'll just use one set for simplicity
        public AttackBuildingManeuver(Vector3 currentPosition, Quaternion currentRotation, Vector3 CoordsToAttack, GameObject building, float flightSpeed = GlobalManager.defaultAttackSpeed, float radius = GlobalManager.defaultCircleRadius, float omega = GlobalManager.defaultCircleOmega)
        {
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
            this.building = building;
            MapCommands.Instance.LockMap();
        }

        /*
        public Vector3 GetEndpointOfAttackPath()
        {
            return AttackCoords;
        }

        public Vector3 GetStartPointOfAttackPath()
        {
            //Threw the relevant equations, into wolfram alpha; it turns out that the resulting mathematical solution is surprisingly complicated.
            //Therefore, we will calculate this point computationally
            float increment = Time.fixedDeltaTime * radius;
            for(float theta = 0; theta < 2 * Math.PI; theta += increment)
            {
                Vector3 position = new Vector3(radius * (float)Math.Cos(theta) + initialPosition.x - radius * Vector3.Dot(initialRight, Vector3.right), initialPosition.y, -radius * (float)Math.Sin(theta) + initialPosition.z + radius * Vector3.Dot(initialRight, Vector3.back));
                Quaternion rotation = Quaternion.LookRotation(-new Vector3((float)-Math.Sin(theta), 0, -(float)Math.Cos(theta)), Vector3.up) * Quaternion.AngleAxis((float)(Math.Atan((radius * Math.Pow(omega, 2)) / GlobalManager.gravityMag) * 180 / Math.PI + GlobalManager.unphysicalBankAngle), Vector3.forward);
                if (stage == 0 && Vector3.Angle(rotation * Vector3.forward, position - new Vector3(AttackCoords.x, position.y, AttackCoords.z)) < permissibleAngleErrorDegrees)
                {
                    return position;
                }
            }
            //if we throw this, than it means that there is no point where the plane leaves the initial circle
            //this can happen if an attempt is made to attack a point inside of the circle
            throw (new ArgumentOutOfRangeException());
        }
        */

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
                try
                {
                    building.GetComponent<BuildingDisplay>().BoomBuilding();
                }
                catch { }
                MapCommands.Instance.UnlockMap();
                executedManeuver = new MakeCircle(position, rotation, omega, radius);
                //Vector3[] pos = { new Vector3() };
                //line.SetPositions(pos);
                line.SetActive(false);
            }
            if(stage == (int)stagesOfAttack.circleSegmentAboveTarget && Vector3.Angle(rotation * Vector3.forward, position - new Vector3(finalCoords.x, position.y, finalCoords.z)) < permissibleAngleErrorDegrees)
            {
                stage = (int)stagesOfAttack.straightFlightBackToCircle;
                executedManeuver = new StraightFlightManeuver(position, finalCoords, flightSpeed, rotation);
            }
            if(stage == (int)stagesOfAttack.straightFlightBackToCircle && ((StraightFlightManeuver) executedManeuver).finished)
            {
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
