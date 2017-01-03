using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonManager : MonoBehaviour {
	

    private OnlineMaps onlineMaps;
    private OnlineMapsBuildings buildings;
    private OnlineMapsTileSetControl onlineMapsTiles;
    
    public Vector2[] vertices;
    private Vector2[] verticesCoords;
    private GameObject[] lines;
    public Color lineColor;

    private Vector3 mapTopLeft;
    private Vector3 mapBottomRight;
    private Vector3 mapCenter;
    private float mapWidth;
    private float mapLength;
    private float maxElevation;
    // Maybe vectors
    double TopLeftLat, TopLeftLong, BottomRightLat, BottomRightLong;

    public bool IsPolygonShown { get; private set; }
	
	void Start () {
		onlineMaps = OnlineMaps.instance;
        onlineMapsTiles = OnlineMapsTileSetControl.instance;
        ShowPolygon();
        //GroundGenerator.Instance.MeshCreated += UpdatePoints;
        onlineMapsTiles.OnMeshUpdated += UpdatePoints;
    }

    private void UpdatePoints()
    {
        Debug.Log("Here!");
        transform.position = mapCenter + new Vector3(0, GroundGenerator.Instance.maxY / 600, 0);
        //var maxY = GroundGenerator.Instance.maxY;


        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    vertices[i] = OnlineMapsTileSetControl.instance.GetWorldPosition(transform.TransformPoint(vertices[i]));
        //}
    }

    public void ShowPolygon()
    {
        // Stop if num of verticles cannot create a polygon
        if (vertices.Length <= 2)
        {
            return;
        }

        lines = new GameObject[vertices.Length];
        verticesCoords = new Vector2[vertices.Length];

        for (int vertexIndex = 0; vertexIndex < vertices.Length; vertexIndex++)
        {
            verticesCoords[vertexIndex] = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(transform.TransformPoint(vertices[vertexIndex]));

            CreateLineObject(vertexIndex);
        }

        InitializePolygonBounds();
        CreatingLines();

    }

    private void CreatingLines()
    {
        Vector3 firstPoint;
        Vector3 secondPoint;

        firstPoint = CalculateVector3(vertices[0]);

        for (int vertexInd = 0; vertexInd < vertices.Length - 1; vertexInd++)
        {
            secondPoint = CalculateVector3(vertices[vertexInd + 1]);
            CreateLineBetweenVertices(lines[vertexInd], firstPoint, secondPoint);
            firstPoint = secondPoint;
        }

        // Connect first and last vertices
        firstPoint = CalculateVector3(vertices[0]);
        secondPoint = CalculateVector3(vertices[vertices.Length - 1]);

        CreateLineBetweenVertices(lines[vertices.Length - 1], firstPoint, secondPoint);
    }

    private void CreateLineObject(int vertexIndex)
    {
        lines[vertexIndex] = new GameObject("line" + vertexIndex);
        lines[vertexIndex].transform.parent = this.transform;
        lines[vertexIndex].transform.localPosition = Vector3.zero;
        lines[vertexIndex].transform.localScale = Vector3.one;
    }

    private Vector3 CalculateVector3(Vector2 vector2)
    {
        //Debug.Log(onlineMapsTiles.GetMaxElevationValue(mapScale));
        return new Vector3(vector2.x, mapCenter.y, vector2.y);
    }

    private void CreateLineBetweenVertices(GameObject lineObject, Vector3 firstPoint, Vector3 secondPoint)
    {
        LineRenderer lr = lineObject.AddComponent<LineRenderer>() as LineRenderer;
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.useWorldSpace = false;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        lr.SetPosition(0, firstPoint);
        lr.SetPosition(1, secondPoint);
    }

    private void InitializePolygonBounds()
    {
        // Getting two corners of map
        onlineMaps.GetTopLeftPosition(out TopLeftLat, out TopLeftLong);
        onlineMaps.GetBottomRightPosition(out BottomRightLat, out BottomRightLong);

        // Retrieving World Position of corners using the corner's latitue and longtitude
        mapTopLeft = onlineMapsTiles.GetWorldPosition(TopLeftLat, TopLeftLong);
        mapBottomRight = onlineMapsTiles.GetWorldPosition(BottomRightLat, BottomRightLong);
        mapCenter = Vector3.Lerp(mapTopLeft, mapBottomRight, 0.5f);
        
        // why * 10 needed?
        mapLength = Math.Abs(mapTopLeft.x - mapBottomRight.x) * 10;
        mapWidth = Math.Abs(mapTopLeft.z - mapBottomRight.z) * 10;
        Debug.Log(mapLength);
        Debug.Log(mapWidth);
        Debug.Log(GroundGenerator.Instance.maxY);
        transform.position = mapCenter + new Vector3(0, GroundGenerator.Instance.maxY / 10, 0);
    }

    // Update is called once per frame
    void Update () {
    }
}
