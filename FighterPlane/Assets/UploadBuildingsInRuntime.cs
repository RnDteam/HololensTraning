using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                Color textColor = new Color(60, 153, 208);
                building.AddComponent<MapChangesListener>();
                building.GetComponent<MeshFilter>().mesh = (Mesh)Resources.Load("Meshes/mesh" + building.name, typeof(Mesh));
                building.GetComponent<MeshCollider>().sharedMesh = building.GetComponent<MeshFilter>().mesh;
                building.transform.FindChild("BuildingInfo(Clone)").GetComponent<TextMesh>().color = new Color(textColor.r / 255, textColor.g / 255, textColor.b / 255);
                if (!building.name.Equals("96623741"))
                {
                    if (building.transform.FindChild("BuildingInfo(Clone)"))
                    {
                        building.transform.FindChild("BuildingInfo(Clone)").gameObject.SetActive(false);
                    }
                }
                        
                else
                {
                    building.transform.FindChild("BuildingInfo(Clone)")
                            .gameObject.GetComponent<TextMesh>().text = "ןוגד" + "\n\n" + "Rocket" + ":שומיח גוס" ;
                    building.transform.FindChild("BuildingInfo(Clone)")
                                .gameObject.GetComponent<TextMesh>().alignment = TextAlignment.Center;
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