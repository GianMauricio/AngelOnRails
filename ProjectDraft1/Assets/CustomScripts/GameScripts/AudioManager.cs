using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource LeadShot;
    public AudioSource HLeadShot;
    public AudioSource BlessedShot;
    public AudioSource LeadRel;
    public AudioSource HLeadRel;
    public AudioSource BlessedRel;

    /// <summary>
    /// Play audio based on gun type and current action
    /// </summary>
    /// <param name="type">Lead, HLead or Blessed</param>
    /// <param name="mode">1 = Shoot, 2 = Reload</param>
    public void playSound(string type, int mode)
    {

        //Shooting sounds
        if (mode == 1)
        {
            //Debug.Log("Playing shootSound for: ");
            if (type == "Lead")
            {
                //Debug.Log("Lead");
                LeadShot.Play();
            }

            else if (type == "HLead")
            {
                //Debug.Log("HLead");
                HLeadShot.Play();
            }

            else if (type == "Blessed")
            {
                //Debug.Log("Blessed");
                BlessedShot.Play();
            }

            else
            {
                Debug.Log("Here be static");
            }
        }

        //Reload sounds
        else if (mode == 2)
        {
            //Debug.Log("Playing relSound for: ");
            if (type == "Lead")
            {
                //Debug.Log("Lead");
                LeadRel.Play();
            }

            else if (type == "HLead")
            {
                //Debug.Log("HLead");
                HLeadRel.Play();
            }

            else if (type == "Blessed")
            {
                //Debug.Log("Blessed");
                BlessedRel.Play();
            }

            else
            {
                Debug.Log("Here be static");
            }
        }
    }

    public bool audioActive()
    {

        //This is terrible for readability. Too bad!
        return LeadShot.isPlaying || HLeadShot.isPlaying || BlessedShot.isPlaying
               || LeadRel.isPlaying || HLeadRel.isPlaying || BlessedRel.isPlaying; ;
    }
}
