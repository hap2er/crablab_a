using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CA
{

    public class GameSceneManager : MonoBehaviour
    {
        public RectTransform canvasRect;
        private void Start()
        {
            AdmobManager.Instance.bannerOnlyView(true);


            if (canvasRect == null)
            {
                canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
            }
        }


        private void OnEnable()
        {
            TouchManager.Instance.touchEvent += touchEvent;

        }
        private void OnDisable()
        {
            TouchManager.Instance.touchEvent -= touchEvent;

        }

        CA.Button[] buttons = null;

        private void touchEvent(TouchPhase phase, int id, float x, float y, float dx, float dy)
        {

            if (buttons == null)
            {
                buttons = GameObject.FindObjectsOfType<CA.Button>();

            }

            if (phase != TouchPhase.Began)
                return;


            Vector2 v_v2 = new Vector2(x, y);

            Vector2 s_v2 = v_v2 *= canvasRect.sizeDelta;


            foreach (CA.Button btn in buttons)
            {
                if (btn.rect.Contains(s_v2))
                {
                    btn.OnTouchBegan();
                    break;
                }
            }




        }
    }

}
