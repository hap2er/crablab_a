using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CA
{
    public class Button : MonoBehaviour
    {

        private RectTransform tr;
        public Rect rect
        {
            get
            {
                Rect _rect = tr.rect;

                _rect.xMin = tr.anchoredPosition.x;
                _rect.yMin = tr.anchoredPosition.y;

                _rect.xMax = tr.anchoredPosition.x + tr.sizeDelta.x;
                _rect.yMax = tr.anchoredPosition.y + tr.sizeDelta.y;

                return _rect;
            }
        }



        private void Awake()
        {

            tr = GetComponent<RectTransform>();
        }

        public void OnTouchBegan()
        {
            Debug.Log(this.gameObject + " 땃쥐.");
        }

    }
}


