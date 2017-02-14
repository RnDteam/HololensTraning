using Academy.HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SadManager : MonoBehaviour {

    public bool StartFlying = false;

    public Action[] Actions = new Action[KeywordManager.Instance.myKeywordsAndResponses.Count()];

    public struct Action
    {
        public string name;
        public bool apply;
    }

    private void Start()
    {
        var keywords = KeywordManager.Instance.myKeywordsAndResponses;
        int count = 0;
        foreach (var keyword in keywords)
        {
            Actions[count++] = new Action { name = keyword.MethodPurpose, apply = false };
            //Actions.Add(keyword.MethodPurpose, false);
        }
    }

    private void Update()
    {
        if (StartFlying)
        {
            StartFlying = false;
            KeywordManager.Instance.myKeywordsAndResponses.Single(k => k.Keywords.Contains("Start")).Response.Invoke(); ;
        }
    }
}
