using AdiveryUnity;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdiveryConfig : MonoBehaviour
{
    // Start is called before the first frame update
    public static AdiveryConfig instance;
    public string APP_ID;
    public string InterstitialAd;
    public string RewardedVideoAd;
    public string BannerIDAd;

    AdiveryListener listener;
    AdiveryListener listenerVideo;
   [HideInInspector] public BannerAd bannerAd;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this.gameObject); }
      
    }
    void Start()
    {
   
        Adivery.Configure(APP_ID);

        // InterstitialAd

        Adivery.PrepareInterstitialAd(InterstitialAd);
        listener = new AdiveryListener();

        listener.OnError += OnError;
        listener.OnInterstitialAdLoaded += OnInterstitialAdLoaded;

        Adivery.AddListener(listener);


        //RewardedVideo

        Adivery.PrepareRewardedAd(RewardedVideoAd);

        listenerVideo = new AdiveryListener();

        listenerVideo.OnError += OnError;
        listenerVideo.OnRewardedAdLoaded += OnRewardedLoaded;
        listenerVideo.OnRewardedAdClosed += OnRewardedClosed;

        Adivery.AddListener(listenerVideo);

        //banner ad
        bannerAd = new BannerAd(BannerIDAd, BannerAd.TYPE_BANNER, BannerAd.POSITION_TOP);
       // bannerAd.OnAdLoaded += OnBannerAdLoaded;
       
        if (UnityEngine.PlayerPrefs.GetInt("status") == 0 )
        {
            bannerAd.LoadAd();
        }
        Invoke("OnBannerAdShow", 10);
    }

    public void OnBannerAdShow()
    {
        if (UnityEngine.PlayerPrefs.GetInt("status") == 0 )
        {
            bannerAd.Show();
        }
      
        // bannerAd.Hide(); می‌توانید در هر زمانی نمایش تبلیغ بنری را غیرفعال کنید.
    }
    public void hideBanner()
    {
        bannerAd.Hide();
    }
    public void OnRewardedLoaded(object caller, string placementId)
    {
        // Rewarded ad loaded
    }

    public void OnRewardedClosed(object caller, AdiveryReward reward)
    {
        // Check if User should receive the reward
        if (reward.IsRewarded)
        {
           // getRewardAmount(reward.PlacementId); // Implrement getRewardAmount yourself
        }
    }

    public void OnError(object caller, AdiveryError args)
    {
      //  Debug.Log("placement: " + error.PlacementId + " error: " + error.Reason);
    }
    public void OnInterstitialAdLoaded(object caller, string placementId)
    {
        // Interstitial ad loaded
    }

  
    public void showInterstitialAd()
    {
        if (UnityEngine.PlayerPrefs.GetInt("status") == 0 )
        {
            if (Adivery.IsLoaded(InterstitialAd))
            {
                Adivery.Show(InterstitialAd);
            }
        }
      
    }

    public void showRewardedAd()
    {
        if (UnityEngine.PlayerPrefs.GetInt("status") == 0 )
        {
            if (Adivery.IsLoaded(RewardedVideoAd))
            {
                Adivery.Show(RewardedVideoAd);
            }
        }
       
    }

    public void showRewardedAllTime()
    {
        
            if (Adivery.IsLoaded(RewardedVideoAd))
            {
                Adivery.Show(RewardedVideoAd);
             
            }
        

    }


}
