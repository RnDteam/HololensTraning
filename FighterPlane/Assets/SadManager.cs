using Academy.HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SadManager : MonoBehaviour {

    public bool StartFlying = false;
    

    private void Update()
    {
        if (StartFlying)
        {
            StartFlying = false;
            KeywordManager.Instance.myKeywordsAndResponses.Single(k => k.Keywords.Contains("Start")).Response.Invoke(); ;
        }
    }
}
