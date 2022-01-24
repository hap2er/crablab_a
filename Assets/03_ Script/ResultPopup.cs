using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




namespace CA
{
    public class ResultPopup : Popup
    {

        public Text scoreText;
        public Text bestScoreText;
        public void ShowAdButton()
        {
            ButtonSound();
            GameSceneManager.Instance.ShowRewardAD();
        }

        public void RestartButton()
        {
            ButtonSound();
            backkey();           
        }



        public override void OnEnable()
        {
            Adapter.Instance.changeRewarded += rewarded;
            if (SoundManager.Instance != null)
                SoundManager.Instance.StartEffect(Sound.GameOver);

            scoreText.text = GameSceneManager.Instance.score.ToString();
            bestScoreText.text = GameSceneManager.Instance.maxScore.ToString();

            base.OnEnable();
        }

        public override void OnDisable()
        {
            Adapter.Instance.changeRewarded -= rewarded;
            base.OnDisable();
        }


        public void rewarded(Adapter.RewardedState state)
        {
            if (state == Adapter.RewardedState.Success)
            {
                GameSceneManager.Instance.goon();
                this.gameObject.SetActive(false);
            }
              
        }


        public override void backkey()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ConstantData.SCENE_TITLE);
        }


    }

}
