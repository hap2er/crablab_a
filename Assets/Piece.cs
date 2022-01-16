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
            Piece01,
            Piece02,
            Piece03,
            Piece04,
            Piece05,
            Piece06,
            Piece07,
            Piece08,
            None

        }

        private Image _image;
        public RectTransform tr;

        [SerializeField]
        public int width;

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


        private void onPiece()
        {
            _image.enabled = true;
        }

        private void offPiece()
        {
            _image.enabled = false;
        }


        Coroutine coroutine = null;
        IEnumerator AnimationStart(bool isPush=true)
        {
           

            if(isPush)
            {
                _image.enabled = true;
            }
            else
            {
                _image.enabled = false;
            }
            yield return new WaitForSeconds(0.2f);
        }


        public Type type = Type.None;


        private void Awake()
        {
            _image = this.GetComponentInChildren<Image>();
            tr = this.GetComponent<RectTransform>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}


