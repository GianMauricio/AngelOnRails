using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Unity.Notifications.Android;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

/// <summary>
/// This script holds the data to be used by all scenes in the game.
/// Settings data will be updated and retrieved here.
/// The level data (completion and last played will also be stored here)
/// </summary>
public class DataHolder : MonoBehaviour
{
    //Settings data
    private static float DEFAULT_MUSICVOL = 0.5f;
    private static float DEFAULT_SFXVOL = 0.5f;
    private static float DEFAULT_BRIGHTNESS = 60;
    private static float MAX_DB = 18.0f;
    private float n_MusicVolume = 0.1f, n_SFXVolume = 0.1f, n_Brightness = 60;

    //UI
    public GameObject LevelDoneUI;
    private int LevelBeingPlayed;

    [Header("Audio")] 
    public AudioMixer Music;
    public AudioMixer SFX;

    //Level data
    private bool[] levelsCompleted = new bool[10];
    private int lastlevel = 1;

    //Shop data
    private int coins, damageUpLead = 1, damageUpHLead = 1, damageUpBLead = 1;

    //Ad manager
    public AdsManager adManager;

   


    //Keep an eye on this; it may cause issues since the scene it's in is always awake
    private void Awake()
    {
        coins = 0;
        NotifChannel();

        adManager.OnAdDone += DataHolder_OnAdDone;
         Debug.Log("Woke up");
    }


    //Set functions
    public void setMusicVol(float level)
    {
        n_MusicVolume = level;
        //Debug.Log("New music volume " + n_MusicVolume);

        Music.SetFloat("MusicVol", Mathf.Log10(n_MusicVolume) * MAX_DB);
    }

    public void setSFXVol(float level)
    {
        n_SFXVolume = level;
        //Debug.Log("New SFX volume " + n_SFXVolume);

        SFX.SetFloat("GunVol", Mathf.Log10(n_SFXVolume) * MAX_DB);
    }

    public void setBright(float level)
    {
        n_Brightness = level;
        Debug.Log("New brightness level " + n_Brightness);
    }

    public void resetProgress()
    {
        //Set all levels to uncompleted
        for (int i = 0; i < levelsCompleted.Length; i++)
        {
            levelsCompleted[i] = false;
        }

        //TODO:Reset shop upgrades
    }

    public void setLastLevel(int level)
    {
        lastlevel = level;
    }

    //Get functions
    public bool queryLevel(int requestLevel)
    {
        return levelsCompleted[requestLevel];
    }

    public int queryLast()
    {
        return lastlevel;
    }

    public int getLeadRank()
    {
        return damageUpLead;
    }

    public int getHLeadRank()
    {
        return damageUpHLead;
    }

    public int getBLeadRank()
    {
        return damageUpBLead;
    }

    public int getCoins()
    {
        //coins = 0;
        Debug.Log(coins);
        deposit(10); //testing if deposit works it does 
                     //addendum it seems like whats happening is that dataholder is coming from different areas
                     //DataHolder of shop is different from data holder of show rewarded ad
                     //Data is persistent between play sessions

        return coins;
    }

    //Reset functions
    public void setAudioDefs()
    {
        n_MusicVolume = DEFAULT_MUSICVOL;
        n_SFXVolume = DEFAULT_SFXVOL;

        Debug.Log("New Audio level " + DEFAULT_MUSICVOL);
    }

    public void setDispDefs()
    {
        n_Brightness = DEFAULT_BRIGHTNESS;
        Debug.Log("New brightness level " + DEFAULT_BRIGHTNESS);
    }

    //Shop functions

    /// <summary>
    /// Add coins to player
    /// </summary>
    /// <param name="increase">amount to add</param>
    public void deposit(int increase)
    {
        //Debug.Log("You now have " + coins + " coins");
        coins += increase;
    }

    /// <summary>
    /// Returns false if the player does not have enough coins
    /// </summary>
    /// <param name="decrease">amount to take</param>
    /// <returns></returns>
    private bool withdraw(int decrease)
    {
        //Debug.Log("Loans are illegal");
        if (coins - decrease > 0)
        {
            coins -= decrease;
        }

        //Debug.Log("You now have " + coins + " coins");
        return coins - decrease > 0;
    }

    //Here's where the fun begins
    public void upLead(int cost)
    {
        if (withdraw(cost))
        {
            Debug.Log("Increasing lead damage");
            damageUpLead++;
        }

        else
        {
            Debug.Log("It smell like BROKE in here");
        }
    }

    public void upHLead(int cost)
    {
        if (withdraw(cost))
        {
            Debug.Log("Increasing Hlead damage");
            damageUpHLead++;
        }

        else
        {
            Debug.Log("It smell like BROKE in here");
        }
    }

    public void upBLead(int cost)
    {
        if (withdraw(cost))
        {
            Debug.Log("Increasing Blead damage");
            damageUpBLead++;
        }

        else
        {
            Debug.Log("It smell like BROKE in here");
        }
    }

    //Notification stuff
    public void NotifChannel()
    {
        //Channel init data
        string ChID = "MainNotifs";
        string title = "Angel On Rails";
        Importance importance = Importance.Default;
        string desc = "Send out AOR notifs";

        //Notif panel struct
        AndroidNotificationChannel ch = new AndroidNotificationChannel(ChID, title, desc, importance);

        //Registry entry
        AndroidNotificationCenter.RegisterNotificationChannel(ch);
    }

    //Send current coin count
    public void SendScoreNotif()
    {
        //Data
        string header = "Current Coins:";
        string msg = coins.ToString();

        //Timeline
        System.DateTime deployNotifTime = System.DateTime.Now.AddSeconds(2);

        //Notif go pewpew
        AndroidNotification newNotif = new AndroidNotification(header, msg, deployNotifTime);

        //Prev Score data
        newNotif.IntentData = coins.ToString();

        //TODO:Gura Icon stuff
        newNotif.SmallIcon = "gura";
        newNotif.LargeIcon = "guralarge";

        AndroidNotificationCenter.SendNotification(newNotif, "MainNotifs");
    }

    /// <summary>
    /// make this do stuff
    /// </summary>
    public void parseData()
    {

    }

    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(LevelBeingPlayed);
    }

    //Go to next level
    public void invokeLevelEndUI(int level, bool isDone)
    {
        LevelDoneUI.SetActive(true);
        LevelBeingPlayed = level;
        if (isDone)
        {
            levelsCompleted[level] = true;
        }
    }

    //Added the admanager adding currency when ad is done
    private void DataHolder_OnAdDone(object sender, AdFinishEventArgs e)
    {
        if(e.PlacementID == AdsManager.adRewarded)
        {
            switch (e.AdShowResult)
            {
                case ShowResult.Failed:
                    Debug.Log("Ad failed to show");
                    break;
                case ShowResult.Skipped:
                    Debug.Log("Betrayal!! ; A;");
                    break;
                case ShowResult.Finished:
                    deposit(120);
                   
                    Debug.Log("Ad is finished completed");
                    
                    break;
            }
        }
        Debug.Log("Dataholder ad done " + coins);
       
    }
}
