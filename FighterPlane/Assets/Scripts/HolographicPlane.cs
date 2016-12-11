using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA;
using UnityEngine.VR.WSA.Persistence;

public class HolographicPlane : MonoBehaviour {

    private bool isPlaced = true;
    private Vector3 planeOffsetFromCamera;
    private WorldAnchor anchor;
    private WorldAnchorStore anchorStore;

    public string objectAnchorStoreName;


    // Use this for initialization
    void Start()
    {
        WorldAnchorStore.GetAsync(AnchorStoreReady);
        planeOffsetFromCamera = transform.position - Camera.main.transform.position;
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;
        isPlaced = false;

        Debug.Log("looking for " + objectAnchorStoreName);
        string[] ids = anchorStore.GetAllIds();
        for (int index = 0; index < ids.Length; index++)
        {
            Debug.Log(ids[index]);
            if (ids[index] == objectAnchorStoreName)
            {
                WorldAnchor wa = anchorStore.Load(ids[index], gameObject);
                isPlaced = true;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
        {
            transform.position = Vector3.Lerp(transform.position, ProposeTransformPosition(), 0.2f);
            transform.rotation = Camera.main.transform.rotation;
        } 
    }

    public void OnSelect()
    {
        isPlaced = !isPlaced;

        if(isPlaced)
        {
            anchor = gameObject.AddComponent<WorldAnchor>();

            if (anchor.isLocated)
            {
                Debug.Log("Saving persisted position immediately");
                bool saved = anchorStore.Save(objectAnchorStoreName, anchor);
                Debug.Log("saved: " + saved);
            }
            else
            {
                anchor.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
            }
        }
        else
        {
            WorldAnchor anchor = gameObject.GetComponent<WorldAnchor>();
            if (anchor != null)
            {
                DestroyImmediate(anchor);
            }

            string[] ids = anchorStore.GetAllIds();
            for (int index = 0; index < ids.Length; index++)
            {
                Debug.Log(ids[index]);
                if (ids[index] == objectAnchorStoreName)
                {
                    bool deleted = anchorStore.Delete(ids[index]);
                    Debug.Log("deleted: " + deleted);
                    break;
                }
            }
        }
    }

    private void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (located)
        {
            Debug.Log("Saving persisted position in callback");
            bool saved = anchorStore.Save(objectAnchorStoreName, self);
            Debug.Log("saved: " + saved);
            self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
        }
    }

    Vector3 ProposeTransformPosition()
    {
        // Put the model 2m in front of the user.
        // Vector3 retval = Camera.main.transform.position + Camera.main.transform.forward * 2;

        Vector3 retval = Camera.main.transform.position + Camera.main.transform.rotation * planeOffsetFromCamera;

        return retval;
    }
}
