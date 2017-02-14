using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GlobalManager {
    public static int TimeBetweenTakeOffToFindingTarget = 5;
    public static int GasThreshold = 25;
    public const double gravityMag = 9.8;
    public const float defaultLoopRadius = 0.3f;
    public const float defaultCircleRadius = 0.3f;
    public const float defaultLoopOmega = 0.33333f;
    public const float defaultCircleOmega = 0.33333f;
    public const float defaultAttackSpeed = 0.16667f;
    public const float unphysicalBankAngle = 20;
    public const float heightAboveBuildingToAttack = 0.3f;
    public const float timeToCorrectPose = 0.5f;

    public static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        return new string(charArray.Reverse().ToArray());
    }
}
