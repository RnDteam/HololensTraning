using HoloToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void GroundMeshCreated();

public partial class GroundGenerator : Singleton<GroundGenerator> {

    private OnlineMapsTileSetControl OnlineMaps;
    private Mesh newMesh;
    private string groundName = "Ground";
    private string meshName = "NewMesh";

    public Material BaseMaterial;
    public LineRenderer TopMargins;
    public LineRenderer BottomMargins;
    public float maxY = 0;

    public event GroundMeshCreated MeshCreated;

    protected virtual void OnMeshCreated()
    {
        if (MeshCreated != null)
        {
            MeshCreated();
        }
    }

    void Start()
    {
        OnlineMaps = GetComponent<OnlineMapsTileSetControl>();
        OnlineMaps.OnMeshUpdated += generateNewMash;
    }

    private void DestroyChildren(Predicate<Transform> predicate)
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (predicate.Invoke(child))
            {
                children.Add(child.gameObject);
            }
        }
        children.ForEach(child => Destroy(child));
    }

    private void generateNewMash()
    {
        var mapMesh = GetComponent<MeshFilter>().sharedMesh;
        if (mapMesh.vertices.Select(v => v.y).Max() == 0)
            return;

        //Destroy all of the map's old ground.
        DestroyChildren(c => c.name == groundName);

        var mapOuterSquare = GetOuterSquare(mapMesh.vertices);
        var bottomSquare = GenerateBottomSquare(mapOuterSquare, 0);

        TopMargins.numPositions = mapOuterSquare.Count();
        TopMargins.SetPositions(mapOuterSquare.ToArray());
        BottomMargins.numPositions = bottomSquare.Count();
        BottomMargins.SetPositions(bottomSquare.ToArray());

        newMesh = new Mesh() { name = meshName };
        var vertices = new List<Vector3>();
        vertices.AddRange(mapOuterSquare);
        vertices.AddRange(bottomSquare);

        newMesh.vertices = vertices.ToArray();
        newMesh.triangles = CreateTriangles(mapOuterSquare, bottomSquare);
        newMesh.RecalculateNormals();

        var newGameObject = CreateNewGameObject(gameObject.transform, groundName, Vector3.zero,
            Quaternion.identity, Vector3.one, new Type[] { typeof(MeshFilter), typeof(MeshRenderer) });
        newGameObject.GetComponent<MeshFilter>().mesh = newMesh;
        newGameObject.GetComponent<MeshRenderer>().material = BaseMaterial;

        OnMeshCreated();
    }

    private GameObject CreateNewGameObject(Transform parent, string name, Vector3 localPosition, Quaternion localRotation, Vector3 localScale, Type[] components)
    {
        var newGameObject = new GameObject(name, components);
        newGameObject.transform.parent = parent;
        newGameObject.transform.localRotation = localRotation;
        newGameObject.transform.localScale = localScale;
        newGameObject.transform.localPosition = localPosition;
        return newGameObject;
    }

    private IEnumerable<Vector3> GetOuterSquare(IEnumerable<Vector3> Vertices)
    {
        var distinctVetrices = Vertices.Distinct();

        float minX, maxX = minX = Vertices.ElementAt(0).x;
        float minZ, maxZ = minZ = Vertices.ElementAt(0).z;
        
        foreach (var v in distinctVetrices)
        {
            if (v.x > maxX) maxX = v.x;
            if (v.x < minX) minX = v.x;
            if (v.y > maxY) maxY = v.y;
            if (v.z > maxZ) maxZ = v.z;
            if (v.z < minZ) minZ = v.z;
        }

        var minXVertices = new List<Vector3>();
        var maxXVertices = new List<Vector3>();
        var minZVertices = new List<Vector3>();
        var maxZVertices = new List<Vector3>();

        foreach (var v in distinctVetrices)
        {
            if (v.x == minX) minXVertices.Add(v);
            if (v.x == maxX) maxXVertices.Add(v);
            if (v.z == minZ) minZVertices.Add(v);
            if (v.z == maxZ) maxZVertices.Add(v);
        }

        return new List<Vector3>()
            .Concat(minXVertices.OrderBy(v => v.z))
            .Concat(maxZVertices.OrderBy(v => v.x))
            .Concat(maxXVertices.OrderByDescending(v => v.z))
            .Concat(minZVertices.OrderByDescending(v => v.x));
    }

    private IEnumerable<Vector3> GenerateBottomSquare(IEnumerable<Vector3> Vertices, float height)
    {
        var square = new List<Vector3>();
        foreach (var v in Vertices)
        {
            square.Add(new Vector3(v.x, height, v.z));
        }
        return square;
    }

    private int[] CreateTriangles(IEnumerable<Vector3> topSquare, IEnumerable<Vector3> bottomSquare)
    {
        int quads = topSquare.Count() - 2;
        int[] triangles = new int[quads * 6];
        int offset = topSquare.Count();
        int t = 0;

        for (int q = 0, v = 0; q < quads; q++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + offset, v + offset + 1);
        }

        return triangles;
    }

    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }
}
