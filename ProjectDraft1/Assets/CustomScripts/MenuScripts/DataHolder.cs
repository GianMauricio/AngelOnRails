﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    static float MAX_DB = 18.0f;

    private float n_MusicVolume = 0.5f, n_SFXVolume = 0.5f, n_Brightness = 60;

    [Header("Audio")] 
    public AudioMixer Music;
    public AudioMixer SFX;

    //Level data
    private bool[] levelsCompleted = new bool[10];
    private int lastlevel = 1;

    //Set functions
    public void setMusicVol(float level)
    {
        n_MusicVolume = level;
        Debug.Log("New music volume " + n_MusicVolume);

        Music.SetFloat("MusicVol", Mathf.Log10(n_MusicVolume) * MAX_DB);
    }

    public void setSFXVol(float level)
    {
        n_SFXVolume = level;
        Debug.Log("New SFX volume " + n_SFXVolume);

        SFX.SetFloat("GunVol", Mathf.Log10(n_SFXVolume) * MAX_DB);
    }

    public void setBright(float level)
    {
        n_Brightness = level;
        Debug.Log("New brightness level " + n_Brightness);
    }

    public void completeLevel(int levelDesignation)
    {
        //Set requested level to completed
        levelsCompleted[levelDesignation] = true;
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
}
