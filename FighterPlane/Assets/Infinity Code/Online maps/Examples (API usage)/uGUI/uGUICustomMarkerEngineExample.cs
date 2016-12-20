/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System.Collections.Generic;
using UnityEngine;

namespace InfinityCode.OnlineMapsExamples
{
    [AddComponentMenu("Infinity Code/Online Maps/Examples (API Usage)/uGUICustomMarkerEngineExample")]
    public class uGUICustomMarkerEngineExample : MonoBehaviour 
    {
        private static uGUICustomMarkerEngineExample _instance;
        private static List<uGUICustomMarkerExample> markers;

        public RectTransform markerContainer;
        public GameObject markerPrefab;

        private GameObject container;
        private bool needUpdateMarkers;
        private Canvas canvas;
        private OnlineMaps map;
        private OnlineMapsControlBase control;

        public static uGUICustomMarkerEngineExample instance
        {
            get { return _instance; }
        }

        private Camera worldCamera
        {
            get
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) return null;
                return canvas.worldCamera;
            }
        }

        public static uGUICustomMarkerExample AddMarker(Vector2 position, string text)
        {
            return AddMarker(position.x, position.y, text);
        }

        public static uGUICustomMarkerExample AddMarker(double lng, double lat, string text)
        {
            GameObject markerGameObject = Instantiate(_instance.markerPrefab) as GameObject;
            (markerGameObject.transform as RectTransform).SetParent(_instance.markerContainer);
            markerGameObject.transform.localScale = Vector3.one;
            uGUICustomMarkerExample marker = markerGameObject.GetComponent<uGUICustomMarkerExample>();
            if (marker == null) marker = markerGameObject.AddComponent<uGUICustomMarkerExample>();

            marker.text = text;
            marker.lng = lng;
            marker.lat = lat;

            markers.Add(marker);
            _instance.UpdateMarker(marker);
            return marker;
        }

        private void OnEnable()
        {
            _instance = this;
            markers = new List<uGUICustomMarkerExample>();
            canvas = markerContainer.GetComponentInParent<Canvas>();
        }

        public static void RemoveAllMarkers()
        {
            foreach (uGUICustomMarkerExample marker in markers)
            {
                marker.Dispose();
                OnlineMapsUtils.DestroyImmediate(marker.gameObject);
            }
            markers.Clear();
        }

        public static void RemoveMarker(uGUICustomMarkerExample marker)
        {
            OnlineMapsUtils.DestroyImmediate(marker.gameObject);
            marker.Dispose();
            markers.Remove(marker);
        }

        public static void RemoveMarkerAt(int index)
        {
            if (index < 0 || index >= markers.Count) return;

            uGUICustomMarkerExample marker = markers[index];
            OnlineMapsUtils.DestroyImmediate(marker.gameObject);
            marker.Dispose();
            markers.RemoveAt(index);
        }

        private void Start () 
        {
            map = OnlineMaps.instance;
            control = OnlineMapsControlBase.instance;

            map.OnMapUpdated += UpdateMarkers;
            if (control is OnlineMapsControlBase3D) OnlineMapsControlBase3D.instance.OnCameraControl += UpdateMarkers;

            AddMarker(map.position, "Example Marker");
        }

        private void UpdateMarkers()
        {
            foreach (uGUICustomMarkerExample marker in markers) UpdateMarker(marker);
        }

        private void UpdateMarker(uGUICustomMarkerExample marker)
        {
            double tlx, tly, brx, bry;

            int countX = map.width / OnlineMapsUtils.tileSize;
            int countY = map.height / OnlineMapsUtils.tileSize;

            double px, py;
            map.projection.CoordinatesToTile(map.buffer.apiPosition.x, map.buffer.apiPosition.y, map.buffer.apiZoom, out px, out py);

            px -= countX / 2f;
            py -= countY / 2f;

            map.projection.TileToCoordinates(px, py, map.buffer.apiZoom, out tlx, out tly);

            px += countX;
            py += countY;

            map.projection.TileToCoordinates(px, py, map.buffer.apiZoom, out brx, out bry);

            UpdateMarker(marker, tlx, tly, brx, bry);
        }

        private void UpdateMarker(uGUICustomMarkerExample marker, double tlx, double tly, double brx, double bry)
        {
            double px = marker.lng;
            double py = marker.lat;

            if (px < tlx || px > brx || py < bry || py > tly)
            {
                marker.gameObject.SetActive(false);
                return;
            }

            Vector2 screenPosition = control.GetScreenPosition(px, py);
            RectTransform markerRectTransform = marker.transform as RectTransform;

            if (!marker.gameObject.activeSelf) marker.gameObject.SetActive(true);

            screenPosition.y += markerRectTransform.rect.height / 2;

            Vector2 point;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(markerRectTransform.parent as RectTransform, screenPosition, worldCamera, out point);
            markerRectTransform.localPosition = point;
        }
    }
}