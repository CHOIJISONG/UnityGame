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
        talkData.Add(1000, new string[] { "�ȳ�?:0", "�� ���� ó�� �Ա���?:1" });
        talkData.Add(2000, new string[] { "���.:0", "�� ȣ���� ���� �Ƹ�����?:1", "��� �� ȣ������ ������ ����� ������ �ִܴ�.:0" });

        //Quest Talk
        talkData.Add(10 + 1000, new string[] { "� ��.:0",
                                               "�� ������ ���� ������ �ִٴµ�:1",
                                               "������ ȣ�� �ʿ� �糪�� �˷��ٰž�.:0"});
        talkData.Add(11 + 1000, new string[] {"���� �� ������?:0",
                                              "�糪�� ���� �� ȣ�� �ʿ� �־�.:1"});
        talkData.Add(11 + 2000, new string[] { "�ȳ�.:0",
                                               "�� ȣ���� ������ ������ �°ž�?:1",
                                               "�׷� �� �� �ϳ� ���ָ� �����ٵ�...:0",
                                               "�� �� ��ó�� ������ ���� �� �ֿ������� ��.:1"});
        talkData.Add(20 + 1000, new string[] { "�糪�� ����?:1",
                                               "���� �긮�� �ٴϸ� ������!:0",
                                               "���߿� �糪���� �Ѹ��� �ؾ߰ھ�.:0"});
        talkData.Add(20 + 2000, new string[] { "ã���� �� �� ������ ��...:1" });
        talkData.Add(20 + 5000, new string[] { "��ó���� ������ ã�Ҵ�." });
        talkData.Add(21 + 2000, new string[] { "��, ã���༭ ����.:1" });

        // �ʻ�ȭ
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
