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

        talkData.Add(1000, new string[] { "�̹��� �����Ϸ� �� 5�г� �л��ΰ���?:0",
                                          "�����Ϸ��� ������ ������ ����ؾ� �Ѵٴ� ���� �˰� ����?:1" ,
                                          "ù ��° �����Դϴ�.\r\n �б� �ֺ��� �л����� �����ϴ� ������ 4������ ��ƿ�����.:0",
                                          "�ڵ� ���� ����� ������ �̵� �� ESC�� ������ �����ϱ⸦ �� ������\r\n �մϴ�.:1",
                                          "�����ص� ���͸� �����߸� ���� ������� �ʰ� ��ġ�� ����Ǵ� ����\r\n ������ּ���.:0",
                                          "���� ���� �� �����ּ���. �׷� ����� ���ϴ�.:1"  });

        talkData.Add(2000, new string[] { "�����ϼ̽��ϴ�. Ȯ���� �⺻���� �Ƿ��� �����ó׿�.:0",
                                          "�� ��° �����Դϴ�. ������ ����, ���� �� ������ ��ƿ�����.:1",
                                          "���� ���� �����Ӻ��� ����ϱ� ��ٷο�� �����ϼ���. \r\n����� ���ϴ�.:0" });

        talkData.Add(3000, new string[] { "�����ϼ̽��ϴ�. �� ��° �������� ������ �޼��ϼ̳׿�.:0",
                                          "������ �����Դϴ�. ���� �� ������ ��� ������ ���� �̰ܿ�����.:1",
                                          "������ ������ �޼��� ��, �����Բ� ���� ������ ġ���� �˴ϴ�. \r\n����� ���ϴ�.:0" });

        talkData.Add(4000, new string[] { "�ڳװ� ���� ������ ��� �޼��� 5�г� �л��̱�. :0",
                                          "������ �� �� �ִ� �ڰ��� �������� ���ø� ���Ѵٸ� ���Ƿ� �����Գ�. :1"  });




        // �ʻ�ȭ
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
