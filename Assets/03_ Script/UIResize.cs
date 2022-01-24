using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIResize : MonoBehaviour
{
    public RectTransform canvasRect;
    public RectTransform rect;


    private Vector2 cSize = Vector2.zero;

    public void Start()
    {
        if(canvasRect==null)
        {
            canvasRect= GameObject.Find("Canvas").GetComponent<RectTransform>();
        }
        rect = this.gameObject.GetComponent<RectTransform>();


        cSize = new Vector2(1080, 1920);
    }
    public void Update()
    {
        if(canvasRect.sizeDelta!=cSize)
        {
            Vector2 s2;
            //s2.x = rect.sizeDelta.x / cSize.x;
            //s2.y = rect.sizeDelta.y / cSize.y;

            s2.x = rect.localScale.x / cSize.x;
            s2.y = rect.localScale.y / cSize.y;


            Vector2 p2;
            p2.x = rect.anchoredPosition.x / cSize.x;
            p2.y = rect.anchoredPosition.y / cSize.y;



            //rect.sizeDelta = s2 * canvasRect.sizeDelta;
            
            rect.localScale = s2 * canvasRect.sizeDelta;
            rect.anchoredPosition = p2 * canvasRect.sizeDelta;

            cSize = canvasRect.sizeDelta;
        }
    }
}
