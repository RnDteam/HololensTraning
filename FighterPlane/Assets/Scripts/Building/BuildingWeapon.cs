using System;
using UnityEngine;

public class BuildingWeapon : MonoBehaviour {

    public Weapon Weapon;
    
    void Start () {
        Array values = Enum.GetValues(typeof(Weapon));
        System.Random random = new System.Random();
        Weapon = (Weapon)values.GetValue(random.Next(values.Length)); 
    }
}
