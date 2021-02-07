using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
/// <summary>
/// Script: "What is my purpose?"
/// Me: "You change one text box"
/// Script: "Oh GOD ; A; "
/// </summary>
public class ShopButtons : MonoBehaviour
{
    public TextMeshProUGUI currCoins;
    public AdsManager adManager;
    public MainMenuButtons MainMenu;


    public void Start()
    {
        UIUpdate();
       
        adManager.OnAdDone += ShopButton_OnAdDone;

        Debug.Log("Are You Starting");
    }

    public void UIUpdate()
    {
        currCoins.text = DataHolder.getCoins().ToString();
    }

    private void ShopButton_OnAdDone(object sender, AdFinishEventArgs e)
    {
        Debug.Log("CheckOnadDoneShop");
        if (e.PlacementID == AdsManager.adRewarded)
        {
            switch (e.AdShowResult)
            {
                case ShowResult.Failed:
                    Debug.Log("Ad failed to show");
                    break;
                case ShowResult.Skipped:
                    Debug.Log("Betrayal!! ; A;");
                    break;
                case ShowResult.Finished:
                    MainMenu.addCoins(120);
                    currCoins.text = DataHolder.getCoins().ToString();
                    Debug.Log("Ad is finished completed");
                    break;
            }
        }
        Debug.Log("ShopButtonScript ad done ");

    }
}
