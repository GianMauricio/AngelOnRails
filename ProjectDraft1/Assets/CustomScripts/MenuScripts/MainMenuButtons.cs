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
    public GameObject dataHolder;

    private DataHolder mDataHolder;

    private void Start()
    {
        mDataHolder = dataHolder.GetComponent<DataHolder>();

        //If the data holder is not found then the program will fail catastrophically
        if (mDataHolder == null)
        {
            Debug.LogError("Data holder missing");
        }
    }

    public void play()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(0);
    }

    public void toLastLevel()
    {
        //Depending on which level was last accessed; the player will resume at that level
        //(but all progress within the level will reset)

        switch (mDataHolder.queryLast())
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
}
