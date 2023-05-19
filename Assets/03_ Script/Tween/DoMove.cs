using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HV
{
    public class DoMove : To
    {
        
        private Vector3 _from;
        private Vector3 _to;



        public override To DoIt(Vector3 from, Vector3 to, float time, bool loop, bool restart, bool unscaleTime = false)
        {
            if (restart.Equals(true))
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
            }
            else
            {
                if (_coroutine != null)
                    return this;
            }

            _from = from;
            _to = to;
            _time = time;
            _loop = loop;
            _restart = restart;
            _unscaleTime = unscaleTime;

            _coroutine = StartCoroutine(mainCoroutine());

            return this;
        }

        protected override IEnumerator mainCoroutine()
        {
            RectTransform uiTransform = this.GetComponent<RectTransform>();
            do
            {
                if (uiTransform is null)
                    transform.position = _from;
                else
                    uiTransform.anchoredPosition = _from;

                ///총 이동 거리
                Vector3 distance = _to - _from;
                // 초를 프레임으로 변환
                int frame = (int)(Frame * _time);

                //프레임당 이동 거리
                Vector3 value = distance / frame;

                for (int i = 0; i < frame; ++i)
                {
                    if (uiTransform is null)
                        transform.position = _from + (value * i);
                    else
                        uiTransform.anchoredPosition = _from + (value * i);

                    if (_unscaleTime)
                        yield return UnscaleFrame;
                    else
                        yield return ScaleFrame;
                }
                if (uiTransform is null)
                    transform.position = _to;
                else
                    uiTransform.anchoredPosition = _to;


            } while (_loop);
           
            Destroy(this);
        }
    }
}