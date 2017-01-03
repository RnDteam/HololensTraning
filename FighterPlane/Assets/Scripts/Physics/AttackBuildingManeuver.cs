using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class AttackBuildingManeuver : Maneuver
    {

        public AttackBuildingManeuver(Vector3 currentPosition, Vector2 CoordsToAttack, float flightSpeed = GlobalManager.defaultAttackSpeed, float attackRadius = GlobalManager.defaultCircleRadius, float returnToCircleRadius = GlobalManager.defaultCircleRadius, float attackOmega = GlobalManager.defaultCircleOmega, float returnToCircleOmega = GlobalManager.defaultCircleOmega)
        {
            //TODO change 10 to the height of the building
            AttackCoords = new Vector3(CoordsToAttack.x, 10, CoordsToAttack.y);
            initialCoords = currentPosition;
            this.flightSpeed = flightSpeed;
            startTime = Time.time;
        }

        Vector3 AttackCoords;
        Vector3 initialCoords;
        float flightSpeed;
        float startTime;

        private float NormalizedTime()
        {
            return flightSpeed * (Time.time - startTime) / (AttackCoords - initialCoords).magnitude;
        }

        private Vector3 CalculateStraightLinePath(Vector3 startPoint, Vector3 endPoint)
        {

        }

        public override void UpdateState()
        {
            
        }

        public override Vector3 CalculateWorldPosition()
        {
            throw new NotImplementedException();
        }

        public override Quaternion CalculateWorldRotation()
        {
            throw new NotImplementedException();
        }
    }
}
