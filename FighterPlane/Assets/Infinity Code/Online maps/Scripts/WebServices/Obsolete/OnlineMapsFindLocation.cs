/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;

[Obsolete("OnlineMapsFindLocation is obsolete. Use OnlineMapsGoogleGeocoding.")]
public class OnlineMapsFindLocation: OnlineMapsGoogleGeocoding
{
    public new static OnlineMapsFindLocationResult[] GetResults(string response)
    {
        OnlineMapsGoogleGeocodingResult[] results = OnlineMapsGoogleGeocoding.GetResults(response);
        return OnlineMapsUtils.DeepCopy<OnlineMapsFindLocationResult[]>(results);
    }
}

[Obsolete("OnlineMapsFindLocationResult is obsolete. Use OnlineMapsGoogleGeocodingResult.")]
public class OnlineMapsFindLocationResult : OnlineMapsGoogleGeocodingResult
{
    public OnlineMapsFindLocationResult()
    {
        
    }

    public OnlineMapsFindLocationResult(OnlineMapsXML node) : base(node)
    {
    }
}

[Obsolete("OnlineMapsFindLocationResultAddressComponent is obsolete. Use OnlineMapsGoogleGeocodingResult.AddressComponent.")]
public class OnlineMapsFindLocationResultAddressComponent : OnlineMapsGoogleGeocodingResult.AddressComponent
{
    public OnlineMapsFindLocationResultAddressComponent()
    {
        
    }

    public OnlineMapsFindLocationResultAddressComponent(OnlineMapsXML node) : base(node)
    {
    }
}