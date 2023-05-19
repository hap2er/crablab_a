using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnim : MonoBehaviour
{

    RectTransform tr;


    private void Awake()
    {
        tr = this.GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        CA.SoundManager.Instance.StartEffect(CA.Sound.Angry);
        StartCoroutine(anim());
    }


    IEnumerator anim()
    {

        tr.localScale = Vector3.one * 2f;


        for(int i=0;i<10;i++)
        {
            tr.localScale -= Vector3.one * 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        tr.localScale = Vector3.one;

    }
}
