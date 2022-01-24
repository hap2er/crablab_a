using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CA
{
    public class TitleScene : MonoBehaviour
    {
        public void Start()
        {
            AdmobManager.Instance.bannerOnlyView(false);
        }
        public void nextScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(ConstantData.SCENE_GAME);
            SoundManager.Instance.StartEffect(Sound.Button);
        }
    }

}

