using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace HV
{
    [System.Serializable]
    public class RatioEvent : UnityEvent<Vector2>
    {
    }


    /// <summary>
    /// 레터박스가 필요한 씬마다 각각 배치 필요
    /// </summary>
    public class LetterBox : MonoBehaviour
    {
        /// <summary>
        /// 기본 해상도 비율
        /// </summary>
        static public Vector2 baseRatio = new Vector2(9, 16);

        /// <summary>
        /// 최소 해상도 비율
        /// </summary>
        static public Vector2 minRatio = new Vector2(9, 16);

        float f_minRatio;

        /// <summary>
        /// 최대 해상도 비율
        /// </summary>
        static public Vector2 maxRatio = new Vector2(9, 16);

        float f_maxRatio;

        private Vector2 screenRatio;

        /// <summary>
        /// 카메라 배열
        /// </summary>
        private Camera[] cameraArray = null;

        private void Awake()
        {
            f_minRatio = minRatio.x / minRatio.y;
            f_maxRatio = maxRatio.x / maxRatio.y;
        }


        private void Update()
        {
            if (screenRatio == new Vector2(Screen.width, Screen.height))
                return;

            float d_value = screenRatio.x / screenRatio.y;
            float n_value = Screen.width / Screen.height;

            if (d_value != n_value)
                setupLetterBox();

        }    
        private void setupLetterBox()
        {

            Vector2 _screen = new Vector2(Screen.width, Screen.height);
            //해상도 비율
            float value = _screen.x / _screen.y;
            if (cameraArray == null)
                cameraArray = FindObjectsOfType<Camera>();

            Rect rect = new Rect();
            //비율 확인
            if (value == Mathf.Clamp(value, f_minRatio, f_maxRatio))    //박스 OFF
            {
                rect.width = 1.0f;
                rect.height = 1.0f;
                rect.x = 0;
                rect.y = 0;
            }
            else                                                        //박스 ON
            {
                if (value > f_maxRatio) //가로 > 세로
                {
                    rect.width = f_maxRatio / value;
                    rect.height = 1.0f;
                    rect.x = (1 - rect.width) * 0.5f;
                    rect.y = 0;

                    value = f_maxRatio;

                    _screen.x = _screen.y * f_maxRatio;

                }
                else                   //세로 > 가로
                {

                    rect.width = 1.0f;
                    rect.height = value / f_minRatio;
                    rect.x = 0;
                    rect.y = (1 - rect.height) * 0.5f;

                    value = f_minRatio;

                    _screen.y = _screen.x / f_minRatio;
                }

            }

            // 뒷 배경 담당 카메라 제외
            for (int i = 0; i < cameraArray.Length; i++)
            {
                if (cameraArray[i].name != "ExceptionCamera")
                {
                    cameraArray[i].rect = rect;
                }

            }


            screenRatio = new Vector2(Screen.width, Screen.height);

            //연결된 이벤트 호출
            _event.Invoke(_screen);
        }

        //해상도 비율 변경 Event
        public RatioEvent _event;

    }
}

