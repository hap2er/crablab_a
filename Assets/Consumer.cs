using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CA
{
    /// <summary>
    /// 전광판
    /// </summary>
    public class Consumer : MonoBehaviour
    {
        
        private RectTransform tr;

        public void Start()
        {
            tr = this.gameObject.GetComponent<RectTransform>();
            HV.MotionHelper.DoMove(transform,new Vector2(300,0),new Vector2(-1680,0), 10, true, false,true);
        }
    }

}
