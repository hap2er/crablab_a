using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CA
{
    public class Npc : MonoBehaviour
    {
        

        [SerializeField]
        private List<int> answer = new List<int>();


        private List<int> burger = new List<int>();

        private int except = (int)Piece.Type.None;


        [SerializeField]
        private int maxTime = 30;

        public  Transform tr;
        Stack<Piece> piece = new Stack<Piece>();
        public void OnEnable()
        {          
        }


        public void setup(List<int> _burger,int _except,int _time)
        {
            burger.Clear();
            answer.Clear();


            burger = _burger;
            except = _except;

            for(int i=0;i< _burger.Count;i++)
            {
                if (_burger[i] != _except)
                    answer.Add(_burger[i]);
            }

            showOrder();
            GameSceneManager.Instance.setTime(maxTime);
        }


        public bool CheckAnswer(List<int> list)
        {
            if (list.Count != answer.Count)
                return false;

            //스택에서 뽑아왔기때문에 역순으로 재정렬 필요
            list.Reverse();
            for (int i=0;i<answer.Count;i++)
            {

                if (answer[i] != list[i])
                    return false;

            }
            return true;

        }


        public void showOrder()
        {
            int y = 0;
            foreach (int value in answer)
            {
                GameObject obj = Instantiate(GameSceneManager.Instance.piecePrefab[value], tr);
                Piece stuff = obj.GetComponent<Piece>();

                stuff.tr.anchoredPosition = new Vector3(0, y, 0);
                y += stuff.width;
            }
            
        }

    }

}

