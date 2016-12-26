/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;

[Obsolete("OnlineMapsFindPlaceDetails is obsolete. Use OnlineMapsGooglePlaceDetails.")]
public class OnlineMapsFindPlaceDetails:OnlineMapsGooglePlaceDetails
{
    public new static OnlineMapsFindPlaceDetailsResult GetResult(string response)
    {
        OnlineMapsGooglePlaceDetailsResult results = OnlineMapsGooglePlaceDetails.GetResult(response);
        return OnlineMapsUtils.DeepCopy<OnlineMapsFindPlaceDetailsResult>(results);
    }

}

[Obsolete("OnlineMapsFindPlaceDetailsResult is obsolete. Use OnlineMapsGooglePlaceDetailsResult.")]
public class OnlineMapsFindPlaceDetailsResult : OnlineMapsGooglePlaceDetailsResult
{
    public OnlineMapsFindPlaceDetailsResult()
    {
        
    }

    public OnlineMapsFindPlaceDetailsResult(OnlineMapsXML node) : base(node)
    {
    }
}