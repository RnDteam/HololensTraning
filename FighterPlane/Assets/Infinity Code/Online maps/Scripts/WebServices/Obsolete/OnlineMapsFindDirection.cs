/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;

[Obsolete("OnlineMapsFindDirection is obsolete. Use OnlineMapsGoogleDirections.")]
public class OnlineMapsFindDirection: OnlineMapsGoogleDirections
{
    public new static OnlineMapsFindDirectionResult GetResult(string response)
    {
        OnlineMapsGoogleDirectionsResult result = OnlineMapsGoogleDirections.GetResult(response);
        return OnlineMapsUtils.DeepCopy<OnlineMapsFindDirectionResult>(result);
    }
}

[Obsolete("OnlineMapsFindDirectionResult is obsolete. Use OnlineMapsGoogleDirectionsResult.")]
public class OnlineMapsFindDirectionResult : OnlineMapsGoogleDirectionsResult
{
    public OnlineMapsFindDirectionResult()
    {
        
    }

    public OnlineMapsFindDirectionResult(OnlineMapsXML xml) : base(xml)
    {
    }
}