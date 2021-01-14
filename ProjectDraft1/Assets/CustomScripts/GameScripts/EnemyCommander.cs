using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void DeployNextWave()
    {
        if (nCurrWave < Waves.Count)
        {
            //Activate requested wave
            Waves[nCurrWave].SetActive(true);
            nCurrWave++;
        }
    }
}
