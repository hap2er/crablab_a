using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CA
{

    public class PausePopup : Popup
    {

        public GameObject soundOnButton;
        public GameObject soundOffButton;

        public override void OnEnable()
        {
            if (SoundManager.Instance.isSound)
            {
                soundOffButton.SetActive(true);
                soundOnButton.SetActive(false);
            }
            else
            {
                soundOffButton.SetActive(false);
                soundOnButton.SetActive(true);
            }
               



            base.OnEnable();
        }

        public void homeButton()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ConstantData.SCENE_TITLE);
        }
        public void soundOn()
        {
            SoundManager.Instance.onEffect();
            soundOffButton.SetActive(true);
            soundOnButton.SetActive(false);
        }
        public void soundOff()
        {            
            SoundManager.Instance.offEffect();
            soundOffButton.SetActive(false);
            soundOnButton.SetActive(true);
        }
    }
}

