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
                slotText[i].text = "�������";
            }
        }
        DataManager.instance.DataClear();
    }

    public void Slot(int number)
    {
        DataManager.instance.nowSlot = number;

        // ����� �����Ͱ� ���� �� => �ϴ� �츮�� ���� ���� â�� ���� ����. �׳� �ٷ� ���� ����
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
            // ����� �����Ͱ� ���� �� ���ο� ������ �����ϰ� �̸��� ����ϴ�.
            Creat();
        }
    }

    public void Creat()
    {
        creat.gameObject.SetActive(true);
    }


    public void GameSceneCtrl()
    {
        // ���ο� ���Կ� �÷��̾� �̸��� ����
        DataManager.instance.nowPlayer.name = newPlayerName.text;
        // ���� �� �̸��� ����
        DataManager.instance.nowPlayer.sceneName = "Game"; // ���⿡ ���� ���� �̸��� ��������.
        DataManager.instance.SaveData();

        // ���� ������ �ٷ� �̵��մϴ�.
        SceneManager.LoadScene("Game"); // ���⿡ ���� ���� �̸��� ��������.
    }

    public void QuitGame()
    {
        Time.timeScale = 0;
        Debug.Log("���� ����");
        Application.Quit();
    }
}