using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameSceneMove : MonoBehaviour
{
    public GameObject creat;
    public Text[] slotText;

    bool[] saveFile;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            if (File.Exists(DataManager.instance.path + $"{i}"))
            {
                saveFile[i] = true;
                DataManager.instance.nowSlot = i;
                DataManager.instance.LoadData();
                slotText[i].text = DataManager.instance.nowPlayer.level;
            }
        }
        DataManager.instance.DataClear();
    }

    public void Slot(int number)
    {
        DataManager.instance.nowSlot = number;

        // 저장된 데이터가 없을 때 => 일단 우리는 게임 시작 창이 따로 없음. 그냥 바로 게임 시작

        // 저장된 데이터가 있을 때
        DataManager.instance.LoadData();
        GameSceneCtrl();
    }
    public void Creat()
    {
        creat.gameObject.SetActive(true);
    }


    public void GameSceneCtrl()
    {
        SceneManager.LoadScene("Game"); // 어떤 씬으로?
    }

    public void QuitGame()
    {
        Time.timeScale = 0;
        Debug.Log("게임 종료");
        Application.Quit();
    }
}
