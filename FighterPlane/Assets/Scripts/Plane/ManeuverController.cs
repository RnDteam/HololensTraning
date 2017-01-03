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
        public bool hasManeuvered = false;

        public int SetManeuver(Maneuver man)
        {
            hasManeuvered = true;
            if (maneuver == null || maneuver.canInterrupt)
            {
                maneuver = man;
                return 0;
            }
            return 1;
        }

        void Update()
        {
            if(maneuver != null)
            {
                maneuver.UpdateState();
                gameObject.transform.position = maneuver.UpdateWorldPosition();
                gameObject.transform.rotation = maneuver.UpdateWorldRotation();
            }
        }
    }
}
