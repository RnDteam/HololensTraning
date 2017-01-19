using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    public string buildingId = "39383661";

    public void ActivateScenario()
    {
        StartCoroutine(DetectTartget(3, 0.3f, "123"));
    }

    public IEnumerator DetectTartget(float seconds, float flashSeconds, string targetId)
    {
        PlaneManager.Instance.AllPlanesTakeOff();

        yield return new WaitForSeconds(seconds);
        GetComponent<AudioSource>().Play();
        // Grand canyon mall id
        GameObject building = BuildingManager.Instance.getBuildingById(buildingId);

        for (int i = 0; i < 15; i++)
        {
            building.GetComponent<BuildingDisplay>().Select();
            yield return new WaitForSeconds(flashSeconds);
            building.GetComponent<BuildingDisplay>().Unselect();
            yield return new WaitForSeconds(flashSeconds);
        }

        building.GetComponent<BuildingDisplay>().Select();
        yield return new WaitForSeconds(flashSeconds);
    }
}
