/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;

[Obsolete("OnlineMapsFindPlaces is obsolete. Use OnlineMapsGooglePlaces.")]
public class OnlineMapsFindPlaces:OnlineMapsGooglePlaces
{
    public new static OnlineMapsFindPlacesResult[] GetResults(string response, out string nextPageToken)
    {
        OnlineMapsGooglePlacesResult[] results = OnlineMapsGooglePlaces.GetResults(response, out nextPageToken);
        return OnlineMapsUtils.DeepCopy<OnlineMapsFindPlacesResult[]>(results);
    }
}

[Obsolete("OnlineMapsFindPlacesResult is obsolete. Use OnlineMapsGooglePlacesResult.")]
public class OnlineMapsFindPlacesResult : OnlineMapsGooglePlacesResult
{
    public OnlineMapsFindPlacesResult()
    {
        
    }

    public OnlineMapsFindPlacesResult(OnlineMapsXML node) : base(node)
    {
    }
}

[Obsolete("OnlineMapsFindPlacesResultPhoto is obsolete. Use OnlineMapsGooglePlacesResult.Photo.")]
public class OnlineMapsFindPlacesResultPhoto : OnlineMapsGooglePlacesResult.Photo
{
    public OnlineMapsFindPlacesResultPhoto ()
    {
        
    }

    public OnlineMapsFindPlacesResultPhoto(OnlineMapsXML node) : base(node)
    {
    }
}