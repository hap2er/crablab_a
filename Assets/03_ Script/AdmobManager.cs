using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
    #region Instance

    private static AdmobManager _instance = null;

    public static AdmobManager Instance
    {
        get
        {
            init();
            return _instance;
        }
    }

    private static void init()
    {
        if (_instance == null)
            _instance = FindObjectOfType<AdmobManager>();

        if (_instance == null)
        {
            GameObject obj = new GameObject(typeof(AdmobManager).Name);
            obj.AddComponent<AdmobManager>();
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion

    #region Initialize

    public void initAdmob()
    {
        Debug.unityLogger.logEnabled = true;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus =>
        {
            Dictionary<string, AdapterStatus> dic = initStatus.getAdapterStatusMap();

            foreach (KeyValuePair<string, AdapterStatus> pair in dic)
            {
                log("ADMOB initialize adapter : {0}, desc : {1} latency : {2} statue : {3}", pair.Key, pair.Value.Description, pair.Value.Latency, pair.Value.InitializationState);
            }

            createBannerAd(AdSize.Banner, AdPosition.Bottom, ConstantData.ADMOB_BANNER_UNIT_ID);
            //createMiddleAd(AdSize.MediumRectangle, AdPosition.Top, ConstantData.ADMOB_MIDDLE_UNIT_ID);
           // createFullAd(ConstantData.ADMOB_INTER_UNIT_ID);
            createRewardAd(ConstantData.ADMOB_REWARD_UNIT_ID);
        });
    }


    public void destroy()
    {
        if (mBannerView != null)
            mBannerView.Destroy();

        if (mFullView != null)
            mFullView.Destroy();
    }

    #endregion


    #region Banner

    BannerView mBannerView = null;
    AdSize mBannerSize = AdSize.Banner;
    AdPosition mBannerGravity = AdPosition.Top;
    string mBannerAdmobUnitId = null;
    int mRequestCount = 0;
    bool mbMiddleLoaded = false;

    public void createBannerAd(AdSize p_adsize, AdPosition p_gravity, string p_strAdmobUnitId)
    {
        mBannerView = null;
        mBannerSize = p_adsize;
        mBannerGravity = p_gravity;
        mBannerAdmobUnitId = p_strAdmobUnitId;

        createBannerAdmob();
    }

    public void AdmobBannerLoadFailed(object sender, AdFailedToLoadEventArgs args)
    {
        mBannerView = null;

        // 애드몹 광고를 호출 한다.
        createBannerAdmob();
    }

    private void createBannerAdmob()
    {
        mBannerView = null;

        mBannerView = new BannerView(mBannerAdmobUnitId, mBannerSize, mBannerGravity);


        AdRequest request = new AdRequest.Builder().Build();
        mBannerView.LoadAd(request);

        mBannerView.Hide();
    }
    private void OnPaidEventBanner(object sender, AdValueEventArgs args)
    {
        AdValue adValue = args.AdValue;


    }

    #endregion




    #region Interstitial

    private bool bInterAdReady = false;
    private InterstitialAd mFullView = null;
    private string mFullAdmobUnitId = null;


    public void createFullAd(string p_strAdmobUnitId)
    {
        mFullView = null;
        mFullAdmobUnitId = p_strAdmobUnitId;

        loadFullAdAdmob();
    }


    public void OnInterAdOpening(object sender, System.EventArgs args)
    {
        // 사운드 음소거 / サウンドを消す
        //Android는 자체적으로 음소거를 사용.

    }

    public void OnInterAdClosed(object sender, System.EventArgs args)
    {
        // 사운드 복구 / サウンド復旧

        //Android는 자체적으로 음소거를 사용.

        bInterAdReady = false;

        loadFullAdAdmob();
    }

    public void OnInterAdLoaded(object sender, System.EventArgs args)
    {
        bInterAdReady = true;
    }

    public void onInterAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

        mFullView = null;

        // 애드몹 광고를 호출 한다.
        createFullAdmob();
    }

    private void createFullAdmob()
    {
        loadFullAdAdmob();
    }

    private void loadFullAdAdmob()
    {
        bInterAdReady = false;

        mFullView = new InterstitialAd(mFullAdmobUnitId);

        mFullView.OnAdOpening += OnInterAdOpening;
        mFullView.OnAdClosed += OnInterAdClosed;
        mFullView.OnAdLoaded += OnInterAdLoaded;
        mFullView.OnAdFailedToLoad += OnInterAdFailedToLoad;


        AdRequest request = new AdRequest.Builder().Build();
        mFullView.LoadAd(request);
    }
    private void OnPaidEventInter(object sender, AdValueEventArgs args)
    {

        AdValue adValue = args.AdValue;
    }

    private void OnInterAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        log("OnInterAdFailedToLoad : {0}", e.LoadAdError.GetMessage());
    }

    #endregion


    #region AdHandle

    public void bannerOnlyView(bool isVisible)
    {
        if (mBannerView != null)
        {
            if (isVisible)
                mBannerView.Show();
            else
                mBannerView.Hide();
        }
    }


    

    public void fullOnlyView()
    {
        if (mFullView != null)
        {
            if (bInterAdReady)
            {
                mFullView.Show();
            }
            else
            {
                loadFullAdAdmob();
            }
        }
    }


  

    #endregion


    #region Reward

    private bool bRewardDone = false;

    private RewardedAd mRewardedVideoAd = null;
    private string mRewardAdmobUnitId = null;

    public bool isRewardReady = false;

    public void createRewardAd(string p_strAdmobUnitId)
    {
        mRewardedVideoAd = null;
        mRewardAdmobUnitId = p_strAdmobUnitId;
        bRewardDone = false;

        mRewardedVideoAd = new RewardedAd(mRewardAdmobUnitId);

        mRewardedVideoAd.OnAdLoaded += OnRewardBasedVideoLoaded;
        mRewardedVideoAd.OnAdOpening += OnRewardBasedVideoOpening;
        mRewardedVideoAd.OnUserEarnedReward += OnRewardBasedVideoRewarded;

        mRewardedVideoAd.OnAdClosed += OnRewardBasedVideoClosed;

        loadRewardedVideoAd();
    }
    public void OnPaidEventRewardedAd(object sender, AdValueEventArgs args)
    {
        AdValue adValue = args.AdValue;
    }
    public void OnRewardBasedVideoLoaded(object sender, System.EventArgs args)
    {
        isRewardReady = true;

        //버튼 온오프
        //adRewardBtnSetting();
    }

    public void OnRewardBasedVideoOpening(object sender, System.EventArgs args)
    {
        // 사운드 음소거 / サウンドを消す
        //setUnityPause(true);


        Adapter.Instance.newRewarded = Adapter.RewardedState.Show;

    }


    public void OnRewardBasedVideoRewarded(object sender, GoogleMobileAds.Api.Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;

        // set reward 
        Adapter.Instance.newRewarded = Adapter.RewardedState.Success;



    }


    private void createAdmobRewardAd()
    {
        bRewardDone = false;

        mRewardedVideoAd.OnAdClosed += OnRewardBasedVideoClosed;

        loadRewardedVideoAd();
    }


    public void OnRewardBasedVideoClosed(object sender, System.EventArgs args)
    {
        // 사운드 복구 / サウンド復旧


        createRewardAd(ConstantData.ADMOB_REWARD_UNIT_ID);

        if (bRewardDone)
        {
            bRewardDone = false;
        }

        if (Adapter.Instance.newRewarded != Adapter.RewardedState.Success)
            Adapter.Instance.newRewarded = Adapter.RewardedState.Fail;


    }

    public bool showRewardAd()
    {
        isRewardReady = false;

        if (mRewardedVideoAd != null)
        {
            if (mRewardedVideoAd.IsLoaded())
            {
                mRewardedVideoAd.Show();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        return false;
    }

    private void loadRewardedVideoAd()
    {
        if (mRewardedVideoAd != null)
        {
            mRewardedVideoAd.LoadAd(new AdRequest.Builder().Build());
        }
    }

    #endregion




    //		#endregion




    #region AdRequest




    #endregion


    #region Debug

    private void log(string format, params object[] args)
    {
        string text = string.Format(format, args);
        Debug.LogFormat("{0} - {1}", GetType().Name, text);
    }

    #endregion


}