using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    const int dataNum = 8;
    private string SaveFilePath;
    private SaveData[] saveData = new SaveData[dataNum];
    private CommonSaveData commonSaveData;
    
    private SaveManager saveManager;
    
    void Awake()
    {
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        DoLoad();
    }
    
    public void DoLoad()
    {
        for (int i=0;i<dataNum;i++)
        {
            SaveFilePath = Application.persistentDataPath + "/SavedData" + i.ToString() + ".save";
            if (File.Exists(SaveFilePath))
            {
                // バイナリ形式でデシリアライズ
                BinaryFormatter bf = new BinaryFormatter();
                // 指定したパスのファイルストリームを開く
                FileStream file = File.Open(SaveFilePath, FileMode.Open);
                try
                {
                    // 指定したファイルストリームをSaveDataクラスにデシリアライズ。
                    saveData[i] = (SaveData)bf.Deserialize(file);
                }
                finally
                {
                    if (file != null)
                        file.Close();
                }
            }
            else
            {
                Debug.Log("no load file");
                saveData[i] = new SaveData();
            }
        }
        //共通セーブデータ
        SaveFilePath = Application.persistentDataPath + "/CommonSavedData.save";
        if (File.Exists(SaveFilePath))
        {
            // バイナリ形式でデシリアライズ
            BinaryFormatter bf = new BinaryFormatter();
            // 指定したパスのファイルストリームを開く
            FileStream file = File.Open(SaveFilePath, FileMode.Open);
            try
            {
                // 指定したファイルストリームをSaveDataクラスにデシリアライズ。
                commonSaveData = (CommonSaveData)bf.Deserialize(file);
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
        }
        else
        {
            Debug.Log("no load file");
            commonSaveData = new CommonSaveData();
        }
    }
    
    public void SetSaveData(SaveData setData,int dataNumber)
    {
        saveData[dataNumber] = setData.Clone();
        saveManager.SaveNewData(saveData[dataNumber],dataNumber,commonSaveData);
        
    }
    public SaveData GetSaveData(int i)
    {
        return saveData[i];
    }
    
    public string GetChapter(int i)
    {
        return saveData[i].chapter;
    }
    public string GetScene(int i)
    {
        return saveData[i].scene;
    }
    
    public void SetCommonSaveData(CommonSaveData hoge)
    {
        commonSaveData = hoge;
    }
    public CommonSaveData GetCommonSaveData()
    {
        return commonSaveData;
    }
}
