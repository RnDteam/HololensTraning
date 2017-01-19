using System;
using UnityEngine;

public class BuildingWeapon : MonoBehaviour {

    public Weapon Weapon;
    public string asd = "asd";
    
    void Start () {
        long id;
        if (long.TryParse(GetComponent<OnlineMapsBuildingBase>().id, out id))
        {
            if (id % 2 == 0)
            {
                Weapon = Weapon.Missile;

            }
            else
            {
                Weapon = Weapon.Rocket;
            }
        }
        else
        {
            Weapon = Weapon.None;
        }
        
    }
}
