using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsManager {

    const double gravityMag = 9.8;
    const double hercArea = 162.1;
    const double hercMass = 34400;

    private static double CalculateAirPressure(Transform transform)
    {
        return 101325 * Math.Exp((-gravityMag * 0.0289644 * transform.position.y / (8.3144598 * 273.6)));
    }

    public static PhysicsParameters CalculateFlightParameters(Vector3 accelaration,
                                          Vector3 velocity,
                                          Transform transform,
                                          Vector3 prevPosition,
                                          Vector3 prevVelocity)
    {
        PhysicsParameters pParams = new PhysicsParameters();
        pParams.Rotation = transform.rotation;


        pParams.Speed = velocity.magnitude;
        pParams.AngleOfAttack = transform.rotation.x;
        if (pParams.Speed == 0)
        {
            pParams.AngleOfAscent = 0;
        }
        else
        {
            pParams.AngleOfAscent = Math.Asin(velocity.y / velocity.magnitude);
        }
        pParams.ParasiticDrag = 0.5 * CalculateAirPressure(transform) * Math.Pow(pParams.Speed, 2) * 0.4 * 162.1;
        pParams.Lift = Math.Cos(pParams.AngleOfAttack) * (hercMass * (accelaration.y + gravityMag) + pParams.ParasiticDrag * Math.Sin(pParams.AngleOfAscent)) - Math.Sin(pParams.AngleOfAttack) * (hercMass * accelaration.x + pParams.ParasiticDrag * Math.Cos(pParams.AngleOfAscent));
        pParams.InducedDrag = pParams.Lift * Math.Sin(pParams.AngleOfAttack) * Math.Cos(pParams.AngleOfAscent);
        pParams.TotalDrag = pParams.InducedDrag + pParams.ParasiticDrag;
        pParams.Thrust = Math.Sin(pParams.AngleOfAttack) * (hercMass * (accelaration.y + gravityMag) + pParams.ParasiticDrag * Math.Sin(pParams.AngleOfAscent)) + Math.Cos(pParams.AngleOfAttack) * (hercMass * Math.Sqrt(Math.Pow(accelaration.x, 2) + Math.Pow(accelaration.z, 2)) + pParams.ParasiticDrag * Math.Cos(pParams.AngleOfAscent));

        return pParams;
    }
}