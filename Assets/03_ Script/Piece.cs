using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CA
{


    public class Piece : MonoBehaviour
    {

        [System.Serializable]
        public enum Type
        {
            None,
            Bun,
            Patty,
            Lettuce,
            Tomato,
            Cheese,
            RedOnion,
            Bacon,
            Mushroom,
            BadOrder


        }

        private Image _image;
        public RectTransform tr;

        [SerializeField]
        public int width;

        public Sprite[] sprites = null;
        public Vector2[] sizes = null;


        private bool _isActive=false;
        public bool isActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive == value)
                    return;

                _isActive = value;

                //On
                if(value)
                {
                    coroutine = StartCoroutine(AnimationStart(true));
                }
                //Off
                else
                {
                    StopCoroutine(coroutine);
                    coroutine = StartCoroutine(AnimationStart(false));
                }
            }
        }



        //한번에 제출
        public void submission()
        {
            _isActive = false;
            offPiece();
        }


        private void onPiece(int type = 0)
        {
            _image.enabled = true;
        }

        public void changeSprite(int type = 0)
        {
            if (sprites == null || sizes == null)
                return;
            if (sprites.Length == 0 || sizes.Length == 0)
                return;

            _image.sprite = sprites[type];
            tr.sizeDelta = sizes[type];
        }


        private void offPiece()
        {
            _image.enabled = false;

            if (sprites == null || sizes == null)
                return;

            if (sprites.Length == 0 || sizes.Length == 0)
                return;

            _image.sprite = sprites[0];
            tr.sizeDelta = sizes[0];

        }


        Coroutine coroutine = null;
        IEnumerator AnimationStart(bool isPush=true)
        {
           

            if(isPush)
            {
                onPiece();
            }
            else
            {
                offPiece();
            }
            yield return new WaitForSeconds(0.2f);
        }


        public Type type = Type.None;


        private void Awake()
        {
            _image = this.GetComponentInChildren<Image>();
            tr = this.GetComponent<RectTransform>();
        }


        private void OnDisable()
        {

            if (sprites == null || sizes == null)
                return;

            if (sprites.Length == 0 || sizes.Length == 0)
                return;

            _image.sprite = sprites[0];
            tr.sizeDelta = sizes[0];
        }
    }

}


