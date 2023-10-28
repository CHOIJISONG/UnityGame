using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

// �����ϴ� ���
// 1. ������ �����Ͱ� �����ϴ°�
// 2. �����͸� json���� ��ȯ
// 3. json �ܺο� ����

// �ҷ����� ���
// 1. �ܺο� ����� json �ҷ�����
// 2. ���̽��� ���������·� ��ȯ
// 3. �ҷ��� �����͸� ���

public class PlayerData
{
    // ��������
    public string level;

}

public class DataManager : MonoBehaviour
{
    //�̱���
    public static DataManager instance;

    public PlayerData nowPlayer = new PlayerData();

    public string path;
    public int nowSlot;

    private void Awake()
    {
        #region �̱���
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion

        path = Application.persistentDataPath + "/save";
    }


    void Start()
    {
       
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(nowPlayer);
        File.WriteAllText(path + nowSlot.ToString(), data);
    }
    public void LoadData()
    {
        string data = File.ReadAllText(path + nowSlot.ToString());
        JsonUtility.FromJson<PlayerData>(data);
    }
    public void DataClear()
    {
        nowSlot = -1;
        nowPlayer = new PlayerData();
    }

}
