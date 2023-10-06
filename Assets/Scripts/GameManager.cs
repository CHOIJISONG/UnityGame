using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public int stageIndex;
    public PlayerMove player;
    public GameObject[] Stages;
    public GameObject scanObject;
    public TextMeshProUGUI talkText;
    public GameObject talkPanel;
    public bool isAction;
    public TalkManager talkManager;
    public int talkIndex;

    public Image portraitImg;

    public QuestManager questManager;

    public void Action(GameObject scanObj)
    {
        isAction = true;
        scanObject = scanObj;
        ObjData objectData = scanObject.GetComponent<ObjData>();
        Talk(objectData.id, objectData.isNpc);

        talkPanel.SetActive(isAction);
    }
    void Start()
    {
        //퀘스트 이름 출력
        //Debug.Log(questManager.CheckQuest());
    }

    void Talk(int id, bool isNpc)
    {
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

        //대화 끝날 경우
        if (talkData == null)
        {
            talkIndex = 0;
            isAction = false;
            Debug.Log(questManager.CheckQuest(id));
            return;
        }

        if (isNpc)
        {
            talkText.text = talkData.Split(':')[0]; //':'를 기준으로 배열이 두개로 나뉜다.
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
        }
        else
        {
            talkText.text = talkData;
            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isAction = true;
        talkIndex++;
    }
    public void NextStage()
    {
        // 스테이지 변경
        if(stageIndex < Stages.Length-1) 
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else
        {
            // 게임 클리어
            Time.timeScale = 0;
            Debug.Log("게임 종료");
            Application.Quit();
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = new Vector3(0, 0, -1);
    }


    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, 0);
        player.VelocityZero();
    }
}
