using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveManager : MonoBehaviour
{
    const int dataNum = 8;
    private string SaveFilePath;
    
    public void SaveNewData(SaveData setData,int dataNumber,CommonSaveData commonSaveData)
    {
        SaveFilePath = Application.persistentDataPath + "/SavedData" + dataNumber.ToString() + ".save";
        // バイナリ形式でシリアル化
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(SaveFilePath);
        try
        {
            bf.Serialize(file, setData);
        }
        finally
        {
            if (file != null)
                file.Close();
        }
        
        SaveFilePath = Application.persistentDataPath + "/CommonSavedData.save";
        // バイナリ形式でシリアル化
        bf = new BinaryFormatter();

        file = File.Create(SaveFilePath);
        try
        {
            bf.Serialize(file, commonSaveData);
        }
        finally
        {
            if (file != null)
                file.Close();
        }
    }
}
