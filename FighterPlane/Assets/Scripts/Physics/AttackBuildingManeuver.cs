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
        public AttackBuildingManeuver(Vector3 currentPosition, Vector3 currentRight, Vector3 CoordsToAttack, float flightSpeed = GlobalManager.defaultAttackSpeed, float radius = GlobalManager.defaultCircleRadius, float omega = GlobalManager.defaultCircleOmega)
        {
            AttackCoords = CoordsToAttack;
            AttackCoords.y = AttackCoords.y + GlobalManager.heightAboveBuildingToAttack;
            this.flightSpeed = flightSpeed;
            initialCoords = currentPosition;
            this.radius = radius;
            this.omega = omega;
            startTime = Time.time;
            executedManeuver = new MakeCircle(currentPosition, currentRight, omega, radius);
        }

        Vector3 AttackCoords;
        Vector3 initialCoords;
        Vector3 finalCoords;
        Maneuver executedManeuver;
        float flightSpeed;
        float startTime;
        float radius;
        float omega;
        //let's divide the attack up into five stages: the initial circle, the straight flight to the target, the circle segment above the target,
        //the straight flight back to the area the plane was in at the beginning, and the final circle. It's not strictly necessary,
        //but less messy than things like "if(executedManeuver is MakeCircle && ...)"
        int stage = 0; 

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
                executedManeuver = new StraightFlightManeuver(position, AttackCoords, flightSpeed, Quaternion.LookRotation(-(AttackCoords - position), Vector3.up));
            }
            if(stage == 1 && ((StraightFlightManeuver) executedManeuver).finished)
            {
                stage = 2;
                //BoomBuilding();
                executedManeuver = new MakeCircle(position, rotation * Vector3.right, omega, radius);
            }
            if(stage == 2 && Vector3.Angle(rotation * Vector3.forward, position - new Vector3(finalCoords.x, position.y, initialCoords.z)) < permissibleAngleErrorDegrees)
            {
                stage = 3;
                executedManeuver = new StraightFlightManeuver(position, finalCoords, flightSpeed, Quaternion.LookRotation(-(finalCoords - position), Vector3.up));
            }
            if(stage == 3 && ((StraightFlightManeuver) executedManeuver).finished)
            {
                stage = 4;
                executedManeuver = new MakeCircle(position, rotation * Vector3.right, omega, radius);
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
    }
}
