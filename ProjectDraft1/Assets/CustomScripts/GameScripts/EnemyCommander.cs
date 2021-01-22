using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCommander : MonoBehaviour
{
    //Set totals waves
    public List<GameObject> Waves;

    //Track current wave
    private int nCurrWave = 0;
    private int nWavesLeft;

    // Start is called before the first frame update
    void Start()
    {
        //Get total wave count
        nWavesLeft = Waves.Count;
    }

    //Get all enemies still alive
    public int enemiesRemaining()
    {
        var enemiesAlive = 0;
        //This logic took me 3 fucking days to fix goddamn it
        //For every wave *Note: use hard logic in loops, avoid var at all costs
        foreach (GameObject child in Waves)
        {
            //For every enemy
            for (int j = 0; j < child.transform.childCount; j++)
            {
                //if the child is not an enemy do not execute this
                if (child.transform.GetChild(j).GetComponent<EnemyLogic>() == null) continue;

                EnemyLogic currChild = child.transform.GetChild(j).GetComponent<EnemyLogic>();
                //Debug.Log(currChild.isDead());

                if (!currChild.isDead())
                {
                    enemiesAlive++;
                }
            }
        }

        //Debug.Log(enemiesAlive);
        return enemiesAlive;
    }

    public int enemiesTotal()
    {
        var enemiesSpawned = 0;
        //For every enemy in the current wave
        foreach (Transform child in Waves[nCurrWave - 1].transform)
        {
            if (child.GetComponent<EnemyLogic>() == null) continue;
            enemiesSpawned++;
        }

        //Debug.Log(enemiesSpawned);
        return enemiesSpawned;
    }

    public Vector3 DeployNextWave()
    {
        Vector3 targetOrientation = new Vector3(0,0,0);

        if (nCurrWave < Waves.Count)
        {
            //Activate requested wave
            Waves[nCurrWave].SetActive(true);

            //Get the position of the spawn door and tell the player to look at it
            Transform EnemyOrigin = Waves[nCurrWave].transform.GetChild(0);
            targetOrientation = EnemyOrigin.position;

            nCurrWave++;
        }

        return targetOrientation;
    }

    /// <summary>
    /// Tells a random enemy to generate a shot based on their type and rank
    /// This function is badly implemented as it technically means that the player is asking enemies to shoot them
    /// But this is the only way to implement this without dynamic shoot functions that rely on searching for the player
    /// </summary>
    public float allowShot()
    {
        //Debug.Log("Shot request received");
        float shotDamage = 0;

        //This implementation is slow.
        //But it's faster than randomizing a number then checking if the number is legal
        //Fuck you're right
        //But this means weird gameplay since enemies will mostly likely fire in an ordered manner
        //Ask for next enemy (that is still alive)
        foreach (Transform child in Waves[nCurrWave - 1].transform)
        {
            if (child.GetComponent<EnemyLogic>() != null)
            {
                shotDamage = child.GetComponent<EnemyLogic>().requestDamage();
                break; /*uSinG brEaK iS bAd imPlem-- well yeah this is terrible implementation. Too Bad!*/
            }
        }

        return shotDamage;
    }
}
