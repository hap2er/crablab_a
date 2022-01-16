using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CA
{
    public class Npc : MonoBehaviour
    {
        [SerializeField]
        private List<int> answer = new List<int>();

        [SerializeField]
        private int maxTime = 30;

        public  Transform tr;
        Stack<Piece> piece = new Stack<Piece>();
        public void OnEnable()
        {
            answer.Add(0);
            answer.Add(2);
            answer.Add(1);
            answer.Add(2);
            answer.Add(0);
            answer.Add(1);

            piece.Clear();


            
            showAnswer();
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


        public void showAnswer()
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

