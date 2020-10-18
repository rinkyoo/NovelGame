[System.Serializable]
public class SaveData
{
    public string chapter;
    public string scene;
    
    public SaveData()
    {
        chapter = "001";
        scene = "001";
        
    }
    
    public SaveData Clone()
    {
        return (SaveData)MemberwiseClone();
    }
    
}
