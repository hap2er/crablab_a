using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HV
{
    public class MotionHelper : MonoBehaviour
    {
        public static To DoMove(Transform tr, Vector3 to, float time,bool loop,bool restart, bool unscaleTime = false)
        {
            DoMove doMove = tr.gameObject.GetComponent<DoMove>();
            if (doMove == null)
            {
                doMove = tr.gameObject.AddComponent<DoMove>();
            }

            doMove.DoIt(tr.position, to, time, loop, restart, unscaleTime);

            return doMove;
        }

        public static To DoMove(Transform tr, Vector3 from,Vector3 to, float time, bool loop, bool restart, bool unscaleTime = false)
        {
            DoMove doMove = tr.gameObject.GetComponent<DoMove>();
            if (doMove == null)
            {
                Debug.Log("...?");

                doMove = tr.gameObject.AddComponent<DoMove>();
            }

            doMove.DoIt(from, to, time, loop, restart,unscaleTime);

            return doMove;
        }

    }

}

