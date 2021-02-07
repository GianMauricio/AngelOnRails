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
    public static float n_MusicVolume = 0.1f, n_SFXVolume = 0.1f, n_Brightness = 60;

    [Header("Audio")] 
    public static AudioMixer Music;
    public static AudioMixer SFX;

    //Ad manager
    //public AdsManager adManager;


    /// <summary>
    /// Statics that will persists throughout the game
    /// </summary>
    //Level data
    public static bool[] levelsCompleted = new bool[10];
    public static int LevelBeingPlayed;

    //Shop data
    private static int coins, damageUpLead = 1, damageUpHLead = 1, damageUpBLead = 1;


    //Keep an eye on this; it may cause issues since the scene it's in is always awake
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        coins = 0;
        NotifChannel();

       // adManager.OnAdDone += DataHolder_OnAdDone;
        Debug.Log("Woke up");
    }


    //Set functions
    public static void setMusicVol(float level)
    {
        n_MusicVolume = level;
        //Debug.Log("New music volume " + n_MusicVolume);

        Music.SetFloat("MusicVol", Mathf.Log10(n_MusicVolume) * MAX_DB);
    }

    public static void setSFXVol(float level)
    {
        n_SFXVolume = level;
        //Debug.Log("New SFX volume " + n_SFXVolume);

        SFX.SetFloat("GunVol", Mathf.Log10(n_SFXVolume) * MAX_DB);
    }

    public static void setBright(float level)
    {
        n_Brightness = level;
        Debug.Log("New brightness level " + n_Brightness);
    }

    public static void resetProgress()
    {
        //Set all levels to uncompleted
        for (int i = 0; i < levelsCompleted.Length; i++)
        {
            levelsCompleted[i] = false;
        }

        //TODO:Reset shop upgrades
    }

    public static int getLeadRank()
    {
        return damageUpLead;
    }

    public static int getHLeadRank()
    {
        return damageUpHLead;
    }

    public static int getBLeadRank()
    {
        return damageUpBLead;
    }

    public static int getCoins()
    {
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
    public static void deposit(int increase)
    {
        //Debug.Log("You now have " + coins + " coins");
        coins += increase;
    }

    /// <summary>
    /// Returns false if the player does not have enough coins
    /// </summary>
    /// <param name="decrease">amount to take</param>
    /// <returns></returns>
    public static bool withdraw(int decrease)
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
    public static void upLead(int cost)
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

    public static void upHLead(int cost)
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

    public static void upBLead(int cost)
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

   
}
