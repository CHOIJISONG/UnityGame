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

        talkData.Add(1000, new string[] { "����� �����Ϸ��� �� 5�г� �л��� �ڳ��ΰ�?:0",
                                          "�����ϱ� ���ؼ��� ������ ������ �ִٴ� ���� �˰� �ְ���?:1" ,
                                          "ù ��° ������ �ְڽ��ϴ�. �б� �ֺ��� �л����� �����ϴ� ������ 4������ ��ƿ�����.:0",
                                          "�� �ڵ� ���� ����� �����ϱ� �� �̵��� ESC�� ������ �����ϱ⸦ �� ��������!!\r\n�����Ѵٰ� �ص� ���� óġ�� ī��Ʈ �ȵǰ� ��ġ�� ����ٴ°� �����!!:1",
                                          "�ȴ����� �ʸ� ���ؾ�!! �����϶��:0"  });

        talkData.Add(2000, new string[] { "�⺻���� �Ƿ��� �����ñ���.:0",
                                          "�������� ������ ����, ���� ��  ���� ��ƿ�����.?:1",
                                          "���� ���� ������ ���ٴ� �����Ŵ� �����ϼ���.:0" });

        talkData.Add(3000, new string[] { "�� ��° ������ �����߱���...:0",
                                          "�׷� ������� �� ������ �����ð� ������ ���� �̰ܿ�����. :1",
                                          "���������� ��������. �׷� ���� ������ ġ�� �� �־��.:0" });

        talkData.Add(4000, new string[] { "�ڳװ� ���� ���� 3���� �� ���� �л��ΰ�.... :0",
                                          "���� ������ ġ�� �� �ִ� �ڰ��� ���屸�� ���� ������ ġ��� ������ ���Ƿ� ���Գ�. :1"  });




        // �ʻ�ȭ
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
