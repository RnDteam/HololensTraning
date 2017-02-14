using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollider : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        CollisionManager.Instance.ColliderTriggered(gameObject.name, other.gameObject.name);
    }

}
