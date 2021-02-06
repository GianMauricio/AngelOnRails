using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    //ID's of things
    public const string adInterstitial = "video";
    public const string adRewarded = "rewardedVideo";
    public const string adBanner = "sampleBanner";

    //Actual ad event for interstitial and vid ads
    public event EventHandler<AdFinishEventArgs> OnAdDone;


    //Reference to self...?
    //public AdsManager adManager;
   // public DataHolder data;

    
   

    public string GameID
    {
        get
        {
#if UNITY_ANDROID
            return "3999029";

#elif UNITY_IOS
            return "3999028";
#endif
            return "check";
        }
    }

    public void Start()
    {
        //Initialize
        Advertisement.Initialize(GameID, true);

        Advertisement.AddListener(this);
        //ShowBannerAd();
    //   adManager.OnAdDone += AdsManager_OnAdDone;

    }
    
    private void AdsManager_OnAdDone(object sender, AdFinishEventArgs e)
    {
        //Check if the fully finished
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
                   
                    Debug.Log("Ad completed Ad manager");
                   
                  
                    break;
            }
        }
    }
    public void ShowInterstitialAd()
    {
        if (Advertisement.IsReady(adInterstitial))
        {
            Advertisement.Show(adInterstitial);
        }
        else
        {
            Debug.Log("No Ads!");
        }
    }
    public void StartBanner()
    {
        ShowBannerAd(); /*may not start sometimes, attempt to move this somewhere else...?*/ //TODO:DONE
    }

    public void ShowBannerAd()
    {
        //Try start banner ad
        StartCoroutine(BannerAdCoRoutine());
    }

    //Probs will never be used...?
    public void HideBannerAd()
    {
        //If ad is loaded, then hide the ad
        if (Advertisement.Banner.isLoaded)
        {
            Advertisement.Banner.Hide();
        }
    }

    //THIS MUST NOT MOVE I DON'T KNOW WHY BUT IT MUST NOT MOVE
    IEnumerator BannerAdCoRoutine()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }

        //Set position
        Advertisement.Banner.SetPosition(BannerPosition.TOP_LEFT);

        //Show ad
        Advertisement.Banner.Show(adBanner);
    }

    //Implementations of the UnityAdsListener interface
    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log($"Done Loading {placementId}");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log($"Ads error: {message}");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log($"Started Ad {placementId}");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        //Check if the listener is ready
        if (OnAdDone != null)
        {
            //Make the args!
            AdFinishEventArgs args = new AdFinishEventArgs(placementId, showResult);

            //Fire!!!!!
            OnAdDone(this, args);
        }
    }

    public void ShowRewardedAd()
    {
        //Check if ad is ready
        if (Advertisement.IsReady(adRewarded))
        {
            //Surprise ADS!
            Advertisement.Show(adRewarded);
        }

        else
        {
            //No ads found
            Debug.Log("No ads ; Ad manager;");
        }
    }
}
