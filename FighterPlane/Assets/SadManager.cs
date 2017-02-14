using Academy.HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SadManager : MonoBehaviour {

    public bool StartFlying = false;
    public bool ShowInfo = false;
    public bool ApproveClimb = false;
    public bool DisapproveClimb = false;

    private void InvokeMethod(string keyword)
    {
        var count = KeywordManager.Instance.myKeywordsAndResponses.Count(k => k.Keywords.Contains(keyword));
        if (count == 0)
        {
            Debug.LogWarning("No keyword: " + keyword);
        }
        else if (count > 1)
        {
            Debug.LogWarning("Multiple keywords: " + keyword);
        }
        else
        {
            KeywordManager.Instance.myKeywordsAndResponses.Single(k => k.Keywords.Contains(keyword)).Response.Invoke();
        }
    }

    private void Update()
    {
        if (StartFlying)
        {
            StartFlying = false;
            InvokeMethod("Start");
        }
        if (ApproveClimb)
        {
            ApproveClimb = false;
            InvokeMethod("Approve");
        }
        if (DisapproveClimb)
        {
            DisapproveClimb = false;
            InvokeMethod("Disapprove");
        }
		if (ShowInfo)
		{
			ShowInfo = false;
			KeywordManager.Instance.myKeywordsAndResponses.Single(k => k.Keywords.Contains("Show Info")).Response.Invoke(); ;
		}
    }
}
