using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class LoopThenCircle : Maneuver
    {
        public LoopThenCircle(Vector3 currentPos, float loopOmega = 0.5f, float loopR = 2, float circleOmega = 0.5f, float circleR = 2, bool insideLoop = true)
        {
            loop = new DoLoop(currentPos, loopOmega, loopR, insideLoop);
            startTime = Time.time;
            this.circleOmega = circleOmega;
            this.circleR = circleR;
        }

        float circleOmega;
        float circleR;
        DoLoop loop;
        MakeCircle circle;
        float startTime;

        bool stillInLoop = true;

        public override Vector3 newPos()
        {
            if (stillInLoop)
            {
                if(loop.omega * (Time.time - startTime) >= 2 * Math.PI)
                {
                    stillInLoop = false;
                    circle = new MakeCircle(loop.newPos(), circleOmega, circleR);
                }
                return loop.newPos();
            }
            return circle.newPos();
        }

        public override Quaternion newRot()
        {
            if (stillInLoop)
            {
                return loop.newRot();
            }
            return circle.newRot();
        }
    }
}
