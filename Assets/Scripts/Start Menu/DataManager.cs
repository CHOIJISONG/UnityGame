using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

// 저장하는 방법
// 1. 저장할 데이터가 존재하는가
// 2. 데이터를 json으로 변환
// 3. json 외부에 저장

// 불러오는 방법
// 1. 외부에 저장된 json 불러오기
// 2. 제이슨을 데이터형태로 변환
// 3. 불러온 데이터를 사용

public class PlayerData
{
    // 스테이지
    public string name;
    public int level;
    public string sceneName;
}

public class DataManager : MonoBehaviour
{
    //싱글톤
    public static DataManager instance;

    public PlayerData nowPlayer = new PlayerData();

    public string path;
    public int nowSlot;

    private void Awake()
    {
        #region 싱글톤
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

        // 씬이 로드될 때마다 현재 씬 이름 업데이트
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    // 씬 로드 시 호출될 이벤트 핸들러
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