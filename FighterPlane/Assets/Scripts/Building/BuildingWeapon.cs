using System;
using UnityEngine;

public class BuildingWeapon : MonoBehaviour {

    public Weapon Weapon;
    
    void Start () {
        long id;
        if (long.TryParse(GetComponent<OnlineMapsBuildingBase>().id, out id))
        {
            if (id % 2 == 0)
            {
                Weapon = Weapon.Rocket;
            }
            else
            {
                Weapon = Weapon.Missile;
            }
        }
        else
        {
            Weapon = Weapon.None;
        }
    }
}
