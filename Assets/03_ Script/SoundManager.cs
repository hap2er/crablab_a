using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace CA
{

    [System.Serializable]
    public enum Sound
    {
        Button,
        Water,
        Clear01,
        Clear02,
        Trash,
        Bgm,
        GameOver,


        Burger01=11,
        Burger02,
        Burger03,
        Burger04,
        Burger05,
        Burger06,
        Burger07,
        Burger08


    }

    public class SoundManager : MonoBehaviour
    {
        #region Instance

        private static SoundManager _instance = null;

        public static SoundManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<SoundManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("SoundManager");
                    _instance = obj.AddComponent<SoundManager>();

                }
                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        #endregion


        [System.Serializable]
        public class Package
        {
            public Sound sound;
            public AudioSource source;

        }


        [SerializeField]
        public bool isSound = true;

        [SerializeField]
        List<Package> soundList = new List<Package>();


        // Start is called before the first frame update
        void Start()
        {
            if (isSound == false)
                return;

            foreach (Package pack in soundList)
            {
                if (pack.sound == Sound.Bgm)
                    pack.source.Play();
            }
        }

        public void StartEffect(Sound _sound)
        {
            if (isSound == false)
                return;


            foreach (Package pack in soundList)
            {
                if (pack.sound == _sound)
                {
                    pack.source.Play();
                }
                   
            }


        }



        public void StartBurger(Piece.Type _type)
        {

            Sound sound=(Sound)((int)_type+10);

            

            foreach (Package pack in soundList)
            {
                if (pack.sound == sound)
                    pack.source.Play();
            }
        }

        public void onEffect()
        {
            isSound = true;
            foreach (Package pack in soundList)
            {
                if (pack.sound == Sound.Bgm)
                    pack.source.Play();
            }
        }


        public void offEffect()
        {
            isSound = false;
            foreach (Package pack in soundList)
            {
                if (pack.sound == Sound.Bgm)
                    pack.source.Stop();
            }
        }

    }

}

