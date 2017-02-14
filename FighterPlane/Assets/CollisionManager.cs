﻿using Assets.Scripts.Plane;
using HoloToolkit;
using HoloToolkit.Unity;
using System;
using System.Collections;
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

    public TextToSpeechManager Speech;

    private Collision? collisionWaitingForAnswer = null;

    private void Start()
    {
        planes = PlaneManager.Instance.planes;

        var timer = new System.Threading.Timer((e) =>
        {
            collisions = new List<Collision>();
        }, null, 0, (int)(TimeSpan.FromSeconds(1).TotalMilliseconds));

        var timer2 = new System.Threading.Timer((e) =>
        {
            collisionWaitingForAnswer = null;
        }, null, 0, (int)(TimeSpan.FromSeconds(8).TotalMilliseconds));
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
        Debug.Log("Collision: " + collision.object1 + " and " + collision.object2);
        Speech.SpeakText(string.Format("Collision detected between {0} and {1}. Would you like to climb?", collision.object1, collision.object2));
        collisionWaitingForAnswer = collision;
    }

    public void ApproveClimb()
    {
        if (collisionWaitingForAnswer == null)
            return;
         
        Speech.SpeakText("Climb approved");
    }

    public void DisapproveClimb()
    {
        if (collisionWaitingForAnswer == null)
            return;

        Speech.SpeakText("Climb disapproved");
    }
}