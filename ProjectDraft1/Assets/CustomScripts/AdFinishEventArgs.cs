using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdFinishEventArgs : EventArgs
{
    //Args properties
    public string PlacementID
    {
        private set; get;
    }

    public UnityEngine.Advertisements.ShowResult AdShowResult
    {
        private set; get;
    }

    //Constructor
    public AdFinishEventArgs(string id, UnityEngine.Advertisements.ShowResult result)
    {
        PlacementID = id;
        AdShowResult = result;
    }
}
