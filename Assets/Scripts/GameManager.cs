using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public int stageIndex;
    public PlayerMove player;
    public GameObject[] Stages;

    public void NextStage()
    {
        // �������� ����
        if(stageIndex < Stages.Length-1) 
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else
        {
            // ���� Ŭ����
            Time.timeScale = 0;
            Debug.Log("���� ����");
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
