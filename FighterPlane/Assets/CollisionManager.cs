using HoloToolkit;
using HoloToolkit.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class CollisionManager : Singleton<CollisionManager> {

    public struct Collision
    {
        public string object1;
        public string object2;
    }

    private GameObject[] planes;
    private List<Collision> collisions;
    private bool waitForAnswer = false;

    public float ClimbHeight = .25f;
    public float Timeout = 8;

    private Collision? collisionWaitingForAnswer = null;

    private void Start()
    {
        planes = PlaneManager.Instance.planes;

        var timer = new System.Threading.Timer((e) =>
        {
            collisions = new List<Collision>();
        }, null, 0, (int)(TimeSpan.FromSeconds(1).TotalMilliseconds));
    }

    private void Update()
    {
        if (waitForAnswer)
        {
            if (Timeout > 0)
            {
                Timeout -= Time.deltaTime;
            }
            else
            {
                RemoveCollisionInfo();
            }
        }
    }
    
    public void ColliderTriggered(string plane1, string plane2)
    {
        if (string.Compare(plane1, plane2) < 0)
        {
            var temp = plane2;
            plane2 = plane1;
            plane1 = temp;
        }
        var collision = new Collision { object1 = plane1, object2 = plane2 };
        if (!collisions.Any(c => c.object1 == collision.object1 && c.object2 == collision.object2))
        {
            collisions.Add(collision);
            NotifyCollision(collision);
        }
    }

    private void NotifyCollision(Collision collision)
    {
        PlaneManager.Instance.SetCollisionPlanes(collision.object1, collision.object2);
        TextToSpeechManager.Instance.SpeakText(string.Format("Collision detected between {0} and {1}. Would you like {2} to climb?", collision.object1, collision.object2, collision.object1));
        collisionWaitingForAnswer = collision;
        waitForAnswer = true;
    }

    private void RemoveCollisionInfo()
    {
        waitForAnswer = false;
        collisionWaitingForAnswer = null;
        PlaneManager.Instance.UnsetCollisionPlanes();
    }

    public void ApproveClimb()
    {
        if (!collisionWaitingForAnswer.HasValue)
            return;

        var plane = collisionWaitingForAnswer.Value.object1;
        planes.Single(p => p.name == plane).GetComponent<BoxCollider>().enabled = false;
        PlaneManager.Instance.Climb(ClimbHeight, plane);
        TextToSpeechManager.Instance.SpeakText(string.Format("Climbing to {0} feet", (int)Math.Round(ClimbHeight * 5.859936 + planes.Single(p => p.name == plane).transform.localPosition.y) * GlobalManager.heightDisplayFactor));

        RemoveCollisionInfo();
    }

    public void DisapproveClimb()
    {
        if (collisionWaitingForAnswer == null)
            return;

        RemoveCollisionInfo();

        TextToSpeechManager.Instance.SpeakText("Climb disapproved");
    }
}
