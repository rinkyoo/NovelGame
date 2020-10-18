using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour
{
    //セーブデータ関連***************
    [SerializeField] private GameObject SaveDataManager;
    private SaveDataManager sdManager;
    private PlaySaveDataHolder psdHolder;
    private string SaveFilePath;
    private SaveData saveData;
    private CommonSaveData commonSaveData;
    //Playによって更新されるSaveData
    private SaveData nowData;
    private CommonSaveData nowCommonData;
    //*****************************
    public SceneController sc;
    
    Dictionary<string,int> charaNumber = new Dictionary<string,int>()
    {
        {"chara1",0},
        {"chara2",1},
        {"chara3",2},
        {"chara4",3},
        {"chara5",4},
        {"chara6",5},
        {"chara7",6},
        {"chara8",7},
    };
    
    void Awake()
    {
        sdManager = SaveDataManager.GetComponent<SaveDataManager>();
        psdHolder = GameObject.Find("PlaySaveDataHolder").GetComponent<PlaySaveDataHolder>();
        saveData = psdHolder.GetPlaySaveData();
        nowData = saveData.Clone();
        commonSaveData = psdHolder.GetCommonSaveData();
        nowCommonData = commonSaveData.Clone();
        sc = new SceneController(this);
    }
    
    void Start ()
    {
        SetFirstScene();
    }

    void Update()
    {
        sc.WaitClick();
        sc.SetComponents();
    }
    //*****************************************:
    void SetFirstScene()
    {
        sc.LoadChapter(saveData.chapter);
        sc.SetScene(saveData.scene);
    }
    
    public void ChangeChapter(string id)
    {
        sc.LoadChapter(id);
        sc.SetScene("001"); //chapterの最初は001の前提処理
    }
    
    public void SetNowData(SaveData setData)
    {
        nowData = setData.Clone();
    }
    public void SetNowChapter(string id)
    {
        nowData.chapter = id;
    }
    public void SetNowScene(string id)
    {
        nowData.scene = id;
    }
    public void CharaRelease(string name)
    {
        nowCommonData.charaReleased[charaNumber[name]] = true;
    }
    public void SetSaveData(int saveDataNum)
    {
        sdManager.SetCommonSaveData(nowCommonData);
        sdManager.SetSaveData(nowData,saveDataNum);
    }
    public string GetChapter(int i){return sdManager.GetChapter(i);}
    public string GetScene(int i){return sdManager.GetScene(i);}
    public void UpdateSaveData(){sdManager.DoLoad();}

}
