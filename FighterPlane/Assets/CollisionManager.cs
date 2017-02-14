using Assets.Scripts.Plane;
using HoloToolkit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class CollisionManager : Singleton<CollisionManager> {

    private GameObject[] planes;

    private void Start()
    {
        planes = PlaneManager.Instance.planes;
        
        
    }
    

    public void ColliderTriggered(string plane1, string plane2)
    {
        
        Debug.Log(plane1 + "\t" + plane2);
        //PlaneManager.Instance.BeginFlight(plane1);
        //PlaneManager.Instance.DoLoop(plane1);
    }
}
