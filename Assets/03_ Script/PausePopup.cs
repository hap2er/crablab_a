using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CA
{
    /// <summary>
    /// 일시정지
    /// </summary>
    public class PausePopup : Popup
    {

        public GameObject soundOnButton;
        public GameObject soundOffButton;

        public override void OnEnable()
        {
            if (SoundManager.Instance.isSound)
            {
                soundOffButton.SetActive(false);
                soundOnButton.SetActive(true);
            }
            else
            {

                soundOffButton.SetActive(true);
                soundOnButton.SetActive(false);
               
            }



            GameSceneManager.Instance.blindOrder(true);
            base.OnEnable();
        }

        public override void OnDisable()
        {
            GameSceneManager.Instance.blindOrder(false);

            base.OnDisable();
        }


        public void homeButton()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ConstantData.SCENE_TITLE);
        }
        public void soundOn()
        {
            SoundManager.Instance.onEffect();
            soundOffButton.SetActive(false);
            soundOnButton.SetActive(true);
        }
        public void soundOff()
        {            
            SoundManager.Instance.offEffect();
            soundOffButton.SetActive(true);
            soundOnButton.SetActive(false);
          
        }
    }
}

