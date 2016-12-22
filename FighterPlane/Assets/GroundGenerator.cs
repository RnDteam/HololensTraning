using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundGenerator : MonoBehaviour {

    private OnlineMapsTileSetControl OnlineMaps;
    private Mesh newMesh;
    
    public GameObject BottomBase;
    public Material BaseMaterial;

    void Start()
    {
        OnlineMaps = GetComponent<OnlineMapsTileSetControl>();
        OnlineMaps.OnMeshUpdated += generateNewMash;
    }

    private void generateNewMash()
    {
        var mapVertices = GetComponent<MeshFilter>().sharedMesh.vertices;
        if (mapVertices.Select(v => v.y).Min() == 0)
            return;

        var mapMesh = GetComponent<MeshFilter>().sharedMesh;
        var bottomMesh = BottomBase.GetComponent<MeshFilter>().sharedMesh;

        var mapOuterSquare = GetOuterSquare(mapMesh.vertices);
        var bottomSquare = GenerateBottomSquare(mapOuterSquare, BottomBase.transform.position.y);
       
        
        newMesh = new Mesh();
        newMesh.name = "newMesh";
        List<Vector3> vertices = new List<Vector3>();
        vertices.AddRange(mapOuterSquare);
        vertices.AddRange(bottomSquare);

        newMesh.vertices = vertices.ToArray();
        newMesh.triangles = CreateTriangles(mapOuterSquare, bottomSquare);
        newMesh.RecalculateNormals();


        var newGameObject = new GameObject("Ground");
        newGameObject.transform.parent = gameObject.transform;
        newGameObject.transform.localRotation = Quaternion.identity;
        newGameObject.transform.localScale = Vector3.one;
        newGameObject.transform.localPosition = Vector3.zero;

        newGameObject.AddComponent<MeshFilter>();
        newGameObject.AddComponent<MeshRenderer>();
        newGameObject.GetComponent<MeshFilter>().mesh = newMesh;
        newGameObject.GetComponent<MeshRenderer>().material = BaseMaterial;

        OnlineMaps.OnMeshUpdated -= generateNewMash;
    }

    private IEnumerable<Vector3> GetOuterSquare(IEnumerable<Vector3> Vertices)
    {
        var distinctVetrices = Vertices.Distinct();

        var minX = distinctVetrices.Select(v => v.x).Min();
        var maxX = distinctVetrices.Select(v => v.x).Max();
        var minZ = distinctVetrices.Select(v => v.z).Min();
        var maxZ = distinctVetrices.Select(v => v.z).Max();

        var minXVertices = distinctVetrices.Where(v => v.x == minX);
        var maxXVertices = distinctVetrices.Where(v => v.x == maxX);
        var minZVertices = distinctVetrices.Where(v => v.z == minZ);
        var maxZVertices = distinctVetrices.Where(v => v.z == maxZ);

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
