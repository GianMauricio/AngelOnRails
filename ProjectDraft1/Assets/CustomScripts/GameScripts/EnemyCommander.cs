using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommander : MonoBehaviour
{
    //Set totals waves
    public List<GameObject> Waves;

    //Track current wave
    private int nCurrWave;
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
        //For every wave *Note: use hard logic, avoid var at all costs
        foreach (GameObject child in Waves)
        {
            //For every enemy
            for (int j = 1; j < child.transform.childCount; j++) /*j is one to skip the spawn door*/
            {
                EnemyLogic currChild = child.transform.GetChild(j).GetComponent<EnemyLogic>();

                if (currChild.isDead())
                {
                    enemiesAlive++;
                }
            }
        }

        //Debug.Log(enemiesAlive);
        return enemiesAlive;
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
