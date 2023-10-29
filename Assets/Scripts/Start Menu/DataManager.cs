using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

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
    public string name;
    public int level;
    public string sceneName;
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
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion

        path = Application.persistentDataPath + "/save";
        print(path);

        // ���� �ε�� ������ ���� �� �̸� ������Ʈ
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    // �� �ε� �� ȣ��� �̺�Ʈ �ڵ鷯
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        nowPlayer.sceneName = scene.name;
    }
    public void SaveData()
    {
        string data = JsonUtility.ToJson(nowPlayer);
        File.WriteAllText(path + nowSlot.ToString(), data);
    }
    public void LoadData()
    {
        string data = File.ReadAllText(path + nowSlot.ToString());
        nowPlayer = JsonUtility.FromJson<PlayerData>(data);
    }
    public void DataClear()
    {
        nowSlot = -1;
        nowPlayer = new PlayerData();
    }

}