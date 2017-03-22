using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadBuildingsInRuntime : MonoBehaviour
{
    
    void Start()
    {
        GameObject building;
        Material roofMaterial = (Material)Resources.Load("Assets/Resources/Materials/RoofMaterial", typeof(Material));

        foreach (Transform child in transform)
        {
            building = child.gameObject;
            if (!building.active || !building.name.StartsWith("96"))
            {
                Destroy(building);
            }
            else
            {
                building.AddComponent<MapChangesListener>();
                building.GetComponent<MeshFilter>().mesh = (Mesh)Resources.Load("Meshes/mesh" + building.name, typeof(Mesh));
                building.GetComponent<MeshCollider>().sharedMesh = building.GetComponent<MeshFilter>().mesh;
                if (building.transform.FindChild("BuildingInfo(Clone)"))
                {
                    Debug.Log("Found");
                    building.transform.FindChild("BuildingInfo(Clone)").gameObject.SetActive(false);
                }
                for (var i = 0; i < building.GetComponent<MeshRenderer>().materials.Length; i++)
                {
                    building.GetComponent<MeshRenderer>().materials[i] = roofMaterial;
                }
            }
        }
    }
}