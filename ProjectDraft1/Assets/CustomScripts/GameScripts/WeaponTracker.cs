using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script serves to track the gun data of whatever fire arm the player currently has equipped (at base level)
/// </summary>
public class WeaponTracker : MonoBehaviour
{
    //Manage UI
    public UIScript HUD;
    private string GunType = "Lead";

    //Manage Audio
    public AudioManager SoundBoard;

    //Ammunition data
    private int MAXLEAD = 100, MAXHLEAD = 50, MAXBLESS = 20;
    private int leadAmmo = 100, heavyAmmo = 50, blessedAmmo = 20;
    private int leadClips = 3, heavyClips = 2, blessedClips = 1;

    //Switch gun to selected type
    public void setLead()
    {
        //Debug.Log("Switched To Lead");
        GunType = "Lead";
        updateUI();
    }

    public void setHeavyLead()
    {
        //Debug.Log("Switched To HeavyLead");
        GunType = "HeavyLead";
        updateUI();
    }

    public void setEnergy()
    {
        //Debug.Log("Switched To Blessed");
        GunType = "Blessed";
        updateUI();
    }

    //Reload current gun based on gun type
    public void tryReload()
    {
        switch (GunType)
        {
            case "Lead":
            {
                Debug.Log("Reloading lead");
                if (leadClips > 0)
                {
                    leadClips--;
                    leadAmmo = MAXLEAD;
                }

                else
                {
                    Debug.Log("No clips");
                    //TODO: make the clicky sound effect when you have no ammo
                }

                break;
            }
            case "HeavyLead":
            {
                Debug.Log("Reloading Hlead");
                if (heavyClips > 0)
                {
                    heavyClips--;
                    heavyAmmo = MAXHLEAD;
                }

                else
                {
                    Debug.Log("No clips");
                    //TODO: make the clicky sound effect when you have no ammo
                }

                break;
            }
            case "Blessed":
            {
                Debug.Log("Reloading Blessings");
                if (blessedClips > 0)
                {
                    blessedClips--;
                    blessedAmmo = MAXBLESS;
                }

                else
                {
                    Debug.Log("No clips");
                    //TODO: make the clicky sound effect when you have no ammo
                }

                break;
            }
        }

        //Tell UI to update:
        updateUI();
    }

    //Tell UI that an enemy died
    public void tryKill(float enemiesLeft, float enemiesTotal)
    {
        HUD.setKillCount(enemiesLeft, enemiesTotal);
    }

    //Retrieve data
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

    public int ammoLeft()
    {
        if (GunType == "Lead")
        {
            return leadAmmo;
        }

        else if (GunType == "HeavyLead")
        {
            return heavyAmmo;
        }

        else if (GunType == "Blessed")
        {
            return blessedAmmo;
        }

        return 0;
    }

    public void shotFired()
    {
        if (GunType == "Lead")
        {
            leadAmmo--;
        }

        else if (GunType == "HeavyLead")
        {
            heavyAmmo--;
        }

        else if (GunType == "Blessed")
        {
            blessedAmmo--;
        }

        else
        {
            Debug.Log("wtf are you firing?");
        }

        updateUI();
    }

    //Display data (internal ref)
    private int getMax()
    {
        if (GunType == "Lead")
        {
            return MAXLEAD;
        }

        else if (GunType == "HeavyLead")
        {
            return MAXHLEAD;
        }

        else if (GunType == "Blessed")
        {
            return MAXBLESS;
        }

        return 1;
    }

    private void updateUI()
    {
        HUD.setAmmo(ammoLeft(), getMax());
    }

    public void shiftState(bool isMoving)
    {
        if (isMoving)
        {
            HUD.hide();
        }

        else
        {
            HUD.show();
        }
    }

    //Audio data
    /// <summary>
    /// Play sound based on current weapon and prompt from the player manager
    /// </summary>
    /// <param name="prompt">Pew for shot sound, Rel for reload sound</param>
    public void playAudio(string prompt)
    {
        switch (prompt)
        {
            //Shoot
            case "Pew" when GunType == "Lead":
                SoundBoard.playSound("Lead", 1);
                break;
            case "Pew" when GunType == "HeavyLead":
                SoundBoard.playSound("HLead", 1);
                break;
            case "Pew" when GunType == "Blessed":
                SoundBoard.playSound("Blessed", 1);
                break;
            case "Pew":
                Debug.Log("TF gun do you have?");
                break;

            //Reload
            case "Rel" when GunType == "Lead":
                SoundBoard.playSound("Lead", 2);
                break;
            case "Rel" when GunType == "HeavyLead":
                SoundBoard.playSound("HLead", 2);
                break;
            case "Rel" when GunType == "Blessed":
                SoundBoard.playSound("Blessed", 2);
                break;
            case "Rel":
                Debug.Log("TF gun do you have?");
                break;
            default:
                Debug.Log("No such audio exists");
                break;
        }
    }
}
