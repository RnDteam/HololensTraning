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
        
        public Vector3 ManeuverCenter
        {
            get
            {
                return maneuver.GetCenter();
            }
        }

        private void Start()
        {
            MapMovement.Instance.Moved += SetManeuverOnMapMoved;
            MapMovement.Instance.ZoomChanged += SetManeuverOnZoomChanged;
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
                if(newManeuver is LoopThenCircle)
                {
                    interruptedManeuver = maneuver;
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
