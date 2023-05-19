using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




namespace CA
{
    public class ImageAnimation : MonoBehaviour
    {

        RectTransform tr;


        public Image image;

        [System.Serializable]
        public class Package
        {
            //시작이랑 끝 지정
            public Vector2 StartPosition;
            public Vector2 EndPosition;

            public bool isLoop = true;
            public float time = 1f;
        }



        public List<Package> list = new List<Package>();

        public void Awake()
        {
            tr = this.gameObject.GetComponent<RectTransform>();
            image = this.gameObject.GetComponent<Image>();
        }

        public void start(int index)
        {
            StartCoroutine(anim(list[index]));
        }


        public void end()
        {
            //무조건  EndPosition으로 집합
            StopAllCoroutines();
        }

        IEnumerator anim(Package pack)
        {
            tr.anchoredPosition = pack.StartPosition;

            Vector2 dist = pack.EndPosition - pack.StartPosition;
            dist = dist / 50.0f;

            for (int i = 0; i < 50; i++)
            {
                tr.anchoredPosition += dist;
                yield return new WaitForSeconds(0.01f);
            }
            tr.anchoredPosition = pack.EndPosition;




        }




    }

}
