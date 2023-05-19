using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace HV
{
    public class To : MonoBehaviour
    {
        protected const int Frame =60;
        protected WaitForSeconds ScaleFrame = new WaitForSeconds(1f / Frame);
        protected WaitForSecondsRealtime UnscaleFrame = new WaitForSecondsRealtime(1f / Frame);


        //<-------- 공용 사용 --------->//
        protected Coroutine _coroutine;
        protected float _time;
        protected bool _loop;
        protected bool _restart;
        protected bool _unscaleTime;

        public virtual To DoIt(Vector3 from, Vector3 to, float time, bool loop = false, bool restart = false ,bool unscaleTime = false)
        {
            return this;
        }

        public virtual To SetLoops(int count = -1)
        {

            return this;
        }
        public virtual To OnComplete(System.Action action)
        {
            return this;
        }

        

        protected virtual IEnumerator mainCoroutine()
        {
            yield return null;
        }

    }

}

