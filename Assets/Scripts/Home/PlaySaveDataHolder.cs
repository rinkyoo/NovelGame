using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySaveDataHolder : MonoBehaviour
{
    private static bool created = false;
    private SaveData playSaveData;
    private int dataNumber;
    
    private CommonSaveData commonSaveData;
    
    void Awake()
    {
        //オブジェクトが重複しないように
        if(!created){
            DontDestroyOnLoad(this);
            created = true;
        }else{
            Destroy(this.gameObject);
        }
        
    }
    
    public void SetPlaySaveData(SaveData setData,int setNumber)
    {
        playSaveData = setData.Clone();
        dataNumber = setNumber;
    }
    public SaveData GetPlaySaveData()
    {
        return playSaveData;
    }
    public int GetDataNumber()
    {
        return dataNumber;
    }
    
    public void SetCommonSaveData(CommonSaveData setCommonData)
    {
        commonSaveData = setCommonData;
    }
    public CommonSaveData GetCommonSaveData()
    {
        return commonSaveData;
    }
}
