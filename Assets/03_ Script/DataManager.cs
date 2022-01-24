using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace CA
{
    [System.Serializable]
    public enum Lv
    {
        easy,
        normal,
        hard
    }



    [System.Serializable]
    public class Map
    {
        public string name;
        public int time;
        public int except;
        public List<int> burger;
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


        private Map badMap = null;
        public TextAsset burger_csv;

        //public List<Map> mapPool = new List<Map>();
        public Dictionary<Lv, List<Map>> maps = new Dictionary<Lv, List<Map>>();
        public void load()
        {
            burger_csv = Resources.Load<TextAsset>("crab_burger_ex");



            string data = burger_csv.text;

            string[] lineData = data.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < lineData.Length; ++i)
            {
                string[] cell = lineData[i].Split(new char[] { ',' });

                //npc 이름
                string name = cell[1];


                //난이도
                Lv lv = (Lv)System.Enum.Parse(typeof(Lv), cell[2]);

                int except=0;
                //제외 재료
                if (cell[3] == "")
                    except = 0;
                else
                    except = (int)(Piece.Type)System.Enum.Parse(typeof(Piece.Type), cell[3]);


                List<int> list = new List<int>();

                // 타입
                for (int j=4;j<cell.Length;j++)
                {
                    if (cell[j] == "")
                        continue;
                    Piece.Type type = (Piece.Type)System.Enum.Parse(typeof(Piece.Type), cell[j]);
                    list.Add((int)type);
                   
                }

              

                Map map = new Map();
                map.burger = list;
                map.time = 10;
                map.name = name;
                map.except = except;

                if(!maps.ContainsKey(lv))
                {
                    maps.Add(lv, new List<Map>());
                }

                maps[lv].Add(map);

            }

            badMap = new Map();
            List<int> _list = new List<int>();
            _list.Add((int)Piece.Type.BadOrder);
            badMap.burger = _list;
            badMap.time = 2;
            badMap.name = "JIN SANG";
            badMap.except = 0;




          

        }


        public Map GetMap(Lv _lv)
        {
            
            int rand = Random.Range(1,101);

            Lv yourRandomLv = Lv.easy;
            switch (_lv)
            {
                case Lv.easy:
                    yourRandomLv = Lv.easy;
                    break;
                case Lv.normal:
                    if (rand > 50)
                        yourRandomLv = Lv.normal;
                    else
                        yourRandomLv = Lv.easy;
                    break;
                case Lv.hard:
                    if (rand > 66)
                        yourRandomLv = Lv.hard;
                    else if (rand > 33)
                        yourRandomLv = Lv.normal;
                    else
                        yourRandomLv = Lv.easy;
                    break;
            }





            int number = Random.Range(0, maps[yourRandomLv].Count);

            int bad = Random.Range(1, 101);

            Map map = new Map();

            if (bad <= 15)
            {
                return badMap;
            }


            return maps[yourRandomLv][number];
        }
       
        public void makeMap()
        {
            StartCoroutine(makeRandomMap());
        }

        IEnumerator makeRandomMap()
        {

            int count = 1;
            while(true)
            {
                Map randMap = new Map();
                List<int> _list = new List<int>();

                //랜덤..일단... 음... 7단~12단
                int wid = Random.Range(5, 8); ;
                
                

                //예의상 빵은 추가
                _list.Add((int)Piece.Type.Bun);
                for(int i=0;i<wid;i++)
                {
                    //전부 랜덤
                    _list.Add(Random.Range((int)Piece.Type.Bun, (int)Piece.Type.BadOrder));
                }
                _list.Add((int)Piece.Type.Bun);

                randMap.burger = _list;
                randMap.time = 10;
                randMap.name = string.Format("Ranadom {0:0000}", count);

                
                //빼주세요도 랜덤
                randMap.except = Random.Range((int)Piece.Type.None,(int)Piece.Type.BadOrder);

                //1초에 하나씩 랜덤한 맵 생성
                yield return new WaitForSeconds(1.0f);


                maps[Lv.hard].Add(randMap);
                count++;
            }
         
        }





    }


}
