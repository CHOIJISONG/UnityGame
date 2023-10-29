using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;

public class GameSceneMove : MonoBehaviour
{
    public GameObject creat;
    public Text[] slotText;
    public TextMeshProUGUI newPlayerName;
    bool[] saveFile = new bool[3];

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.instance.path + $"{i}"))
            {
                saveFile[i] = true;
                DataManager.instance.nowSlot = i;
                DataManager.instance.LoadData();
                slotText[i].text = DataManager.instance.nowPlayer.name;
            }
            else
            {
                slotText[i].text = "비어있음";
            }
        }
        DataManager.instance.DataClear();
    }

    public void Slot(int number)
    {
        DataManager.instance.nowSlot = number;

        // 저장된 데이터가 없을 때 => 일단 우리는 게임 시작 창이 따로 없음. 그냥 바로 게임 시작
        if (saveFile[number])
        {
            DataManager.instance.LoadData();
            string sceneName = DataManager.instance.nowPlayer.sceneName;
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("Invalid scene name in saved data");
            }
        }
        else
        {
            // 저장된 데이터가 없을 때 새로운 슬롯을 생성하고 이름을 물어봅니다.
            Creat();
        }
    }

    public void Creat()
    {
        creat.gameObject.SetActive(true);
    }


    public void GameSceneCtrl()
    {
        // 새로운 슬롯에 플레이어 이름을 저장
        DataManager.instance.nowPlayer.name = newPlayerName.text;
        // 현재 씬 이름을 저장
        DataManager.instance.nowPlayer.sceneName = "Game"; // 여기에 게임 씬의 이름을 넣으세요.
        DataManager.instance.SaveData();

        // 게임 씬으로 바로 이동합니다.
        SceneManager.LoadScene("Game"); // 여기에 게임 씬의 이름을 넣으세요.
    }

    public void QuitGame()
    {
        Time.timeScale = 0;
        Debug.Log("게임 종료");
        Application.Quit();
    }
}