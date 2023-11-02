using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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

   
    public TextMeshProUGUI name;
    public GameObject menuSet;

    public int monsterKill;

    public GameObject finishObject;



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
        
        finishObject.SetActive(false);

    }
    void Update()
    {
        //submenu
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuSet.activeSelf)
                menuSet.SetActive(false);
            else
                menuSet.SetActive(true);

        }
        NextStage();


    }
    void Talk(int id, bool isNpc)
    {
        
        string talkData = talkManager.GetTalk(id , talkIndex);

        //대화 끝날 경우
        if (talkData == null)
        {
            talkIndex = 0;
            isAction = false;
           
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
         //씬별 조건에 따라 오브젝트 활성화

        if (SceneManager.GetActiveScene().name == "Game")
        {

            if (monsterKill >= 1)
            {

                finishObject.SetActive(true);
            }
            
        }
        else if (SceneManager.GetActiveScene().name == "Game 2")
        {
            if (monsterKill >= 4)
            {

                finishObject.SetActive(true);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Game 3")
        {
            if (monsterKill >= 4)
            {

                finishObject.SetActive(true);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Game 4")
        {
            if (monsterKill >= 5)
            {

                finishObject.SetActive(true);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Game 5")
        {
            if (monsterKill >= 1)
            {

                finishObject.SetActive(true);
            }
        }
        else if (SceneManager.GetActiveScene().name == "Game Talk2" || SceneManager.GetActiveScene().name == "Game Talk3" || SceneManager.GetActiveScene().name == "Game Talk4")
        {
           
                finishObject.SetActive(true);
            
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
    public void save()
    {
        DataManager.instance.SaveData();
    }
    public void title()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
