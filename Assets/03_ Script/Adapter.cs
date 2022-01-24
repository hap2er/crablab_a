using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adapter : MonoBehaviour
{


    public enum RewardedState
    {
        //
        None,

        Show,
        Fail,
        Success
    }

    private RewardedState nowRewarded = RewardedState.None;

    public RewardedState newRewarded = RewardedState.None;

    static Adapter _instance = null;


    public static Adapter Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Adapter>();

                if (_instance == null)
                {
                    _instance = new GameObject("Adapter").AddComponent<Adapter>(); ;
                }
            }


            return _instance;
        }
    }



    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }



    public delegate void ChangeRewarded(RewardedState state);

    public ChangeRewarded changeRewarded;



    public void Update()
    {
        //상태 변경
        if(nowRewarded != newRewarded)
        {

            nowRewarded = newRewarded;
            if (changeRewarded != null)
                changeRewarded(nowRewarded);



        }
    }


}
