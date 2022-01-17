using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CA
{
    public class Map
    {
        public string name;
        public int time;
        public List<int> answer;
    }





    public class DataManager : MonoBehaviour
    {
        #region Instance

        private static DataManager _instance = null;

        public static DataManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<DataManager>();
                if(_instance == null)
                {
                    GameObject obj = new GameObject("DataManager");
                    _instance = obj.AddComponent<DataManager>();

                }
                return _instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        #endregion



        public TextAsset burger_csv;

        public List<Map> mapPool = new List<Map>();
        private void load()
        {
            burger_csv = Resources.Load<TextAsset>("Data/Burger");



            string data = burger_csv.text;

            string[] lineData = data.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < lineData.Length; ++i)
            {
                string[] cell = lineData[i].Split(new char[] { ',' });

                //npc 이름
                string name = cell[0];

                //제한 시간
                int time = int.Parse(cell[1]);

                List<int> list = new List<int>();

                // 타입
                for(int j=2;j<cell.Length;j++)
                {
                    Piece.Type type = (Piece.Type)System.Enum.Parse(typeof(Piece.Type), cell[0]);
                    list.Add((int)type);
                }


                Map map = new Map();
                map.answer = list;
                map.time = time;
                map.name = name;

                mapPool.Add(map);

            }
        }
    }


}
