using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script serves to track the gun data of whatever fire arm the player currently has equipped (at base level)
/// </summary>
public class WeaponTracker : MonoBehaviour
{
    private string GunType = "None";

    public void setLead()
    {
        Debug.Log("Switched To Lead");
        GunType = "Lead";
    }

    public void setHeavyLead()
    {
        Debug.Log("Switched To HeavyLead");
        GunType = "HeavyLead";
    }

    public void setEnergy()
    {
        Debug.Log("Switched To Blessed");
        GunType = "Blessed";
    }

    public string getType()
    {
        return GunType;
    }

    public float getDamage()
    {
        float damage = 0;

        if (GunType == "Lead")
        {
            damage = 10;
        }
        else if (GunType == "HeavyLead")
        {
            damage = 15;
        }
        else if (GunType == "Blessed")
        {
            damage = 20;
        }
        return damage;

    }
}
