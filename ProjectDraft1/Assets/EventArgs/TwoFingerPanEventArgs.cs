using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TwoFingerPanEventArgs : EventArgs
{
    public Touch Finger1
    {
        get;private set;
    }

    public Touch Finger2
    {
        get;private set;
    }

    public Directions direction
    {
        get;
        private set;
    }

    //Adding panDirection

    public TwoFingerPanEventArgs(Touch f1, Touch f2, Vector2 deltaChange)
    {
        Finger1 = f1;
        Finger2 = f2;

        //Calculate if the pan happened more left/right or up/down
        if (Math.Abs(deltaChange.x) > Math.Abs(deltaChange.y))
        {
            Debug.Log("HorizontalPan");
            //If left or right
            direction = deltaChange.x < 0 ? Directions.LEFT : Directions.RIGHT;
        }

        else
        {
            Debug.Log("VerticalPan");
            //If up or down
            direction = deltaChange.y > 0 ? Directions.UP : Directions.DOWN;
        }

        Debug.Log(direction);
    }
}
