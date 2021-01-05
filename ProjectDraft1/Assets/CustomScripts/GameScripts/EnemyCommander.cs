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

    //Track active enemies
    private int enemiesAlive = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Get total wave count
        nWavesLeft = Waves.Count;
    }

    //Get all enemies still alive
    public int enemiesRemaining()
    {
        checkEnemies();
        return enemiesAlive;
    }

    public void DeployNextWave()
    {
        if (nCurrWave < Waves.Count)
        {
            //Activate requested wave
            Waves[nCurrWave].SetActive(true);

            enemiesAlive = 0;
            foreach (Transform child in Waves[nCurrWave].transform)
            {
                if (child.CompareTag("BasicEnemy") || child.CompareTag("ShieldedEnemy") ||
                    child.CompareTag("ArmoredEnemy") || child.CompareTag("ProtectedEnemy"))
                {
                    enemiesAlive++;
                }
            }

            nCurrWave++;
        }
    }

    private void checkEnemies()
    {
        enemiesAlive = 0;
        foreach (Transform child in Waves[nCurrWave].transform)
        {
            if (child.CompareTag("BasicEnemy") || child.CompareTag("ShieldedEnemy") ||
                child.CompareTag("ArmoredEnemy") || child.CompareTag("ProtectedEnemy"))
            {
                enemiesAlive++;
            }
        }
    }
}
