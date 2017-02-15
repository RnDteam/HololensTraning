using UnityEngine;

public class PlaneCollider : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactible"))
            CollisionManager.Instance.ColliderTriggered(gameObject, other.gameObject);
    }

}
