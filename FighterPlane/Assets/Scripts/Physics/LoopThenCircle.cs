using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class LoopThenCircle : Maneuver
    {
        public LoopThenCircle(Vector3 currentPos, Vector3 currentForward, float loopOmega = 0.5f, float loopR = 2, float circleOmega = 0.5f, float circleR = 2)
        {

            currentManeuver = new DoLoop(currentPos, currentForward, loopOmega, loopR);
            startTime = Time.time;
            this.loopOmega = loopOmega;
            this.circleOmega = circleOmega;
            this.circleR = circleR;
        }

        float circleOmega;
        float circleR;
        float loopOmega;
        Maneuver currentManeuver;
        float startTime;

        bool stillInLoop = true;

        public override Vector3 newPos()
        {
            if (stillInLoop && loopOmega * (Time.time - startTime) >= 2 * Math.PI)
            {
                currentManeuver = new MakeCircle(currentManeuver.newPos(), currentManeuver.newRot() * Vector3.right, circleOmega, circleR);
                stillInLoop = false;
            }
            return currentManeuver.newPos();
        }

        public override Quaternion newRot()
        {
            return currentManeuver.newRot();
        }
    }
}
