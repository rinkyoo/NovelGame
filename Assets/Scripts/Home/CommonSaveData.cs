[System.Serializable]
public class CommonSaveData
{
    public bool[] charaReleased = new bool[8];
    
    public CommonSaveData()
    {
        for(int i=0;i<8;i++)
        {
            charaReleased[i] = false;
        }
    }
    
    public CommonSaveData Clone()
    {
        return (CommonSaveData)MemberwiseClone();
    }
    
}
