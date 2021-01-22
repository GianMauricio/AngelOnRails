using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script does nothing on it's own until ordered by player scripts
public class UIScript : MonoBehaviour
{
    //Track UI elements
    public Image healthBar, ammoBar, enemyBar, levelBar, hurtUI;
    public GameObject levelCompleteUI;

    //Track maximums
    private float MAXHEALTH = 100;

    // Start is called before the first frame update
    void Start()
    {
        //set all fills to 100%
        healthBar.fillAmount = 1;
        ammoBar.fillAmount = 1;
        enemyBar.fillAmount = 1;
        levelBar.fillAmount = 1;
    }

    public void setHealth(float newhealth)
    {
        healthBar.fillAmount = newhealth / MAXHEALTH;

        //Calc transparency using inverse of current health
        float newAlpha = 1 - (newhealth / MAXHEALTH);

        //Terrible method but fuck it it works
        hurtUI.color = new Color(hurtUI.color.a, hurtUI.color.b, hurtUI.color.g, newAlpha);
    }

    public void setAmmo(float newAmmo, float maxAmmo)
    {
        ammoBar.fillAmount = newAmmo / maxAmmo;
    }

    public void setKillCount(float enemiesLeft, float enemiesTotal)
    {
        enemyBar.fillAmount = enemiesLeft / enemiesTotal;
    }

    public void setProgress(float waypointsRemaining, float waypointsTotal)
    {
        levelBar.fillAmount = waypointsRemaining / waypointsTotal;
    }

    public void hide()
    {
        gameObject.SetActive(false);
    }

    public void show()
    {
        gameObject.SetActive(true);
    }
}