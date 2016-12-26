/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;

[Obsolete("OnlineMapsGetElevation is obsolete. Use OnlineMapsGoogleElevation.")]
public class OnlineMapsGetElevation:OnlineMapsGoogleElevation
{
    
}

[Obsolete("OnlineMapsGetElevationResult is obsolete. Use OnlineMapsGoogleElevationResult.")]
public class OnlineMapsGetElevationResult : OnlineMapsGoogleElevationResult
{
    public OnlineMapsGetElevationResult()
    {
        
    }

    public OnlineMapsGetElevationResult(OnlineMapsXML node) : base(node)
    {
    }
}
