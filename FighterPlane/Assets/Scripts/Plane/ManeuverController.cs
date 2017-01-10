﻿using Assets.Scripts.Physics;
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

        public int SetManeuver(Maneuver newManeuver)
        {
            canFly = canFly || newManeuver is BeginFlightManeuver;
            if (canFly && (maneuver == null || maneuver.canInterrupt) && !(hasBegunFlight && newManeuver is BeginFlightManeuver))
            {
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
