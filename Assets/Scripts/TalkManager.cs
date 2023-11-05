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

        talkData.Add(1000, new string[] { "이번에 졸업하러 온 5학년 학생인가요?:0",
                                          "졸업하려면 과제와 시험을 통과해야 한다는 것은 알고 있죠?:1" ,
                                          "첫 번째 과제입니다.\r\n 학교 주변에 학생들을 위협하는 슬라임 4마리를 잡아오세요.:0",
                                          "자동 저장 기능이 없으니 이동 시 ESC를 눌러서 저장하기를 꼭 눌러야\r\n 합니다.:1",
                                          "저장해도 몬스터를 쓰러뜨린 수는 저장되지 않고 위치만 저장되는 것을\r\n 명심해주세요.:0",
                                          "잊지 말고 꼭 눌러주세요. 그럼 행운을 빕니다.:1"  });

        talkData.Add(2000, new string[] { "수고하셨습니다. 확실히 기본적인 실력은 있으시네요.:0",
                                          "두 번째 과제입니다. 슬라임 마리, 얼음 골렘 마리를 잡아오세요.:1",
                                          "얼음 골렘은 슬라임보다 상대하기 까다로우니 조심하세요. \r\n행운을 빕니다.:0" });

        talkData.Add(3000, new string[] { "수고하셨습니다. 두 번째 과제까지 무사히 달성하셨네요.:0",
                                          "마지막 과제입니다. 얼음 골렘 마리를 잡고 마법사 명을 이겨오세요.:1",
                                          "마지막 과제를 달성한 후, 교수님께 가서 시험을 치르면 됩니다. \r\n행운을 빕니다.:0" });

        talkData.Add(4000, new string[] { "자네가 졸업 과제를 모두 달성한 5학년 학생이군. :0",
                                          "시험을 볼 수 있는 자격이 생겼으니 응시를 원한다면 교실로 들어오게나. :1"  });




        // 초상화
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);

        portraitData.Add(2000 + 0, portraitArr[0]);
        portraitData.Add(2000 + 1, portraitArr[1]);

        portraitData.Add(3000 + 0, portraitArr[0]);
        portraitData.Add(3000 + 1, portraitArr[1]);

        portraitData.Add(4000 + 0, portraitArr[2]);
        portraitData.Add(4000 + 1, portraitArr[3]);
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
