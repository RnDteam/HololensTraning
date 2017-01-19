using Assets.Scripts.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Plane
{
    class ManeuverController : MonoBehaviour
    {
        private Maneuver maneuver;
        private bool hasBegunFlight = false;
        private bool canFly = false;
        private Maneuver interruptedManeuver;
        private GameObject line;
        
        public Vector3 ManeuverCenter
        {
            get
            {
                return maneuver.GetFocusPoint();
            }
        }

        public Vector3 GetAttackEndPoint()
        {
            return OnlineMapsTileSetControl.instance.GetWorldPosition(BuildingManager.Instance.SelectedBuildingCoords) + GlobalManager.heightAboveBuildingToAttack * Vector3.up;
        }

        /*
         * Point where a plane would leave its initial circle and begin a straight flight towards a building,
         * were it to attack, assuming the plane is currently circling with the same radius as its "attack radius"
         * */
        public Vector3 GetAttackStartPoint()
        {
            float increment = Time.fixedDeltaTime * GlobalManager.defaultCircleRadius;
            Vector3 attackEndPoint = GetAttackEndPoint();
            Vector3 initPos = maneuver.CalculateWorldPosition();
            Vector3 initRight = maneuver.CalculateWorldRotation() * Vector3.right;
            float r = MapMovement.Instance.AbsoluteZoomRatio * GlobalManager.defaultCircleRadius;
            for (float theta = 0; theta < 2 * Math.PI; theta += increment)
            {
                Vector3 position = new Vector3(r * (float)Math.Cos(theta) + initPos.x - r * Vector3.Dot(initRight, Vector3.right), initPos.y, -r * (float)Math.Sin(theta) + initPos.z + r * Vector3.Dot(initRight, Vector3.back));
                Quaternion rotation = Quaternion.LookRotation(-new Vector3((float)-Math.Sin(theta), 0, -(float)Math.Cos(theta)), Vector3.up) * Quaternion.AngleAxis((float)(Math.Atan((r * Math.Pow(GlobalManager.defaultCircleOmega, 2)) / GlobalManager.gravityMag) * 180 / Math.PI + GlobalManager.unphysicalBankAngle), Vector3.forward);
                if (Vector3.Angle(rotation * Vector3.forward, position - new Vector3(attackEndPoint.x, position.y, attackEndPoint.z)) < AttackBuildingManeuver.permissibleAngleErrorDegrees)
                {
                    return position;
                }
            }
            //if we throw this, than it means that there is no point where the plane leaves the initial circle
            //this can happen if an attempt is made to attack a point inside of the circle
            throw (new ArgumentOutOfRangeException());
        }


        public Maneuver getManeuver()
        {
            return maneuver;
        }

        private void Start()
        {
            MapMovement.Instance.Moved += SetManeuverOnMapMoved;
            MapMovement.Instance.ZoomChanged += SetManeuverOnZoomChanged;
            line = GetComponentInChildren<LineRenderer>(true).gameObject;
        }

        private void SetManeuverOnMapMoved()
        {
            if (IsFlying)
            {
                maneuver.UpdateOnMapMoved(MapMovement.Instance.MovementVector);
            }
        }

        private void SetManeuverOnZoomChanged()
        {
            if (IsFlying)
            {
                maneuver.UpdateOnZoomChanged(transform.parent, MapMovement.Instance.CurrentZoomRatio, MapMovement.Instance.AbsoluteZoomRatio);
            }
        }

        public int SetManeuver(Maneuver newManeuver)
        {
            canFly = canFly || newManeuver is BeginFlightManeuver;
            if (canFly && (maneuver == null || maneuver.canInterrupt) && !(hasBegunFlight && newManeuver is BeginFlightManeuver))
            {
                if (newManeuver is AttackBuildingManeuver)
                {
                    ((AttackBuildingManeuver)newManeuver).SetLine(line);
                }
                if (newManeuver is LoopThenCircle)
                {
                    interruptedManeuver = maneuver;
                    maneuver.Pause();
                }
                hasBegunFlight = true;
                maneuver = newManeuver;
                return 0;
            }
            return 1;
        }

        void Update()
        {
            if(maneuver != null)
            {
                maneuver.UpdateState();
                gameObject.transform.position = maneuver.CalculateWorldPosition();
                gameObject.transform.rotation = maneuver.CalculateWorldRotation();
                if(maneuver is LoopThenCircle && maneuver.canInterrupt && !(interruptedManeuver is MakeCircle))
                {
                    maneuver = interruptedManeuver;
                    maneuver.Resume();
                }
            }
        }

        public bool IsFlying
        {
            get
            {
                return maneuver != null;
            }
        }
    }
}
