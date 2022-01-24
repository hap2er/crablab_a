using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CA
{

    public class GameSceneManager : MonoBehaviour
    {
        #region Instance

        private static GameSceneManager _instance = null;

        public static GameSceneManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<GameSceneManager>();
                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// 상태
        /// </summary>
        public enum State
        {
            Start,
            //시작 전 주문
            Idle,
            //플레이
            Play,
            //제출
            Wait,
            //결과
            Result
        }

        /// <summary>
        /// 진행 상태
        /// </summary>
        private State _state = State.Start;

        /// <summary>
        /// 진행 상태 (Getter/Setter)
        /// </summary>
        public State state
        { 
        get
            {
                return _state;
            }
            set
            {
                if (value == _state)
                    return;

                _state = value;

                
                changeState();
            }
        
        }





        /// <summary>
        /// Game 햄바가 프리팹
        /// </summary>
        [Header("- Prefab")]
        public List<GameObject> piecePrefab = new List<GameObject>();

        [Header("- Npc")]
        /// <summary>
        /// NPC
        /// </summary>
        [SerializeField]
        public Npc npc;


        #region UI
        [Header("- Parents")]
        /// <summary>
        /// Game의 햄버거 부모 Transform
        /// </summary>
        [SerializeField]
        private Transform burgerDishes;

        [Header("- Timer")]
        public Image timerImage = null;

        [Header("- Water")]
        public Image waterImage = null;
        public UnityEngine.UI.Button waterButton;

        [Header("- MaxScore")]
        public int maxScore = 0;
        public Image newRecordImage;
        /// <summary>
        /// 메인 캔버스의 RectTransform
        /// </summary>
        private RectTransform canvasRect;


        /// <summary>
        /// 재료 버튼
        /// </summary>
        private CA.Button[] buttons = null;

        /// <summary>
        /// 점수 텍스트
        /// </summary>
        public Text scoreText;

        #endregion

        [Header("- Popup")]
        public GameObject resultPopup;

        /// <summary>
        /// 쓰고 남은 햄-바가 재료 
        /// </summary>
        private List<Piece> pool = new List<Piece>();

        /// <summary>
        /// 햄바-가 쌓은 부분
        /// </summary>
        private Stack<Piece> burgerStack = new Stack<Piece>();


        /// <summary>
        /// 남은 시간
        /// </summary>
        private float remainingTime = 0;

        /// <summary>
        /// 누적 점수
        /// </summary>
        public int score = 0;

        /// <summary>
        /// 라운드 수
        /// </summary>
        private int round = 0;


        public Lv level;

        private void Start()
        {
            //배너 광고
            AdmobManager.Instance.bannerOnlyView(true);

            //캔버스 찾기
            if (canvasRect == null)
            {
                canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
            }

            state = State.Idle;

            maxScore = PlayerPrefs.GetInt("MaxScore",0);
        }

        /// <summary>
        /// 상태 변경시 호출
        /// </summary>
        private void changeState()
        {
            switch (state)
            {
                case State.Idle:
                    StartCoroutine(UpdateIdle());
                    break;
                case State.Play:
                    StartCoroutine(UpdatePlay(remainingTime));
                    break;
                case State.Wait:
                    //타이머 종료
                    StopAllCoroutines();
                    StartCoroutine(UpdateWait());
                    break;
                case State.Result:
                    StartCoroutine(UpdateResult());
                    break;

            }
        }

        /// <summary>
        /// IDLE 상태 코루틴
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateIdle()
        {
            //초기화
            timerImage.fillAmount = 1f;

            //대기
            yield return new WaitForSeconds(0.5f);

            //npc 켜기 (물병이 필요하면 이 부분에서 다시 ons)
            npc.gameObject.SetActive(true);

            //대기
            yield return new WaitForSeconds(1.0f);



            //if(level!=Lv.hard)
            //    DataManager.Instance.makeMap();
            //level = Lv.hard;
            if (round >= 20)
            {
                if (level == Lv.normal)
                {
                    DataManager.Instance.makeMap();
                }
                level = Lv.hard;
                npc.text.color = Color.red;
            }
            else if (round >= 10)
            {
                level = Lv.normal;
                npc.text.color = Color.green;
            }
            else
            {
                level = Lv.easy;
                npc.text.color = Color.white;
            }


            //주문 
            Map map = DataManager.Instance.GetMap(level);
            npc.setup(map);

            //시간 정의
            remainingTime = map.time;
           
            //대기
            //yield return new WaitForSeconds(1.0f);

            //주문 시작
            state = State.Play;            
        }

        /// <summary>
        /// PALY 상태 코루틴
        /// </summary>
        /// <param name="_time"></param>
        /// <returns></returns>
        private IEnumerator UpdatePlay(float _time)
        {
            while (remainingTime > 0)
            {
                //타이머 
                if (state == State.Play)
                {
                    remainingTime -= 0.1f;
                    timerImage.fillAmount = remainingTime / _time;
                }

                yield return new WaitForSeconds(0.1f);
            }

            //타임 오버         
            state = State.Result;
        }
        private IEnumerator UpdateWait()
        {

            if (SoundManager.Instance != null && npc.isTake)
            {
                SoundManager.Instance.StartEffect(Sound.Clear01);

            }

            //대기 
            yield return new WaitForSeconds(0.5f);
            
            //햄ㅂ-ㅏ가 치우기
            removePiece();


            if (SoundManager.Instance != null && npc.isTake)
            {
                SoundManager.Instance.StartEffect(Sound.Clear02);

            }

            //대기
            yield return new WaitForSeconds(1f);

            //Npc퇴장
            npc.gameObject.SetActive(false);

            //라운드 추가
            round++;

            //상태 변경
            state = State.Idle;
           
        }
        private IEnumerator UpdateResult()
        {
            timerImage.fillAmount = 0f;

            //대기 
            yield return new WaitForSeconds(0.5f);

            //햄ㅂ-ㅏ가 치우기
            removePiece();

            //Npc퇴장
            npc.gameObject.SetActive(false);

            openPopup("ResultPopup");
            
        }
        public void addScore(int _score)
        {
            //점수 증가
            score += _score;

            //텍스트 변경
            scoreText.text = score.ToString();

            if (maxScore < score)
            {
                maxScore = score;
                PlayerPrefs.SetInt("MaxScore", score);
                newRecordImage.gameObject.SetActive(true);
            }
        }
        private void OnEnable()
        {
            TouchManager.Instance.touchEvent += touchEvent;

        }
        private void OnDisable()
        {
            TouchManager.Instance.touchEvent -= touchEvent;

        }

        #region MakeBurger

        /// <summary>
        /// 재료 추가
        /// </summary>
        /// <param name="type"></param>
        public void addPiece(Piece.Type type)
        {
            if (state != State.Play)
                return;

            Piece stuff =null;
            foreach (Piece piece in pool)
            {
                if (piece.isActive == true)
                    continue;

                if (piece.type == type)
                {
                    stuff = piece;
                    //stuff.isActive = true;
                    break;
                }
            }

            if(stuff == null)
            {
                GameObject obj = Instantiate(piecePrefab[(int)type], burgerDishes);
                stuff = obj.GetComponent<Piece>();
                pool.Add(stuff);
            }

            int y = 0;
            foreach (Piece piece in burgerStack)
            {
                y+=piece.width;
            }

            stuff.tr.anchoredPosition = new Vector3(0, y, 0);

            //버거 쌓은 두께와 NPC정답의 두께가 같고
            if(burgerStack.Count == npc.answer.Count-1)
            {
                //NPC가 요구하는 버거의 상단이 번일 경우
                if (npc.answer[npc.answer.Count - 1] == (int)Piece.Type.Bun)
                {
                    //내가 선택한 조각이 번일 경우
                    if ((int)Piece.Type.Bun == (int)stuff.type)
                    {

                        bool isChange = true;
                        List<int> checkList = new List<int>();

                        //여기서 정답 체크 한번 하고가기
                        foreach (Piece piece in burgerStack)
                        {
                            checkList.Add((int)piece.type);
                        }
                        checkList.Reverse();
                        for (int i = 0; i < checkList.Count; i++)
                        {
                            if (checkList[i] != npc.answer[i])
                            {
                                isChange = false;
                                break;
                            }

                        }

                        if(isChange)
                            stuff.changeSprite(1);
                    }
                }
            }
           

            stuff.isActive = true;
            stuff.tr.SetSiblingIndex(burgerStack.Count);


           
            burgerStack.Push(stuff);

            Sound _sound = (Sound)(Mathf.Min(burgerStack.Count, (int)Piece.Type.Mushroom) + 10);
            SoundManager.Instance.StartEffect(_sound);

            List<int> answer = new List<int>();
            int count = 0;
            foreach (Piece piece in burgerStack)
            {
                answer.Add((int)piece.type);
                count++;
            }
            if (npc.CheckAnswer(answer))
                state = State.Wait;


        }
        /// <summary>
        /// 재료 뺴기
        /// </summary>
        public void popPiece()
        {
            if (burgerStack.Count <= 0)
                return;
            Piece stuff = burgerStack.Pop();
            stuff.isActive = false;


            List<int> answer = new List<int>();
            foreach (Piece piece in burgerStack)
            {
                answer.Add((int)piece.type);
            }
            if (npc.CheckAnswer(answer))
                state = State.Wait;

        }


        #endregion


        #region Button

        /// <summary>
        /// 쓰레기통 버튼
        /// </summary>
        public void removePiece()
        {
            foreach (Piece stuff in burgerStack)
            {
                stuff.submission();
            }

            npc.angryEffect.SetActive(false);
            burgerStack.Clear();
        }
        /// <summary>
        /// 버튼 연결 함수
        /// </summary>
        public void washNpc()
        {
            StartCoroutine(boomWater());
        }

        #endregion


        /// <summary>
        /// Touch 관련 작업
        /// </summary>
        /// <param name="phase"></param>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        private void touchEvent(TouchPhase phase, int id, float x, float y, float dx, float dy)
        {

        }


        /// <summary>
        /// 네네... 물벼락 드세요... 
        /// </summary>
        /// <returns></returns>
        IEnumerator boomWater()
        {
            waterImage.gameObject.SetActive(true);
            
            yield return new WaitForSeconds(0.25f);

            waterImage.gameObject.SetActive(false);
        }


        /// <summary>
        /// 진상에게는 물이 약이다.
        /// </summary>
        public void givemeWater()
        {
            waterButton.gameObject.SetActive(true);
        }

        ///// <summary>
        ///// 고객의 주문에는 물이 필요 없어요.
        ///// </summary>
        //public void getoutWater()
        //{
        //    waterImage.gameObject.SetActive(false);
        //    waterButton.gameObject.SetActive(false);
        //}




        public void ShowRewardAD()
        {
            AdmobManager.Instance.showRewardAd();
        }


        public void goon()
        {
            state = State.Idle;
        }
        public void restart()
        {
            if (maxScore< score)
            {
                maxScore = score;
                PlayerPrefs.SetInt("MaxScore", score);
            }

            //StopAllCoroutines();

            //state = State.Idle;
            //round = 0;
            //score = 0;
            //scoreText.text = score.ToString();

            //newRecordImage.gameObject.SetActive(false);

            UnityEngine.SceneManagement.SceneManager.LoadScene(ConstantData.SCENE_TITLE);
        }




        private void Update()
        {
            processKey();
        }


        Stack<Popup> popupStack = new Stack<Popup>();
        Dictionary<string, GameObject> popupList = new Dictionary<string, GameObject>();

        public void openPopup(string name)
        {
            GameObject pop;
            if (!popupList.ContainsKey(name))
            {
               

                Transform pa = GameObject.Find("Popup").transform;

                pop = Instantiate(Resources.Load<GameObject>(string.Format("Popup/{0}", name)), pa);
                popupList.Add(name, pop);
            }
            else
            {
                pop = popupList[name];
            }

            pop.gameObject.SetActive(true);
            popupStack.Push(pop.GetComponent<Popup>());

        }
        public void processKey()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(popupStack.Count>0)
                {
                    Popup pop = popupStack.Pop();
                    pop.backkey();

                    if(popupStack.Count==0)
                        Time.timeScale = 1.0f;
                }
                else
                {
                    openPopup("PausePopup");
                }
                    
            }
        }


        public void backKey()
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.StartEffect(Sound.Button);
            openPopup("PausePopup");
        }

        public void TrashSound()
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.StartEffect(Sound.Trash);
        }

        public void ButtonSound()
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.StartEffect(Sound.Button);
        }

    }

}
