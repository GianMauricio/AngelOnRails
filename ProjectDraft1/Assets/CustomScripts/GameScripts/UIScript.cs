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
    private float maxHealth;

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

    public void setAmmo(float newAmmo, float maxAmmo)
    {
        ammoBar.fillAmount = newAmmo / maxAmmo;
    }

    public void setKillCount(float enemiesLeft, float enemiesTotal)
    {
        enemyBar.fillAmount = enemiesLeft / enemiesTotal;
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