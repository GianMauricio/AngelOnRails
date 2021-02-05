using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script will contain all the menu button functions
/// includes the options panel and level select panel as well.
/// </summary>
public class MainMenuButtons : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene(1);
    }

    public void toLastLevel()
    {
        //Depending on which level was last accessed; the player will resume at that level
        //(but all progress within the level will reset)

        switch (DataHolder.LevelBeingPlayed)
        {
            default:
                //Reload menu scene if the last level was not found
                Debug.Log("Last level returns unknown");

                //Restart menu scene if the menu scene was active prior
                SceneManager.UnloadSceneAsync("MenuScene");
                SceneManager.LoadScene("MenuScene", LoadSceneMode.Additive);
                break;
        }
    }

    public void addCoins(int coins)
    {
        DataHolder.deposit(coins);
    }

    public void takeCoins(int coins)
    {
        if (!DataHolder.withdraw(coins))
        {
            Debug.Log("Action illegal");
        }
    }
}
