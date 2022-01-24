using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CA
{

    public class Consumer : MonoBehaviour
    {

        RectTransform tr;
        public ImageAnimation.Package pack;


        public void Start()
        {
            tr = this.gameObject.GetComponent<RectTransform>();
            StartCoroutine(anim());
        }

   

        IEnumerator anim()
        {

            while (true)
            {
                tr.anchoredPosition = pack.StartPosition;

                Vector2 dist = pack.EndPosition - pack.StartPosition;
                dist = dist / 100.0f;

                for (int i = 0; i < 100; i++)
                {
                    tr.anchoredPosition += dist;
                    yield return new WaitForSeconds(0.1f);
                }
                tr.anchoredPosition = pack.EndPosition;

            }
        }
    }

}
