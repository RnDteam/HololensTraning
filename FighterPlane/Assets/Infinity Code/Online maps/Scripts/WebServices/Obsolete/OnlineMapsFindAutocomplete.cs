/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;

[Obsolete("OnlineMapsFindAutocomplete is obsolete. Use OnlineMapsGooglePlacesAutocomplete.")]
public class OnlineMapsFindAutocomplete: OnlineMapsGooglePlacesAutocomplete
{
    public new static OnlineMapsFindAutocompleteResult[] GetResults(string response)
    {
        OnlineMapsGooglePlacesAutocompleteResult[] results = OnlineMapsGooglePlacesAutocomplete.GetResults(response);
        return OnlineMapsUtils.DeepCopy<OnlineMapsFindAutocompleteResult[]>(results);
    }
}

[Obsolete("OnlineMapsFindAutocompleteResult is obsolete. Use OnlineMapsGooglePlacesAutocompleteResult.")]
public class OnlineMapsFindAutocompleteResult : OnlineMapsGooglePlacesAutocompleteResult
{
    public OnlineMapsFindAutocompleteResult()
    {
        
    }

    public OnlineMapsFindAutocompleteResult(OnlineMapsXML node) : base(node)
    {
    }
}

[Obsolete("OnlineMapsFindAutocompleteResultTerm is obsolete. Use OnlineMapsGooglePlacesAutocompleteResult.Term.")]
public class OnlineMapsFindAutocompleteResultTerm : OnlineMapsGooglePlacesAutocompleteResult.Term
{
    public OnlineMapsFindAutocompleteResultTerm()
    {
        
    }

    public OnlineMapsFindAutocompleteResultTerm(OnlineMapsXML node) : base(node)
    {
    }
}

[Obsolete("OnlineMapsFindAutocompleteResultMatchedSubstring is obsolete. Use OnlineMapsGooglePlacesAutocompleteResult.MatchedSubstring.")]
public class OnlineMapsFindAutocompleteResultMatchedSubstring : OnlineMapsGooglePlacesAutocompleteResult.MatchedSubstring
{
    public OnlineMapsFindAutocompleteResultMatchedSubstring()
    {
        
    }

    public OnlineMapsFindAutocompleteResultMatchedSubstring(OnlineMapsXML node) : base(node)
    {
    }
}