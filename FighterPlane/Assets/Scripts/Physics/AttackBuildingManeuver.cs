using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class AttackBuildingManeuver : Maneuver
    {
        const float permissibleAngleErrorDegrees = 1f;
        //in the future, we can add different radii and omegas for the different stages of the attack; in the meantime, we'll just use one set for simplicity
        public AttackBuildingManeuver(Vector3 currentPosition, Quaternion currentRotation, Vector3 CoordsToAttack, GameObject building, float flightSpeed = GlobalManager.defaultAttackSpeed, float radius = GlobalManager.defaultCircleRadius, float omega = GlobalManager.defaultCircleOmega)
        {
            AttackCoords = CoordsToAttack;
            AttackCoords.y = AttackCoords.y + GlobalManager.heightAboveBuildingToAttack;
            this.flightSpeed = flightSpeed;
            this.radius = radius;
            this.omega = omega;
            executedManeuver = new MakeCircle(currentPosition, currentRotation, omega, radius);
            this.building = building;
            MapCommands.Instance.LockMap();
        }

        Vector3 AttackCoords;
        Vector3 finalCoords = new Vector3();
        Maneuver executedManeuver;
        float flightSpeed;
        float radius;
        float omega;
        Vector3 initialPosition;
        Vector3 initialRight;
        GameObject building;
        //let's divide the attack up into five stages: the initial circle, the straight flight to the target, the circle segment above the target,
        //the straight flight back to the area the plane was in at the beginning, and the final circle. It's not strictly necessary,
        //but less messy than things like "if(executedManeuver is MakeCircle && ...)"
        int stage = 0;

        /*
         * Return the Vector3 representing the endpoint of the attack path - that is, the coordinates we are attacking
         * */
        public Vector3 GetEndpointOfAttackPath()
        {
            return AttackCoords;
        }

        /*
         * Return the Vector3 representing the start of the attack path - the coordinates where the plane leaves the initial circle
         * */
        public Vector3 GetStartPointOfAttackPath()
        {
            //Threw the relevant equations, into wolfram alpha; it turns out that the resulting mathematical solution is surprisingly copmplicated.
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

        public override void UpdateState()
        {
            executedManeuver.UpdateState();
            Vector3 position = executedManeuver.CalculateWorldPosition();
            Quaternion rotation = executedManeuver.CalculateWorldRotation();
            if (stage == 0 && Vector3.Angle(rotation * Vector3.forward, position - new Vector3(AttackCoords.x, position.y, AttackCoords.z)) < permissibleAngleErrorDegrees)
            {
                stage = 1;
                Vector3 planesRight = rotation * Vector3.right;
                planesRight.y = 0;
                planesRight.Normalize();
                finalCoords = position - 2 * radius * planesRight;
                //the "forward" vector in the LookRotation call is multpiplied by -1 because the forward vector of the Hercules model is towards its tail
                executedManeuver = new StraightFlightManeuver(position, AttackCoords, flightSpeed, rotation);
            }
            if(stage == 1 && ((StraightFlightManeuver) executedManeuver).finished)
            {
                stage = 2;
                building.GetComponent<BuildingDisplay>().BoomBuilding();
                executedManeuver = new MakeCircle(position, rotation, omega, radius);
            }
            if(stage == 2 && Vector3.Angle(rotation * Vector3.forward, position - new Vector3(finalCoords.x, position.y, finalCoords.z)) < permissibleAngleErrorDegrees)
            {
                stage = 3;
                executedManeuver = new StraightFlightManeuver(position, finalCoords, flightSpeed, rotation);
            }
            if(stage == 3 && ((StraightFlightManeuver) executedManeuver).finished)
            {
                stage = 4;
                MapCommands.Instance.UnlockMap();
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

        public override Vector3 GetCenter()
        {
            return executedManeuver.GetCenter();
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
