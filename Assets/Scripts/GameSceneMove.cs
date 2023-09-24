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

        // ����� �����Ͱ� ���� �� => �ϴ� �츮�� ���� ���� â�� ���� ����. �׳� �ٷ� ���� ����

        // ����� �����Ͱ� ���� ��
        DataManager.instance.LoadData();
        GameSceneCtrl();
    }
    public void Creat()
    {
        creat.gameObject.SetActive(true);
    }


    public void GameSceneCtrl()
    {
        SceneManager.LoadScene("Game"); // � ������?
    }

    public void QuitGame()
    {
        Time.timeScale = 0;
        Debug.Log("���� ����");
        Application.Quit();
    }
}
