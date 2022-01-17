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

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        //상태 관련 추가
        public enum State
        {
            //시작 전 주문
            Idle,
            //플레이
            Play,
            //제출
            Wait,
            //졋음
            Result
        }



        /// <summary>
        /// 현재 씬의 진행 상태
        /// </summary>
        private State state = State.Idle;


        /// <summary>
        /// 메인 캔버스의 RectTransform
        /// </summary>
        private RectTransform canvasRect;


        /// <summary>
        /// 재료 버튼
        /// </summary>
        private CA.Button[] buttons = null;

        /// <summary>
        /// Game 햄바가 프리팹
        /// </summary>
        [Header("- Prefab")]
        public List<GameObject> piecePrefab = new List<GameObject>();

        [Header("- Parents")]
        /// <summary>
        /// Game의 햄버거 부모 Transform
        /// </summary>
        public Transform burgerDishes;

        /// <summary>
        /// 쓰고 남은 햄-바가 재료 
        /// </summary>
        private List<Piece> pool = new List<Piece>();

        /// <summary>
        /// 햄바-가 쌓은 부분
        /// </summary>
        private Stack<Piece> pieceStack = new Stack<Piece>();

        public Npc npc;

        float time;

        private void Start()
        {
            //배너 광고
            AdmobManager.Instance.bannerOnlyView(true);


            if (canvasRect == null)
            {
                canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
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
            if (state != State.Play)
                return;

            //버튼이 없을 경우 현재 씬에서 찾아 넣어준다.
            if (buttons == null)
            {
                buttons = GameObject.FindObjectsOfType<CA.Button>();

            }

            if (phase != TouchPhase.Began)
                return;


            Vector2 v_v2 = new Vector2(x, y);
            Vector2 s_v2 = v_v2 *= canvasRect.sizeDelta;

            //터치 이벤트 호출
            foreach (CA.Button btn in buttons)
            {
                if (btn.rect.Contains(s_v2))
                {
                    btn.OnTouchBegan();
                    break;
                }
            }
        }





       
        public void addPiece(Piece.Type type)
        {
            Piece stuff =null;
            foreach (Piece piece in pool)
            {
                if (piece.isActive == true)
                    continue;

                if (piece.type == type)
                {
                    stuff = piece;
                    stuff.isActive = true;
                    break;
                }
            }

            if(stuff == null)
            {
                GameObject obj = Instantiate(piecePrefab[(int)type], burgerDishes);
                stuff = obj.GetComponent<Piece>();
                stuff.isActive = true;
                pool.Add(stuff);
            }

            int y = 0;
            foreach (Piece piece in pieceStack)
            {
                y+=piece.width;
            }

            stuff.tr.anchoredPosition = new Vector3(0, y, 0);

            pieceStack.Push(stuff);



            List<int> answer = new List<int>();
            int count = 0;
            foreach (Piece piece in pieceStack)
            {
                answer.Add((int)piece.type);
            }
            if (npc.CheckAnswer(answer))
                state = State.Wait;


        }



        public void popPiece()
        {
            if (pieceStack.Count <= 0)
                return;
            Piece stuff = pieceStack.Pop();
            stuff.isActive= false;


            List<int> answer = new List<int>();
            foreach (Piece piece in pieceStack)
            {
                answer.Add((int)piece.type);
            }
            if (npc.CheckAnswer(answer))
                state = State.Wait;

        }

        public void removePiece()
        {

            state = State.Wait;


            foreach (Piece stuff in pieceStack)
            {
                stuff.submission();
            }


            pieceStack.Clear();
        }







        public void setTime(float t)
        {
            time = t;
            StartCoroutine(checkTime(t));
        }

        public Image timeImage = null;

        IEnumerator checkTime(float _time)
        {
            timeImage.fillAmount = 1f;
            state = State.Idle;
            yield return new WaitForSeconds(1f);
            state = State.Play;
            while (time>0)
            {
                if(state == State.Play)
                {
                    time -= 0.1f;
                    timeImage.fillAmount = time / _time;
                }

                yield return new WaitForSeconds(0.1f);
            }

            timeImage.fillAmount = 0f;
            state = State.Result;
            Debug.Log("타임오버");
        }






    }

}
