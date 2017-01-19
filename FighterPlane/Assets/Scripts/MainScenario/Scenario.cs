using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario : MonoBehaviour
{
    public string buildingId = "96623741";
    public float flashesTimeDifference = 0.3f;
    public int flashLoop = 15;

    public void ActivateScenario()
    {
        StartCoroutine(DetectTarget(GlobalManager.TimeBetweenTakeOffToFindingTarget, flashesTimeDifference));
    }

    public IEnumerator DetectTarget(float delayTimeAfterTakeOff, float flashSeconds)
    {
        PlaneManager.Instance.AllPlanesTakeOff();
        yield return new WaitForSeconds(delayTimeAfterTakeOff);

        // Grand canyon mall id
        GameObject building = BuildingManager.Instance.getBuildingById(buildingId);

        if(building != null)
        {
            // Found target sound(sits on map object)
            GetComponent<AudioSource>().Play();

            for (int i = 0; i < flashLoop; i++)
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
}
