using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CA
{
    public class Popup : MonoBehaviour
    {
        public void Update()
        {        
        }

        public virtual void OnEnable()
        {
            Time.timeScale = 0.0f;
        }

        public virtual void OnDisable()
        {
            Time.timeScale = 1.0f;
        }

        public virtual void backkey()
        {
            this.gameObject.SetActive(false);


        }


      

        public void ButtonSound()
        {
            SoundManager.Instance.StartEffect(Sound.Button);
        }
    }

}

