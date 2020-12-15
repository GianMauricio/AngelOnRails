using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script does nothing on it's own until ordered by player scripts
public class UIScript : MonoBehaviour
{
    //Track UI elements
    public Image healthBar, ammoBar, enemyBar, levelBar;

    //Track maximums
    private float maxHealth, maxAmmo;
    private int waypointsTotal, enemiesTotal;

    //TODO: WeaponSlider

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
        healthBar.fillAmount = newhealth / maxHealth;
    }

    public void setAmmo(float newAmmo)
    {
        ammoBar.fillAmount = newAmmo / maxAmmo;
    }

    public void setProgress(float newProgress)
    {
        ammoBar.fillAmount = newProgress / waypointsTotal;
    }

    public void setKillCount(float newKills)
    {
        enemyBar.fillAmount = (enemiesTotal - newKills) / enemiesTotal;
    }
}