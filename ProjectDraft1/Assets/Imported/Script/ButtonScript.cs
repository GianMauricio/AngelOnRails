using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// All button calls are handled here.
/// </summary>
public class ButtonScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("PlayerSceneEdit");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
