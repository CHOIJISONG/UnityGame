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

        talkData.Add(1000, new string[] { "요번에 졸업하려고 온 5학년 학생이 자네인가?:0",
                                          "졸업하기 위해서는 과제와 시험이 있다는 것은 알고 있겠지?:1" ,
                                          "첫 번째 과제를 주겠습니다. 학교 주변에 학생들을 위협하는 슬라임 4마리좀 잡아오세요.:0",
                                          "참 자동 저장 기능이 없으니깐 꼭 이동시 ESC를 눌러서 저장하기를 꼭 눌러야해!!\r\n저장한다고 해도 몬스터 처치는 카운트 안되고 위치만 저장다는거 명심해!!:1",
                                          "안누르면 너만 손해야!! 수고하라고:0"  });

        talkData.Add(2000, new string[] { "기본적인 실력은 있으시군요.:0",
                                          "다음으로 슬라임 마리, 얼음 골렘  마리 잡아오세요.?:1",
                                          "얼음 골렘은 슬라임 보다는 어려울거니 조심하세요.:0" });

        talkData.Add(3000, new string[] { "두 번째 과제를 성공했군요...:0",
                                          "그럼 요번에는 골렘 마리를 잡으시고 마법사 명을 이겨오세요. :1",
                                          "교수님한테 가보세요. 그럼 졸업 시험을 치룰 수 있어요.:0" });

        talkData.Add(4000, new string[] { "자네가 졸업 과제 3개를 다 끝낸 학생인가.... :0",
                                          "졸업 시험을 치룰 수 있는 자격이 생겼구만 졸업 시험을 치루고 싶으면 교실로 오게나. :1"  });




        // 초상화
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);

        portraitData.Add(2000 + 0, portraitArr[0]);
        portraitData.Add(2000 + 1, portraitArr[1]);

        portraitData.Add(3000 + 0, portraitArr[0]);
        portraitData.Add(3000 + 1, portraitArr[1]);

        portraitData.Add(4000 + 0, portraitArr[0]);
        portraitData.Add(4000 + 1, portraitArr[1]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        
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
