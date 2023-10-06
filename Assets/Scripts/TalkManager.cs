using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    
    void GenerateData()
    {
        talkData.Add(1000, new string[] { "안녕?:0", "이 곳에 처음 왔구나?:1" });
        talkData.Add(2000, new string[] { "어서와.:0", "이 호수는 정말 아름답지?:1", "사실 이 호수에는 무언가의 비밀이 숨겨져 있단다.:0" });

        //Quest Talk
        talkData.Add(10 + 1000, new string[] { "어서 와.:0",
                                               "이 마을에 놀라운 전설이 있다는데:1",
                                               "오른쪽 호수 쪽에 루나가 알려줄거야.:0"});
        talkData.Add(11 + 1000, new string[] {"아직 못 만났어?:0",
                                              "루나는 오른 쪽 호수 쪽에 있어.:1"});
        talkData.Add(11 + 2000, new string[] { "안녕.:0",
                                               "이 호수의 전설을 들으러 온거야?:1",
                                               "그럼 일 좀 하나 해주면 좋을텐데...:0",
                                               "내 집 근처에 떨어진 동전 좀 주워줬으면 해.:1"});
        talkData.Add(20 + 1000, new string[] { "루나의 동전?:1",
                                               "돈을 흘리고 다니면 못쓰지!:0",
                                               "나중에 루나에게 한마디 해야겠어.:0"});
        talkData.Add(20 + 2000, new string[] { "찾으면 꼭 좀 가져다 줘...:1" });
        talkData.Add(20 + 5000, new string[] { "근처에서 동전을 찾았다." });
        talkData.Add(21 + 2000, new string[] { "엇, 찾아줘서 고마워.:1" });

        // 초상화
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(2000 + 0, portraitArr[0]);
        portraitData.Add(2000 + 1, portraitArr[1]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
                return GetTalk(id - id % 100, talkIndex); //Get First Talk
            else return GetTalk(id - id % 10, talkIndex); //Get First Quest Talk
        }

        if (talkIndex == talkData[id].Length) 
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
