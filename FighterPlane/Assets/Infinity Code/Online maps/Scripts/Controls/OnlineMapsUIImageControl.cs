/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

#if CURVEDUI
using CurvedUI;
#endif

/// <summary>
/// Class control the map for the uGUI UI Image.
/// </summary>
[AddComponentMenu("Infinity Code/Online Maps/Controls/UI Image")]
public class OnlineMapsUIImageControl : OnlineMapsControlBase2D
{
    private Image image;

#if CURVEDUI
    private CurvedUISettings curvedUI;
#endif

    /// <summary>
    /// Singleton instance of OnlineMapsUIImageControl control.
    /// </summary>
    public new static OnlineMapsUIImageControl instance
    {
        get { return OnlineMapsControlBase.instance as OnlineMapsUIImageControl; }
    }

    private Camera worldCamera
    {
        get
        {
            if (image.canvas.renderMode == RenderMode.ScreenSpaceOverlay) return null;
            return image.canvas.worldCamera;
        }
    }

    protected override void BeforeUpdate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
        int touchCount = Input.GetMouseButton(0) ? 1 : 0;
        if (touchCount != lastTouchCount)
        {
            if (touchCount == 1) OnMapBasePress();
            else if (touchCount == 0) OnMapBaseRelease();
        }
        lastTouchCount = touchCount;
#else
        if (Input.touchCount != lastTouchCount)
        {
            if (Input.touchCount == 1) OnMapBasePress();
            else if (Input.touchCount == 0) OnMapBaseRelease();
        }
        lastTouchCount = Input.touchCount;
#endif
    }

    public override Vector2 GetCoords(Vector2 position)
    {
        Vector2 point;

#if CURVEDUI
        if (curvedUI != null)
        {
            Camera activeCamera = image.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? Camera.main : image.canvas.worldCamera;
            if (!curvedUI.RaycastToCanvasSpace(activeCamera.ScreenPointToRay(position), out point)) return Vector2.zero;
            Vector3 worldPoint = image.canvas.transform.localToWorldMatrix.MultiplyPoint(point);
            point = image.rectTransform.worldToLocalMatrix.MultiplyPoint(worldPoint);
        }
        else
        {
#endif
            if (!RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, position, worldCamera)) return Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, position, worldCamera, out point);
#if CURVEDUI
        }
#endif

        Rect rect = image.GetPixelAdjustedRect();

        Vector2 size = rect.max - point;
        size.x = size.x / rect.size.x;
        size.y = size.y / rect.size.y;

        Vector2 r = new Vector2(size.x - .5f, size.y - .5f);

        int countX = map.width / OnlineMapsUtils.tileSize;
        int countY = map.height / OnlineMapsUtils.tileSize;

        double px, py;
        map.GetTilePosition(out px, out py);

        px -= countX * r.x;
        py += countY * r.y;

        map.projection.TileToCoordinates(px, py, map.zoom, out px, out py);
        return new Vector2((float)px, (float)py);
    }

    public override bool GetCoords(out double lng, out double lat, Vector2 position)
    {
        lng = lat = 0;
        
        Vector2 point;

#if CURVEDUI
        if (curvedUI != null)
        {
            Camera activeCamera = image.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? Camera.main : image.canvas.worldCamera;

            if (!curvedUI.RaycastToCanvasSpace(activeCamera.ScreenPointToRay(position), out point)) return false;
            Vector3 worldPoint = image.canvas.transform.localToWorldMatrix.MultiplyPoint(point);
            point = image.rectTransform.worldToLocalMatrix.MultiplyPoint(worldPoint);
        }
        else
        {
#endif
            if (!RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, position, worldCamera)) return false;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, position, worldCamera, out point);
#if CURVEDUI
        }
#endif

        Rect rect = image.GetPixelAdjustedRect();

        Vector2 size = rect.max - point;
        size.x = size.x / rect.size.x;
        size.y = size.y / rect.size.y;

        Vector2 r = new Vector2(size.x - .5f, size.y - .5f);

        int countX = map.width / OnlineMapsUtils.tileSize;
        int countY = map.height / OnlineMapsUtils.tileSize;

        double px, py;
        map.GetTilePosition(out px, out py);

        px -= countX * r.x;
        py += countY * r.y;

        map.projection.TileToCoordinates(px, py, map.zoom, out lng, out lat);
        return true;
    }

    public override Rect GetRect()
    {
        RectTransform rectTransform = (RectTransform)transform;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        float xMin = float.PositiveInfinity, xMax = float.NegativeInfinity, yMin = float.PositiveInfinity, yMax = float.NegativeInfinity;
        for (int i = 0; i < 4; i++)
        {
            Vector3 screenCoord = RectTransformUtility.WorldToScreenPoint(worldCamera, corners[i]);
            if (screenCoord.x < xMin) xMin = screenCoord.x;
            if (screenCoord.x > xMax) xMax = screenCoord.x;
            if (screenCoord.y < yMin) yMin = screenCoord.y;
            if (screenCoord.y > yMax) yMax = screenCoord.y;
        }
        Rect result = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        return result;
    }

    protected override bool HitTest()
    {
        Vector2 inputPosition = GetInputPosition();

#if CURVEDUI
        if (curvedUI != null)
        {
            Camera activeCamera = image.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? Camera.main : image.canvas.worldCamera;
            return curvedUI.RaycastToCanvasSpace(activeCamera.ScreenPointToRay(inputPosition), out inputPosition);
        }
#endif
        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position = inputPosition;
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);

        if (hits.Count > 0 && hits[0].gameObject != gameObject) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, inputPosition, worldCamera);
    }

    protected override bool HitTest(Vector2 position)
    {
#if CURVEDUI
        if (curvedUI != null)
        {
            Camera activeCamera = image.canvas.renderMode == RenderMode.ScreenSpaceOverlay ? Camera.main : image.canvas.worldCamera;
            return curvedUI.RaycastToCanvasSpace(activeCamera.ScreenPointToRay(position), out position);
        }        
#endif
        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position = position;
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);

        if (hits.Count > 0 && hits[0].gameObject != gameObject) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, position, worldCamera);
    }

    protected override void OnEnableLate()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Can not find Image.");
            OnlineMapsUtils.DestroyImmediate(this);
        }

#if CURVEDUI
        curvedUI = image.canvas.GetComponent<CurvedUISettings>();
#endif
    }

    public override void SetTexture(Texture2D texture)
    {
        base.SetTexture(texture);
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}