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
        int enemies = 0;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Enemy")) enemies++;
        }


        Debug.Log("Enemies found: " + enemies);
        return enemies;
    }

    public void DeployNextWave()
    {
        //Activate requested wave
        Waves[nCurrWave].SetActive(true);
        nCurrWave++;
    }
}
