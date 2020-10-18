using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveManager : MonoBehaviour
{
    const int dataNum = 8;
    private string SaveFilePath;
    //private SaveData[] saveData = new SaveData[dataNum];
    
    void Start()
    {
        
    }
    
    public void SaveNewData(SaveData setData,int dataNumber,CommonSaveData commonSaveData)
    {
        // セーブデータ作成
        //SaveData saveData = CreateSaveData();
        SaveFilePath = Application.persistentDataPath + "/SavedData" + dataNumber.ToString() + ".save";
        // バイナリ形式でシリアル化
        BinaryFormatter bf = new BinaryFormatter();
        // 指定したパスにファイルを作成
        FileStream file = File.Create(SaveFilePath);
        // Closeが確実に呼ばれるように例外処理を用いる
        try
        {
            // 指定したオブジェクトを上で作成したストリームにシリアル化する
            bf.Serialize(file, setData);
        }
        finally
        {
            // ファイル操作には明示的な破棄が必要です。Closeを忘れないように。
            if (file != null)
                file.Close();
        }
        
        SaveFilePath = Application.persistentDataPath + "/CommonSavedData.save";
        // バイナリ形式でシリアル化
        bf = new BinaryFormatter();
        // 指定したパスにファイルを作成
        file = File.Create(SaveFilePath);
        // Closeが確実に呼ばれるように例外処理を用いる
        try
        {
            // 指定したオブジェクトを上で作成したストリームにシリアル化する
            bf.Serialize(file, commonSaveData);
        }
        finally
        {
            // ファイル操作には明示的な破棄が必要です。Closeを忘れないように。
            if (file != null)
                file.Close();
        }
    }

    // 入力された情報をもとにセーブデータを作成
    /*
    private SaveData CreateSaveData()
    {
        SaveData saveData = new SaveData();
        saveData.scenarioNumber = this.scenarioNumber;
        saveData.sceneNumber = this.sceneNumber;
        return saveData;
    }
     */
}
