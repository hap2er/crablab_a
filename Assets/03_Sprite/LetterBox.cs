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
    /// ���͹ڽ��� �ʿ��� ������ ���� ��ġ �ʿ�
    /// </summary>
    public class LetterBox : MonoBehaviour
    {
        /// <summary>
        /// �⺻ �ػ� ����
        /// </summary>
        static public Vector2 baseRatio = new Vector2(9, 16);

        /// <summary>
        /// �ּ� �ػ� ����
        /// </summary>
        static public Vector2 minRatio = new Vector2(9, 16);

        float f_minRatio;

        /// <summary>
        /// �ִ� �ػ� ����
        /// </summary>
        static public Vector2 maxRatio = new Vector2(9, 16);

        float f_maxRatio;

        private Vector2 screenRatio;

        /// <summary>
        /// ī�޶� �迭
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
            //�ػ� ����
            float value = _screen.x / _screen.y;
            if (cameraArray == null)
                cameraArray = FindObjectsOfType<Camera>();

            Rect rect = new Rect();
            //���� Ȯ��
            if (value == Mathf.Clamp(value, f_minRatio, f_maxRatio))    //�ڽ� OFF
            {
                rect.width = 1.0f;
                rect.height = 1.0f;
                rect.x = 0;
                rect.y = 0;
            }
            else                                                        //�ڽ� ON
            {
                if (value > f_maxRatio) //���� > ����
                {
                    rect.width = f_maxRatio / value;
                    rect.height = 1.0f;
                    rect.x = (1 - rect.width) * 0.5f;
                    rect.y = 0;

                    value = f_maxRatio;

                    _screen.x = _screen.y * f_maxRatio;

                }
                else                   //���� > ����
                {

                    rect.width = 1.0f;
                    rect.height = value / f_minRatio;
                    rect.x = 0;
                    rect.y = (1 - rect.height) * 0.5f;

                    value = f_minRatio;

                    _screen.y = _screen.x / f_minRatio;
                }

            }

            // �� ��� ��� ī�޶� ����
            for (int i = 0; i < cameraArray.Length; i++)
            {
                if (cameraArray[i].name != "ExceptionCamera")
                {
                    cameraArray[i].rect = rect;
                }

            }


            screenRatio = new Vector2(Screen.width, Screen.height);

            //����� �̺�Ʈ ȣ��
            _event.Invoke(_screen);
        }

        //�ػ� ���� ���� Event
        public RatioEvent _event;

    }
}

