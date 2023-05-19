using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Initializer : MonoBehaviour
{
    // Use this for initialization


    
    void Start()
    {

        if (Adapter.Instance != null)
            Debug.Log("Adapter Create.");

        //admob 초기화
        AdmobManager.Instance.initAdmob();

     
        //프레임 고정
        Application.targetFrameRate = 60;


        //플레이중에 화면이 꺼지지 않게 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep;


        CA.DataManager.Instance.load();

        //다음씬으로 이동
        StartCoroutine(initEnd());

    }

    
    IEnumerator initEnd()
    {
        yield return new WaitForSeconds(2.0f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(ConstantData.SCENE_TITLE);

    }




}