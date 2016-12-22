using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsManager {

    const double gravityMag = 9.8;
    const double hercArea = 162.1;
    const double hercMass = 34400;

    private static double CalculateAirPressure(Vector3 position)
    {
        return 101325 * Math.Exp((-gravityMag * 0.0289644 * position.y / (8.3144598 * 273.6)));
    }

    public static void CalculateFlightParameters(PhysicsParameters pParams)
    {
        pParams.Speed = pParams.Velocity.magnitude;
        pParams.AngleOfAttack = pParams.Rotation.x;

        if (pParams.Speed == 0)
        {
            pParams.AngleOfAscent = 0;
        }
        else
        {
            pParams.AngleOfAscent = Math.Asin(pParams.Velocity.y / pParams.Velocity.magnitude);
        }
        pParams.ParasiticDrag = 0.5 * CalculateAirPressure(pParams.Position) * Math.Pow(pParams.Speed, 2) * 0.4 * 162.1;
        pParams.Lift = Math.Cos(pParams.AngleOfAttack) * (hercMass * (pParams.Accelaration.y + gravityMag) + pParams.ParasiticDrag * Math.Sin(pParams.AngleOfAscent)) - Math.Sin(pParams.AngleOfAttack) * (hercMass * pParams.Accelaration.x + pParams.ParasiticDrag * Math.Cos(pParams.AngleOfAscent));
        pParams.InducedDrag = pParams.Lift * Math.Sin(pParams.AngleOfAttack) * Math.Cos(pParams.AngleOfAscent);
        pParams.TotalDrag = pParams.InducedDrag + pParams.ParasiticDrag;
        pParams.Thrust = Math.Sin(pParams.AngleOfAttack) * (hercMass * (pParams.Accelaration.y + gravityMag) + pParams.ParasiticDrag * Math.Sin(pParams.AngleOfAscent)) + Math.Cos(pParams.AngleOfAttack) * (hercMass * Math.Sqrt(Math.Pow(pParams.Accelaration.x, 2) + Math.Pow(pParams.Accelaration.z, 2)) + pParams.ParasiticDrag * Math.Cos(pParams.AngleOfAscent));
    }
}