using UnityEngine;
using System.Collections;
using System;
using HoloToolkit.Unity;
using System.Collections.Generic;

namespace Assets
{
    class TempClass : MonoBehaviour
    {
        private static Dictionary<GameObject, Maneuver> planeBehavior = new Dictionary<GameObject, Maneuver>();
        public static void AddBehavior(GameObject plane, Maneuver behavior)
        {
            planeBehavior.Add(plane, behavior);
        }
        public static void RemoveBehavior(GameObject plane)
        {
            planeBehavior.Remove(plane);
        }

        //void Start() { }
       void Update()
        {
            foreach(GameObject plane in planeBehavior.Keys)
            {
                plane.transform.position = planeBehavior[plane].newPos();
                plane.transform.rotation = planeBehavior[plane].newRot();
            }
        }
    }
}
