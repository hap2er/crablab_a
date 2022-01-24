using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CA
{
    public class Npc : MonoBehaviour
    {
        

        [SerializeField]
        public List<int> answer = new List<int>();


        private List<int> burger = new List<int>();

        private int except = (int)Piece.Type.None;


        [SerializeField]
        private int maxTime = 30;

        public  Transform burgerTr;
        public Transform exceptTr;
        Stack<Piece> piece = new Stack<Piece>();

        public Text text;

        int count = 0;

        bool isStay = true;


        public ImageAnimation anim;


        public GameObject angryEffect;

        public void OnEnable()
        {
            //npc켜기
            isStay = true;
            anim.start(0);

        }





        public bool isTake = false;

        public void setup(Map map)
        {
            burger.Clear();
            answer.Clear();
            isTake = false;

            //burger = map.burger;
            except = map.except;

            for(int i=0;i< map.burger.Count;i++)
            {
                burger.Add(map.burger[i]);
                if (map.burger[i] != except)
                {
                    answer.Add(map.burger[i]);
                }
                   
            }

            text.text = map.name;

            showOrder();

        }


        List<Piece> pieceList = new List<Piece>();

        public bool CheckAnswer(List<int> list)
        {
            bool val = true;

            //아무것도 없으면 그냥 꺼져라
            if (list.Count == 0)
                return false;

            //스택에서 뽑아왔기때문에 역순으로 재정렬 필요
            list.Reverse();
            for (int i=0;i< list.Count;i++)
            {
                if (answer[i] != list[i])
                    val= false;

            }

      
            //요소가 다름
            if(val==false)
            {
                angryEffect.SetActive(true);
                return false;
            }           
            else
            {
                angryEffect.SetActive(false);
                //같긴한데 리스트가 부족함
                if (list.Count != answer.Count)
                    return false;
                else
                {
                    //네넹 잘 받앗숩니다.
                    isTake = true;
                    return true;
                }
                    
            }
        }


        public void showOrder()
        {
            burgerTr.parent.gameObject.SetActive(true);
            


            int y = 0;


            int index = 0;

            foreach (int value in burger)
            {
                Piece stuff = null;
                foreach (Piece piece in pieceList)
                {
                    if(piece.gameObject.activeSelf==false && piece.type == (Piece.Type)value)
                    {
                        stuff = piece;
                        stuff.gameObject.SetActive(true);
                        stuff.transform.parent = burgerTr;
                        break;
                    }

                }
                if(stuff==null)
                {
                    GameObject obj = Instantiate(GameSceneManager.Instance.piecePrefab[value], burgerTr);
                    stuff = obj.GetComponent<Piece>();
                    pieceList.Add(stuff);
                }

                if (value > 8)
                    GameSceneManager.Instance.givemeWater();
                if (index == burger.Count - 1 && value == (int)Piece.Type.Bun)
                {
                    stuff.changeSprite(1);
                }



                stuff.tr.SetSiblingIndex(index);
                stuff.tr.localScale = Vector2.one;
                stuff.tr.anchoredPosition = new Vector3(0, y, 0);
                y += stuff.width;
                index++;
            }
            
            if(except!=(int)Piece.Type.None)
            {
                exceptTr.parent.gameObject.SetActive(true);
                Piece ePiece = null;
                foreach (Piece piece in pieceList)
                {
                    if (piece.gameObject.activeSelf == false && piece.type == (Piece.Type)except)
                    {
                        ePiece = piece;
                        ePiece.gameObject.SetActive(true);
                        ePiece.transform.parent = exceptTr;
                        break;
                    }

                }
                if(ePiece==null)
                {
                    GameObject obj = Instantiate(GameSceneManager.Instance.piecePrefab[except], exceptTr);
                    ePiece = obj.GetComponent<Piece>();
                    pieceList.Add(ePiece);
                }
                ePiece.tr.localScale = Vector2.one;
                ePiece.tr.anchoredPosition = Vector2.zero;
               
               
            }


        }


        public void washNpc()
        {
            if (this.gameObject.activeSelf == false)
                return;


            if (SoundManager.Instance != null)
                SoundManager.Instance.StartEffect(Sound.Water);


            //진상이면 얌전히 퇴장
            if (burger.Count == 1 && burger[0] == (int)Piece.Type.BadOrder)
            {
                GameSceneManager.Instance.state = GameSceneManager.State.Wait;
            }
            //아니면 게임 오버
            else
            {
                GameSceneManager.Instance.state = GameSceneManager.State.Result;
            }


            
            GameSceneManager.Instance.washNpc();


        }


        private void OnDisable()
        {
            this.gameObject.SetActive(false);
            foreach (Piece piece in pieceList)
            {
                piece.gameObject.SetActive(false);

            }

            text.text = "";
            burgerTr.parent.gameObject.SetActive(false);
            exceptTr.parent.gameObject.SetActive(false);


            if (isTake)
                GameSceneManager.Instance.addScore(answer.Count * 10);
        }

    }

}

