using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adapter : MonoBehaviour
{
    public enum RewardedState
    { 
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
        //Event로 전달 받을 경우 전달이 안되거나, 게임이 터지는 경우가 있을 수 있음.

        //상태 변경
        if(nowRewarded != newRewarded)
        {

            nowRewarded = newRewarded;
            if (changeRewarded != null)
                changeRewarded(nowRewarded);



        }
    }


}
