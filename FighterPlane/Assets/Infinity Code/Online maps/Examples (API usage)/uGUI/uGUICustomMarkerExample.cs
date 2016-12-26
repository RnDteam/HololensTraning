/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;
using UnityEngine.UI;

namespace InfinityCode.OnlineMapsExamples
{
    [AddComponentMenu("")]
    public class uGUICustomMarkerExample:MonoBehaviour
    {
        public double lng;
        public double lat;

        public Text textField;
        public float height;

        private string _text;

        public string text
        {
            get { return _text; }
            set
            {
                if (textField != null) textField.text = value;
                _text = value;
            }
        }

        public void Dispose()
        {
            textField = null;
        }
    }
}