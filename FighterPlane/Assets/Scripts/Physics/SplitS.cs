using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Physics
{
    class SplitS : Maneuver
    {
        public SplitS(Vector3 initPosition, Quaternion initRotation, float omegaRoll, float omegaLoop, float r, float initialSpeed, float finalSpeed)
        {
            currentPosition = initPosition;
            currentRotation = initRotation;
            this.initRotation = initRotation;
            this.omegaRoll = omegaRoll;
            this.omegaLoop = omegaLoop;
            this.r = r;
            this.initialSpeed = initialSpeed;
            this.finalSpeed = finalSpeed;
            startTime = Time.time;
        }

        GameObject go = new GameObject();
        Vector3 currentPosition;
        Quaternion currentRotation;
        Quaternion initRotation;
        float omegaRoll;
        float omegaLoop;
        float r;
        float initialSpeed;
        float finalSpeed;
        float startTime;
        int stage = 0;// there are three "stages": the 180 roll, the half loop, and the flight out of the loop

        public override Vector3 CalculateWorldPosition()
        {
            if (stage == 0)
            {
                currentPosition -= initialSpeed * (currentRotation * Vector3.forward) * (Time.deltaTime);
            }
            if(stage == 1)
            {

            }
            return currentPosition;
        }

        public override Quaternion CalculateWorldRotation()
        {
            if(stage == 0)
            {
                currentRotation = currentRotation * Quaternion.AngleAxis(omegaRoll * (Time.deltaTime) * 180f / (float)Math.PI, initRotation * Vector3.right);
                if (omegaRoll * (Time.time - startTime) >= Math.PI)
                {
                    stage = 1;
                    startTime = Time.time;
                }
            }
            return currentRotation;
        }
    }
}
