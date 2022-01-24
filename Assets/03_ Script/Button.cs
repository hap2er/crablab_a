using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CA
{
    public class Button : MonoBehaviour
    {
        [SerializeField]
        private Piece.Type type;




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
            //if (SoundManager.Instance != null)
            //    SoundManager.Instance.StartBurger(type);

            if (GameSceneManager.Instance != null)
                GameSceneManager.Instance.addPiece(type);
        }

    }
}


